using System.Text;
using System.Collections.Generic;
using Antlr3.ST;
using System.IO;
using System;

namespace Core
{
    /// <summary>
    /// Třída představuje obecného správce specifických
    /// bibliografických údajů pro konkrétní typ publikace.
    /// </summary>
    public abstract class APublicationModel
    {
        /// <summary>
        /// Uchovává databázový kontext.
        /// </summary>
        protected DbPublicationEntities Context;

        /// <summary>
        /// Uchovává stručný popis typu publikace pro výpis v uživatelském rozhraní..
        /// </summary>
        public readonly string TypeDescription;

        /// <summary>
        /// Uchovává cestu k výchozí šabloně pro export do HTML dokumentu.
        /// </summary>
        public readonly string DefaultTemplate;

        /// <summary>
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        /// <param name="typeDescription">popis typu publikace</param>
        /// <param name="defaultTemplate">výchozí HTML šablona typu publikace</param>
        public APublicationModel(DbPublicationEntities context, string typeDescription, string defaultTemplate)
        {
            DefaultTemplate = defaultTemplate;
            TypeDescription = typeDescription;
            Context = context;
        }
        
        /// <summary>
        /// Z bibliografických údajů zadané publikace
        /// vygeneruje citaci podle ISO normy.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <returns>citace podle ISO normy</returns>
        public abstract string GeneratePublicationIsoCitation(Publication publication);

        /// <summary>
        /// Z bibliografických údajů zadané publikace
        /// vygeneruje BibTeX záznam odpovídající citaci podle ISO normy.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <returns>BibTeX záznam</returns>
        public abstract string GeneratePublicationBibtexEntry(Publication publication);

        /// <summary>
        /// Pro zadanou publikaci sestaví HTML dokument pro umístění publikace na webové stránky.
        /// V případě nezadání cesty k výstupnímu souboru vrátí řetězec pro případný výpis na obrazovku.
        /// V případě zadání provede zápis a vrátí NULL.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <param name="templatePath">cesta k šabloně</param>
        /// <param name="htmlPath">cesta k HTML dokumentu</param>
        /// <returns>obsah HTML dokumentu nebo NULL v případě zápisu do souboru</returns>
        public abstract string ExportPublicationToHtmlDocument(Publication publication, string templatePath, string htmlPath);
        
        /// <summary>
        /// Vygeneruje řetězec autorů publikace pro citaci ze seznamu autorů.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <returns>řetězec pro citaci</returns>
        protected string GenerateAuthorCitationString(Publication publication)
        {
            Queue<Author> authors = new Queue<Author>(publication.Author);
            StringBuilder sb = new StringBuilder();

            // výpis prvního jména
            Author author = authors.Dequeue();
            sb.Append(author.Surname.ToUpper()).Append(", ").Append(author.Name);

            // výpis prostředních jmen
            while (authors.Count > 1)
            {
                author = authors.Dequeue();
                sb.Append(", ").Append(author.Surname.ToUpper()).Append(", ").Append(author.Name);
            }

            // výpis posledního jména
            author = authors.Dequeue();
            sb.Append(" a ").Append(author.Surname.ToUpper()).Append(", ").Append(author.Name).Append(". ");

            return sb.ToString();
        }

        /// <summary>
        /// Vygeneruje řetězec autorů publikace pro BibTeX záznam ze seznamu autorů.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <returns>řetězec pro BibTeX záznam</returns>
        protected string GenerateAuthorBibtexString(Publication publication)
        {
            Queue<Author> authors = new Queue<Author>(publication.Author);
            StringBuilder sb = new StringBuilder("author={");

            // výpis prvního jména
            Author author = authors.Dequeue();
            sb.Append(author.Name).Append(" ").Append(author.Surname);

            // výpis dalších jmen
            while (authors.Count > 0)
            {
                author = authors.Dequeue();
                sb.Append(",").Append(author.Name).Append(" ").Append(author.Surname);
            }
            
            sb.Append("},");

            return sb.ToString();
        }

