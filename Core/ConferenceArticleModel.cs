using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ConferenceArticleModel
    {
        public ConferenceArticle GetConferenceArticle(Publication publication)
        {
            using (var context = new DbPublicationEntities())
            {
                return context.ConferenceArticle.Find(publication.ConferenceArticle);
            }
        }

        public void AddConferenceArticle(Publication publication, ConferenceArticle conferenceArticle)
        {
            using (var context = new DbPublicationEntities())
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
            using (var context = new DbPublicationEntities())
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
