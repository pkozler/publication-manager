using System;
using System.Text;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Třída představuje obecného správce specifických
    /// bibliografických údajů pro konkrétní typ publikace.
    /// </summary>
    public abstract class APublicationModel
    {
        /// <summary>
        /// Z bibliografických údajů uložené publikace se zadaným ID
        /// vygeneruje citaci podle ISO normy.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>citace podle ISO normy</returns>
        public abstract string GeneratePublicationIsoCitation(int id);

        /// <summary>
        /// Z bibliografických údajů uložené publikace se zadaným ID
        /// vygeneruje BibTeX záznam odpovídající citaci podle ISO normy.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>BibTeX záznam</returns>
        public abstract string GeneratePublicationBibtexEntry(int id);

        /// <summary>
        /// Pro uloženou publikaci se zadaným ID sestaví HTML dokument
        /// pro umístění publikace na webové stránky,
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>HTML dokument</returns>
        public abstract string ExportPublicationToHtmlDocument(int id);

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
        /// Načte základní údaje o publikaci se zadaným ID.
        /// </summary>
        /// <param name="context">aktuální kontext</param>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace</returns>
        protected Publication GetPublication(DbPublicationEntities context, int id)
        {
            return context.Publication.Find(id);
        }

        /// <summary>
        /// Vytvoří novou publikaci se zadanými údaji.
        /// </summary>
        /// <param name="context">aktuální kontext</param>
        /// <param name="publication">údaje publikace</param>
        protected void CreatePublication(DbPublicationEntities context, Publication publication, List<Author> authors)
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
        /// <param name="context">aktuální kontext</param>
        /// <param name="oldPublication">existující publikace</param>
        /// <param name="publication">nové údaje</param>
        protected void UpdatePublication(DbPublicationEntities context, Publication oldPublication, Publication publication, List<Author> authors)
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
        /// <param name="context">aktuální kontext</param>
        /// <param name="oldPublication">existující publikace</param>
        protected void DeletePublication(DbPublicationEntities context, Publication oldPublication)
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
