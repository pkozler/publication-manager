using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ConferenceArticleManager
    {
        public ConferenceArticle GetConferenceArticle(Publication publication)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                return context.ConferenceArticle.Find(publication.ConferenceArticle);
            }
        }

        public void AddConferenceArticle(Publication publication, ConferenceArticle conferenceArticle)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                publication.ConferenceArticle = conferenceArticle;
                conferenceArticle.Publication = publication;
                context.Publication.Add(publication);
                context.ConferenceArticle.Add(conferenceArticle);
                context.SaveChanges();
            }
        }

        public void EditConferenceArticle(Publication publication, ConferenceArticle conferenceArticle)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                context.ConferenceArticle.Remove(publication.ConferenceArticle);
                context.Publication.Remove(publication);
                publication.ConferenceArticle = conferenceArticle;
                conferenceArticle.Publication = publication;
                context.Publication.Add(publication);
                context.ConferenceArticle.Add(conferenceArticle);
                context.SaveChanges();
            }
        }
    }
}
