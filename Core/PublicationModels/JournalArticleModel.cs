using System.Collections.Generic;
using System.Text;
using Antlr3.ST;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "článek v časopise".
    /// </summary>
    public class JournalArticleModel : APublicationModel
    {
        /// <summary>
        /// Uchovává název typu pro použití v databázi.
        /// </summary>
        public const string NAME = "JournalArticle";
        
        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        /*public JournalArticle GetPublication(int id)
        {
            Publication publication = GetPublication(context, id);
            return publication.JournalArticle;
        }*/

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="journalArticle">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, List<Author> authors, JournalArticle journalArticle)
        {
            if (journalArticle.ToPage < journalArticle.FromPage)
            {
                throw new PublicationException("Poslední strana citace nesmí být menší než počáteční.");
            }

            publication.JournalArticle = journalArticle;
            journalArticle.Publication = publication;
            CreatePublication(publication, authors);
            context.JournalArticle.Add(journalArticle);
            context.SaveChanges();
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="journalArticle">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, List<Author> authors, JournalArticle journalArticle)
        {
            Publication oldPublication = GetPublication(id);
            UpdatePublication(oldPublication, publication, authors);
            JournalArticle oldJournalArticle = oldPublication.JournalArticle;
            oldJournalArticle.FromPage = journalArticle.FromPage;
            oldJournalArticle.ISSN = journalArticle.ISSN;
            oldJournalArticle.JournalTitle = journalArticle.JournalTitle;
            oldJournalArticle.Number = journalArticle.Number;
            oldJournalArticle.ToPage = journalArticle.ToPage;
            context.SaveChanges();
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            Publication oldPublication = GetPublication(id);
            JournalArticle oldJournalArticle = oldPublication.JournalArticle;
            context.JournalArticle.Remove(oldJournalArticle);
            DeletePublication(oldPublication);
            context.SaveChanges();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationIsoCitation(Publication publication)
        {
            JournalArticle journalArticle = publication.JournalArticle;

            string pages = journalArticle.FromPage == journalArticle.ToPage ?
                $"{journalArticle.FromPage}" :
                $"{journalArticle.FromPage}-{journalArticle.ToPage}";

            return new StringBuilder(GenerateAuthorCitationString(publication))
                .Append($"{publication.Title}. ")
                .Append($"{journalArticle.JournalTitle}. ")
                .Append($"{publication.Year}, ")
                .Append($"{journalArticle.Number}, ")
                .Append($"{pages}. ")
                .Append($"ISSN {journalArticle.ISSN}.").ToString();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationBibtexEntry(Publication publication)
        {
            JournalArticle journalArticle = publication.JournalArticle;

            string pages = journalArticle.FromPage == journalArticle.ToPage ?
                $"{journalArticle.FromPage}" :
                $"{journalArticle.FromPage} -- {journalArticle.ToPage}";

            return new StringBuilder($"@Article{{{publication.Entry},")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"title={{{publication.Title}}},")
                .Append($"journal={{{journalArticle.JournalTitle}}},")
                .Append($"year={{{publication.Year}}},")
                .Append($"number={{{journalArticle.Number}}},")
                .Append($"pages={{{pages}}},")
                .Append($"issn={{{journalArticle.ISSN}}}}}").ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string publicationType, string template)
        {
            StringTemplate stringTemplate = PrepareHtmlTemplate(publication, publicationType, template);
            JournalArticle journalArticle = publication.JournalArticle;
            stringTemplate.SetAttribute("journaltitle", journalArticle.JournalTitle);
            stringTemplate.SetAttribute("number", journalArticle.Number);
            stringTemplate.SetAttribute("pages", journalArticle.FromPage == journalArticle.ToPage ?
                (journalArticle.FromPage + "") :
                (journalArticle.FromPage + " - " + journalArticle.ToPage));
            stringTemplate.SetAttribute("identification", journalArticle.ISSN);

            return stringTemplate.ToString();
        }
    }
}
