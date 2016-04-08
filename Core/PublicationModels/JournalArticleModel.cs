using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "článek v časopise".
    /// </summary>
    public class JournalArticleModel : APublicationModel
    {
        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        public JournalArticle GetPublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = GetPublication(context, id);
                return publication.JournalArticle;
            }
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="journalArticle">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, JournalArticle journalArticle)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.JournalArticle = journalArticle;
                journalArticle.Publication = publication;
                CreatePublication(context, publication);
                context.JournalArticle.Add(journalArticle);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="journalArticle">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, JournalArticle journalArticle)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                UpdatePublication(context, oldPublication, publication);
                JournalArticle oldJournalArticle = oldPublication.JournalArticle;
                oldJournalArticle.FromPage = journalArticle.FromPage;
                oldJournalArticle.ISSN = journalArticle.ISSN;
                oldJournalArticle.JournalTitle = journalArticle.JournalTitle;
                oldJournalArticle.Number = journalArticle.Number;
                oldJournalArticle.ToPage = journalArticle.ToPage;
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
                JournalArticle oldJournalArticle = oldPublication.JournalArticle;
                context.JournalArticle.Remove(oldJournalArticle);
                DeletePublication(context, oldPublication);
                context.SaveChanges();
            }
        }
    }
}
