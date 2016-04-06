using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class AuthorManager
    {
        public List<Author> ListAuthors(Publication publication)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                if (publication != null)
                {
                    var authors = from a in context.Author
                                  where a.Publication == publication
                                  orderby a.Name
                                  select a;

                    return authors.ToList();
                }
                else
                {
                    var authors = from a in context.Author
                                  orderby a.Name
                                  select a;

                    return authors.ToList();
                }              
            }
        }

        public void AddAuthor(int publicationId)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }

        public void EditAuthor(int publicationId, int authorId)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }

        public void RemoveAuthor(int publicationId, int authorId)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
    }
}
