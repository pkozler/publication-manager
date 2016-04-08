using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "článek na konferenci".
    /// </summary>
    public class ConferenceArticleModel : APublicationModel
    {
        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        public ConferenceArticle GetPublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = GetPublication(context, id);
                return publication.ConferenceArticle;
            }
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="conferenceArticle">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, ConferenceArticle conferenceArticle)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.ConferenceArticle = conferenceArticle;
                conferenceArticle.Publication = publication;
                CreatePublication(context, publication);
                context.ConferenceArticle.Add(conferenceArticle);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="conferenceArticle">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, ConferenceArticle conferenceArticle)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                UpdatePublication(context, oldPublication, publication);
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
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                ConferenceArticle oldConferenceArticle = oldPublication.ConferenceArticle;
                context.ConferenceArticle.Remove(oldConferenceArticle);
                DeletePublication(context, oldPublication);
                context.SaveChanges();
            }
        }
    }
}
