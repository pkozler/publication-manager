using System.Collections.Generic;
using System.Text;
using System.IO;
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
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        /*public ConferenceArticle GetPublication(int id)
        {
            Publication publication = GetPublication(context, id);
            return publication.ConferenceArticle;
        }*/

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
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
            context.ConferenceArticle.Add(conferenceArticle);
            context.SaveChanges();
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="conferenceArticle">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, List<Author> authors, ConferenceArticle conferenceArticle)
        {
            Publication oldPublication = GetPublication(id);
            UpdatePublication(oldPublication, publication, authors);
            ConferenceArticle oldConferenceArticle = oldPublication.ConferenceArticle;
            oldConferenceArticle.Address = conferenceArticle.Address;
            oldConferenceArticle.BookTitle = conferenceArticle.BookTitle;
            oldConferenceArticle.FromPage = conferenceArticle.FromPage;
            oldConferenceArticle.ISBN = conferenceArticle.ISBN;
            oldConferenceArticle.ISSN = conferenceArticle.ISSN;
            oldConferenceArticle.Publisher = conferenceArticle.Publisher;
            oldConferenceArticle.ToPage = conferenceArticle.ToPage;
            context.SaveChanges();
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            Publication oldPublication = GetPublication(id);
            ConferenceArticle oldConferenceArticle = oldPublication.ConferenceArticle;
            context.ConferenceArticle.Remove(oldConferenceArticle);
            DeletePublication(oldPublication);
            context.SaveChanges();
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

            return new StringBuilder(GenerateAuthorCitationString(publication))
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

            return new StringBuilder($"@InProceedings{{{publication.Entry},")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"title={{{publication.Title}}},")
                .Append($"booktitle={{{conferenceArticle.BookTitle}}},")
                .Append($"address={{{conferenceArticle.Address}}},")
                .Append($"publisher={{{conferenceArticle.Publisher}}},")
                .Append($"year={{{publication.Year}}},")
                .Append($"pages={{{pages}}},")
                .Append((!string.IsNullOrEmpty(conferenceArticle.ISBN) ? 
                    $"isbn={{{conferenceArticle.ISBN}}}}}" : 
                    $"issn={{{conferenceArticle.ISSN}}}}}")).ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string publicationType, string template)
        {
            StringTemplate stringTemplate = PrepareHtmlTemplate(publication, publicationType, template);
            ConferenceArticle conferenceArticle = publication.ConferenceArticle;
            stringTemplate.SetAttribute("booktitle", conferenceArticle.BookTitle);
            stringTemplate.SetAttribute("address", conferenceArticle.Address);
            stringTemplate.SetAttribute("publisher", conferenceArticle.Publisher);
            stringTemplate.SetAttribute("pages", conferenceArticle.FromPage == conferenceArticle.ToPage ?
                (conferenceArticle.FromPage + "") :
                (conferenceArticle.FromPage + " - " + conferenceArticle.ToPage));
            stringTemplate.SetAttribute("identification", !string.IsNullOrEmpty(conferenceArticle.ISBN) ?
                ("ISBN " + conferenceArticle.ISBN) : ("ISSN" + conferenceArticle.ISSN));
            
            return stringTemplate.ToString();
        }
    }
}
