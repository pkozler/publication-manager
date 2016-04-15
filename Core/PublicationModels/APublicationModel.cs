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
        protected DbPublicationEntities context = new DbPublicationEntities();

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
        /// Pro zadanou publikaci sestaví HTML dokument
        /// pro umístění publikace na webové stránky,
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <returns>HTML dokument</returns>
        public abstract string ExportPublicationToHtmlDocument(Publication publication, string publicationType, string templatePath);
        
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
        /// Připraví HTML šablonu se základními bibliografickými údaji pro export.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <param name="publicationType">typ publikace</param>
        /// <param name="templatePath">cesta k šabloně</param>
        /// <returns>připravená šablona</returns>
        protected StringTemplate PrepareHtmlTemplate(Publication publication, string publicationType, string template)
        {
            StringTemplate stringTemplate = new StringTemplate(template);
            stringTemplate.SetAttribute("title", publication.Title);
            stringTemplate.SetAttribute("authors", GenerateAuthorCitationString(publication));
            stringTemplate.SetAttribute("year", publication.Year);
            stringTemplate.SetAttribute("type", publicationType);
            stringTemplate.SetAttribute("text", publication.Text);

            return stringTemplate;
        }

        /// <summary>
        /// Načte základní údaje o publikaci se zadaným ID.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace</returns>
        protected Publication GetPublication(int id)
        {
            return context.Publication.Find(id);
        }

        /// <summary>
        /// Vytvoří novou publikaci se zadanými údaji.
        /// </summary>
        /// <param name="publication">údaje publikace</param>
        protected void CreatePublication(Publication publication, List<Author> authors)
        {
            context.Publication.Add(publication);

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
            oldPublication.Entry = publication.Entry;
            oldPublication.Title = publication.Title;
            oldPublication.Year = publication.Year;

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

            context.Publication.Remove(oldPublication);
        }
    }
}
