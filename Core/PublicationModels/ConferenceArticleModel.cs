using System.Collections.Generic;
using System.Text;
using Antlr3.ST;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "článek na konferenci".
    /// </summary>
    public class ConferenceArticleModel : APublicationModel
    {
        /// <summary>
        /// Uchovává název typu pro použití v databázi.
        /// </summary>
        public const string NAME = "ConferenceArticle";

        /// <summary>
        /// Uchovává název souboru výchozí šablony.
        /// </summary>
        private const string TEMPLATE = "conference-article"; 
        
        /// <inheritDoc/>
        public ConferenceArticleModel(DbPublicationEntities context, string typeDescription)
            : base(context, typeDescription)
        {
            DefaultTemplateFile = TEMPLATE;
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="authors">seznam autorů publikace</param>
        /// <param name="conferenceArticle">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, List<Author> authors, ConferenceArticle conferenceArticle)
        {
            if (string.IsNullOrEmpty(conferenceArticle.ISBN) 
                && string.IsNullOrEmpty(conferenceArticle.ISSN))
            {
                throw new PublicationException("Musí být zadán alespoň jeden z následujících údajů: ISBN nebo ISSN");
            }

            if (conferenceArticle.ToPage < conferenceArticle.FromPage)
            {
                throw new PublicationException("Poslední strana citace nesmí být menší než počáteční.");
            }

            // propojení základních a specifických údajů
            publication.ConferenceArticle = conferenceArticle;
            conferenceArticle.Publication = publication;
            CreatePublication(publication, authors);
            Context.ConferenceArticle.Add(conferenceArticle);
            Context.SaveChanges();
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="authors">seznam autorů publikace</param>
        /// <param name="conferenceArticle">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, List<Author> authors, ConferenceArticle conferenceArticle)
        {
            Publication oldPublication = GetPublication(id);
            UpdatePublication(oldPublication, publication, authors);
            ConferenceArticle oldConferenceArticle = oldPublication.ConferenceArticle;

            if (conferenceArticle.Address != null)
            {
                oldConferenceArticle.Address = conferenceArticle.Address;
            }
            
            if (conferenceArticle.BookTitle != null)
            {
                oldConferenceArticle.BookTitle = conferenceArticle.BookTitle;
            }
            
            oldConferenceArticle.FromPage = conferenceArticle.FromPage;

            if (conferenceArticle.ISBN != null)
            {
                oldConferenceArticle.ISBN = conferenceArticle.ISBN;
            }
            
            if (conferenceArticle.ISSN != null)
            {
                oldConferenceArticle.ISSN = conferenceArticle.ISSN;
            }
            
            if (conferenceArticle.Publisher != null)
            {
                oldConferenceArticle.Publisher = conferenceArticle.Publisher;
            }
            
            oldConferenceArticle.ToPage = conferenceArticle.ToPage;
            Context.SaveChanges();
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            Publication oldPublication = GetPublication(id);
            ConferenceArticle oldConferenceArticle = oldPublication.ConferenceArticle;
            Context.ConferenceArticle.Remove(oldConferenceArticle);
            DeletePublication(oldPublication);
            Context.SaveChanges();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationIsoCitation(Publication publication)
        {
            ConferenceArticle conferenceArticle = publication.ConferenceArticle;

            string pages = conferenceArticle.FromPage == conferenceArticle.ToPage ? 
                $"{conferenceArticle.FromPage}" : 
                $"{conferenceArticle.FromPage}-{conferenceArticle.ToPage}";
            string identification = (string.IsNullOrEmpty(conferenceArticle.ISBN) ?
                "" : $"ISBN {conferenceArticle.ISBN}")
                + (string.IsNullOrEmpty(conferenceArticle.ISSN) ?
                "" : $" ISSN {conferenceArticle.ISSN}");

            return new StringBuilder($"{GenerateAuthorCitationString(publication)}. ")
                .Append($"{publication.Title}. ")
                .Append($"In: {conferenceArticle.BookTitle}. ")
                .Append($"{conferenceArticle.Address}: ")
                .Append($"{conferenceArticle.Publisher}, ")
                .Append($"{publication.Year}, ")
                .Append($"s. {pages}. ")
                .Append($"{identification}.").ToString();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationBibtexEntry(Publication publication)
        {
            ConferenceArticle conferenceArticle = publication.ConferenceArticle;

            string pages = conferenceArticle.FromPage == conferenceArticle.ToPage ?
                $"{conferenceArticle.FromPage}" :
                $"{conferenceArticle.FromPage} -- {conferenceArticle.ToPage}";

            return new StringBuilder($"@InProceedings{{{publication.Entry},\n")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"\ttitle={{{publication.Title}}},\n")
                .Append($"\tbooktitle={{{conferenceArticle.BookTitle}}},\n")
                .Append($"\taddress={{{conferenceArticle.Address}}},\n")
                .Append($"\tpublisher={{{conferenceArticle.Publisher}}},\n")
                .Append($"\tyear={{{publication.Year}}},\n")
                .Append($"\tpages={{{pages}}},\n")
                .Append((!string.IsNullOrEmpty(conferenceArticle.ISBN) ? 
                    $"\tisbn={{{conferenceArticle.ISBN}}}\n}}\n" : 
                    $"\tissn={{{conferenceArticle.ISSN}}}\n}}\n")).ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string templatePath, string htmlPath)
        {
            StringTemplate stringTemplate = LoadHtmlTemplate(publication, templatePath);

            ConferenceArticle conferenceArticle = publication.ConferenceArticle;
            stringTemplate.SetAttribute("booktitle", conferenceArticle.BookTitle);
            stringTemplate.SetAttribute("address", conferenceArticle.Address);
            stringTemplate.SetAttribute("publisher", conferenceArticle.Publisher);
            stringTemplate.SetAttribute("pages", conferenceArticle.FromPage == conferenceArticle.ToPage ?
                (conferenceArticle.FromPage + "") :
                (conferenceArticle.FromPage + " - " + conferenceArticle.ToPage));
            stringTemplate.SetAttribute("identification", !string.IsNullOrEmpty(conferenceArticle.ISBN) ?
                ("ISBN " + conferenceArticle.ISBN) : ("ISSN" + conferenceArticle.ISSN));

            return SaveHtmlDocument(stringTemplate, htmlPath);
        }
    }
}
