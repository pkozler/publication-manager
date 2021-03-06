﻿using System;
using System.Text;
using System.Collections.Generic;
using Antlr3.ST;
using System.IO;

namespace Core
{
    /// <summary>
    /// Třída představuje obecného správce specifických
    /// bibliografických údajů pro konkrétní typ publikace.
    /// </summary>
    public abstract class APublicationModel
    {
        /// <summary>
        /// Uchovává příponu výchozích šablon pro vytváření HTML dokumentů.
        /// </summary>
        public const string DEFAULT_TEMPLATE_EXTENSION = "st";

        /// <summary>
        /// Uchovává příponu HTML dokumentů použitou pro vytváření ze šablon.
        /// </summary>
        public const string DEFAULT_HTML_EXTENSION = "html";

        /// <summary>
        /// Uchovává název složky s výchozími šablonami pro vytváření HTML dokumentů.
        /// </summary>
        private const string DEFAULT_TEMPLATE_FOLDER_NAME = "default-templates";
        
        /// <summary>
        /// Uchovává stručný popis typu publikace pro výpis v uživatelském rozhraní.
        /// </summary>
        public string TypeDescription { get; set; }

        /// <summary>
        /// Uchovává název souboru výchozí šablony pro daný typ publikace.
        /// </summary>
        protected string DefaultTemplateFile { get; set; }

        /// <summary>
        /// Uchovává databázový kontext.
        /// </summary>
        protected DbPublicationEntities Context;

        /// <summary>
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        /// <param name="typeDescription">popis typu publikace</param>
        public APublicationModel(DbPublicationEntities context, string typeDescription)
        {
            Context = context;
            TypeDescription = typeDescription;
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

            if (authors.Count < 1)
            {
                throw new AuthorException("Pro zadanou publikaci nebyl nalezen seznam autorů. Pravděpodobně došlo k poškození dat.");
            }

            StringBuilder sb = new StringBuilder();

            // výpis prvního jména
            Author author = authors.Dequeue();
            sb.Append(author.Surname.ToUpper()).Append(", ").Append(author.Name);

            if (authors.Count < 1)
            {
                return sb.ToString();
            }

            // výpis prostředních jmen
            while (authors.Count > 1)
            {
                author = authors.Dequeue();
                sb.Append(", ").Append(author.Surname.ToUpper()).Append(", ").Append(author.Name);
            }

            // výpis posledního jména
            author = authors.Dequeue();
            sb.Append(" a ").Append(author.Surname.ToUpper()).Append(", ").Append(author.Name);
            
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
            StringBuilder sb = new StringBuilder("\tauthor={");

            // výpis prvního jména
            Author author = authors.Dequeue();
            sb.Append(author.Name).Append(" ").Append(author.Surname);

            // výpis dalších jmen
            while (authors.Count > 0)
            {
                author = authors.Dequeue();
                sb.Append(",").Append(author.Name).Append(" ").Append(author.Surname);
            }
            
            sb.Append("},\n");

            return sb.ToString();
        }

        /// <summary>
        /// Přidá tečku za položku citace, pokud tato položka sama nekončí tečkou.
        /// </summary>
        /// <param name="str">položka citace</param>
        /// <returns>položka citace s přidanou tečkou (pokud je potřebná)</returns>
        protected string AddTrailingDot(string str)
        {
            if (str.EndsWith("."))
            {
                return str;
            }

            return str + ".";
        }

        /// <summary>
        /// Načte šablonu ze souboru na zadané cestě (nebo z výchozího souboru, pokud nebyla zadána)
        /// a vypíše do ní základní bibliografické údaje pro HTML export.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <param name="templatePath">cesta k šabloně</param>
        /// <returns>připravená šablona</returns>
        protected StringTemplate LoadHtmlTemplate(Publication publication, string templatePath)
        {
            string template = null;

            try
            {
                // načtení šablony ze zadaného souboru nebo z výchozí šablony
                string path = string.IsNullOrWhiteSpace(templatePath) ?
                    (DEFAULT_TEMPLATE_FOLDER_NAME + Path.DirectorySeparatorChar + DefaultTemplateFile + "." + DEFAULT_TEMPLATE_EXTENSION) :
                    templatePath;
                template = File.ReadAllText(Path.GetFullPath(path));
            }
            catch (Exception e)
            {
                throw new PublicationException("Chyba při čtení vstupního souboru šablony: " + e.Message);
            }
            
            // vyplnění základních údajů pro všechny typy publikací
            StringTemplate stringTemplate = new StringTemplate(template);
            stringTemplate.SetAttribute("id", publication.Id);
            stringTemplate.SetAttribute("title", publication.Title);
            stringTemplate.SetAttribute("authors", GenerateAuthorCitationString(publication));
            stringTemplate.SetAttribute("year", publication.Year);
            stringTemplate.SetAttribute("type", TypeDescription);
            stringTemplate.SetAttribute("text", publication.Text.Replace("\n", "<br/>"));

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
        /// <param name="authors">seznam autorů</param>
        protected void CreatePublication(Publication publication, List<Author> authors)
        {
            if (publication.Text == null)
            {
                publication.Text = "";
            }
            
            // uložení záznamu publikace
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
        /// Upraví publikaci podle zadaných údajů.
        /// </summary>
        /// <param name="originalPublication">publikace k úpravě</param>
        /// <param name="publication">nové údaje</param>
        /// <param name="authors">nový seznam autorů</param>
        protected void UpdatePublication(Publication originalPublication, Publication publication, List<Author> authors)
        {
            // ponechání původního BibTeX klíče, pokud nový nebyl vyplněn
            if (publication.Entry != null)
            {
                originalPublication.Entry = publication.Entry;
            }

            // ponechání původního názvu publikace, pokud nový nebyl vyplněn
            if (publication.Title != null)
            {
                originalPublication.Title = publication.Title;
            }

            // ponechání původního textu, pokud nový nebyl vyplněn
            if (publication.Text != null)
            {
                originalPublication.Text = publication.Text;
            }

            // ukončení úprav, pokud nebyl vyplněn nový seznam autorů
            if (authors == null || authors.Count == 0)
            {
                return;
            }
            
            // odstranění stávajícího seznamu autorů
            foreach (Author a in originalPublication.Author)
            {
                a.Publication.Remove(originalPublication);
            }

            originalPublication.Author.Clear();

            // vytvoření nového seznamu autorů
            foreach (Author a in authors)
            {
                originalPublication.Author.Add(a);
                a.Publication.Add(originalPublication);
            }
        }

        /// <summary>
        /// Odstraní zadanou publikaci.
        /// </summary>
        /// <param name="originalPublication">publikace k odstranění</param>
        protected void DeletePublication(Publication originalPublication)
        {
            // odstranění publikace u autorů
            foreach (Author author in originalPublication.Author)
            {
                author.Publication.Remove(originalPublication);
            }

            Context.Publication.Remove(originalPublication);
        }
    }
}
