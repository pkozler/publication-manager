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
        /// Uchovává název souboru výchozí šablony.
        /// </summary>
        private const string TEMPLATE = "journal-article";

        /// <inheritDoc/>
        public JournalArticleModel(DbPublicationEntities context, string typeDescription)
            : base(context, typeDescription)
        {
            DefaultTemplateFile = TEMPLATE;
        }

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
            Context.JournalArticle.Add(journalArticle);
            Context.SaveChanges();
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

            if (journalArticle.ISSN != null)
            {
                oldJournalArticle.ISSN = journalArticle.ISSN;
            }
            
            if (journalArticle.JournalTitle != null)
            {
                oldJournalArticle.JournalTitle = journalArticle.JournalTitle;
            }
            
            if (journalArticle.Number != null)
            {
                oldJournalArticle.Number = journalArticle.Number;
            }
            
            oldJournalArticle.ToPage = journalArticle.ToPage;
            Context.SaveChanges();
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            Publication oldPublication = GetPublication(id);
            JournalArticle oldJournalArticle = oldPublication.JournalArticle;
            Context.JournalArticle.Remove(oldJournalArticle);
            DeletePublication(oldPublication);
            Context.SaveChanges();
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

            return new StringBuilder($"@Article{{{publication.Entry},\n")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"\ttitle={{{publication.Title}}},\n")
                .Append($"\tjournal={{{journalArticle.JournalTitle}}},\n")
                .Append($"\tyear={{{publication.Year}}},\n")
                .Append($"\tnumber={{{journalArticle.Number}}},\n")
                .Append($"\tpages={{{pages}}},\n")
                .Append($"\tissn={{{journalArticle.ISSN}}}\n}}\n").ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string templatePath, string htmlPath)
        {
            StringTemplate stringTemplate = LoadHtmlTemplate(publication, templatePath);

            JournalArticle journalArticle = publication.JournalArticle;
            stringTemplate.SetAttribute("journaltitle", journalArticle.JournalTitle);
            stringTemplate.SetAttribute("number", journalArticle.Number);
            stringTemplate.SetAttribute("pages", journalArticle.FromPage == journalArticle.ToPage ?
                (journalArticle.FromPage + "") :
                (journalArticle.FromPage + " - " + journalArticle.ToPage));
            stringTemplate.SetAttribute("identification", journalArticle.ISSN);

            return SaveHtmlDocument(stringTemplate, htmlPath);
        }
    }
}