        /// <summary>
        /// Načte šablonu ze souboru na zadané cestě (nebo z výchozího souboru, pokud nebyla zadána)
        /// a vypíše do ní základní bibliografické údaje pro HTML export.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <param name="publicationType">popis typu publikace</param>
        /// <param name="defaultTemplate">výchozí HTML šablona typu publikace</param>
        /// <param name="templatePath">cesta k šabloně</param>
        /// <returns>připravená šablona</returns>
        protected StringTemplate LoadHtmlTemplate(Publication publication, string templatePath)
        {
            string template = null;

            try
            {
                // načtení šablony ze souboru
                File.ReadAllText(Path.GetFullPath(
                    string.IsNullOrWhiteSpace(templatePath) ? DefaultTemplate : templatePath));
            }
            catch (Exception e)
            {
                throw new PublicationException("Chyba při čtení vstupního souboru šablony: " + e.Message);
            }
            
            // vyplnění základních údajů pro všechny typy publikací
            StringTemplate stringTemplate = new StringTemplate(template);
            stringTemplate.SetAttribute("title", publication.Title);
            stringTemplate.SetAttribute("authors", GenerateAuthorCitationString(publication));
            stringTemplate.SetAttribute("year", publication.Year);
            stringTemplate.SetAttribute("type", TypeDescription);
            stringTemplate.SetAttribute("text", publication.Text);

            return stringTemplate;
        }

        /// <summary>
        /// Uloží ze šablony vygenerovaný HTML dokument do výstupního souboru na zadané cestě.
        /// </summary>
        /// <param name="stringTemplate">vyplněná šablona</param>
        /// <param name="htmlPath">cesta k dokumentu</param>
        /// <returns>obsah HTML dokumentu nebo NULL v případě zápisu do souboru</returns>
        protected string SaveHtmlDocument(StringTemplate stringTemplate, string htmlPath)
        {
            string html = stringTemplate.ToString();
            
            // vrácení hotového HTML dokumentu, pokud nebyl zadán výstupní soubor
            if (string.IsNullOrWhiteSpace(htmlPath))
            {
                return html;
            }

            try
            {
                // zápis HTML do souboru
                File.WriteAllText(Path.GetFullPath(htmlPath), html);
            }
            catch (Exception e)
            {
                throw new PublicationException("Chyba při zápisu výstupního HTML dokumentu: " + e.Message);
            }

            return null;
        }

        /// <summary>
        /// Načte základní údaje o publikaci se zadaným ID.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace</returns>
        protected Publication GetPublication(int id)
        {
            return Context.Publication.Find(id);
        }

        /// <summary>
        /// Vytvoří novou publikaci se zadanými údaji.
        /// </summary>
        /// <param name="publication">údaje publikace</param>
        protected void CreatePublication(Publication publication, List<Author> authors)
        {
            Context.Publication.Add(publication);

            if (authors == null || authors.Count == 0)
            {
                throw new PublicationException("Publikace musí mít alespoň jednoho autora.");
            }

            // přidání autorů
            foreach (Author a in authors)
            {
                publication.Author.Add(a);
                a.Publication.Add(publication);
            }
        }

        /// <summary>
        /// Upraví zadanou publikaci zadanými údaji.
        /// </summary>
        /// <param name="oldPublication">existující publikace</param>
        /// <param name="publication">nové údaje</param>
        protected void UpdatePublication(Publication oldPublication, Publication publication, List<Author> authors)
        {
            // ponechání původního BibTeX klíče, pokud nový nebyl vyplněn
            if (publication.Entry != null)
            {
                oldPublication.Entry = publication.Entry;
            }

            // ponechání původního názvu publikace, pokud nový nebyl vyplněn
            if (publication.Title != null)
            {
                oldPublication.Title = publication.Title;
            }

            // ukončení úprav, pokud nebyl vyplněn nový seznam autorů
            if (authors == null || authors.Count == 0)
            {
                return;
            }
            
            // odstranění stávajícího seznamu autorů
            foreach (Author a in oldPublication.Author)
            {
                a.Publication.Remove(oldPublication);
            }

            oldPublication.Author.Clear();

            // vytvoření nového seznamu autorů
            foreach (Author a in authors)
            {
                publication.Author.Add(a);
                a.Publication.Add(publication);
            }
        }

        /// <summary>
        /// Odstraní zadanou publikaci.
        /// </summary>
        /// <param name="oldPublication">existující publikace</param>
        protected void DeletePublication(Publication oldPublication)
        {
            // odstranění publikace u autorů
            foreach (Author author in oldPublication.Author)
            {
                author.Publication.Remove(oldPublication);
            }

            Context.Publication.Remove(oldPublication);
        }
    }
}
