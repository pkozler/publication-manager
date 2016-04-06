using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class JournalArticleModel
    {
        public JournalArticle GetJournalArticle(Publication publication)
        {
            using (var context = new DbPublicationEntities())
            {
                return context.JournalArticle.Find(publication.JournalArticle);
            }
        }

        public void AddJournalArticle(Publication publication, JournalArticle journalArticle)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.JournalArticle = journalArticle;
                journalArticle.Publication = publication;
                context.Publication.Add(publication);
                context.JournalArticle.Add(journalArticle);
                context.SaveChanges();
            }
        }

        public void EditJournalArticle(Publication publication, JournalArticle journalArticle)
        {
            using (var context = new DbPublicationEntities())
            {
                context.JournalArticle.Remove(publication.JournalArticle);
                context.Publication.Remove(publication);
                publication.JournalArticle = journalArticle;
                journalArticle.Publication = publication;
                context.Publication.Add(publication);
                context.JournalArticle.Add(journalArticle);
                context.SaveChanges();
            }
        }
    }
}
