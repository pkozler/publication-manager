using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Třída představuje správce uložených autorů publikací.
    /// </summary>
    public class AuthorModel
    {
        /// <summary>
        /// Vrátí seznam všech uložených autorů.
        /// </summary>
        /// <returns>seznam autorů</returns>
        public List<Author> GetAuthors()
        {
            using (var context = new DbPublicationEntities())
            {
                // výběr všech autorů
                var authors = from a in context.Author
                              orderby a.Name
                              select a;

                return authors.ToList();
            }
        }

        /// <summary>
        /// Vrátí seznam autorů publikace se zadaným ID.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <returns>seznam autorů publikace</returns>
        /*public List<Author> GetAuthorsByPublication(int publicationId)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = context.Publication.Find(publicationId);

                // výběr autorů podle ID publikace
                var authors = from a in context.Author
                                where a.Publication == publication
                                orderby a.Name
                                select a;

                return authors.ToList();             
            }
        }*/

        /// <summary>
        /// Uloží údaje nového autora pro pozdější použití.
        /// </summary>
        /// <param name="author">údaje o autorovi</param>
        public void CreateAuthor(Author author)
        {
            using (var context = new DbPublicationEntities())
            {
                context.Author.Add(author);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Přidá nového autora do seznamu autorů publikace a uloží jeho údaje
        /// pro pozdější použití.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="author">údaje o autorovi</param>
        /*public void CreateAuthorOfPublication(Publication publication, Author author)
        {
            using (var context = new DbPublicationEntities())
            {
                author.Publication.Add(publication);
                publication.Author.Add(author);
                context.Author.Add(author);
                context.SaveChanges();
            }
        }*/

        /// <summary>
        /// Přidá existujícího autora do seznamu autorů publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="author">údaje o autorovi</param>
        /*public void AddAuthorToPublication(Publication publication, int authorId)
        {
            using (var context = new DbPublicationEntities())
            {
                Author author = context.Author.Find(authorId);
                author.Publication.Add(publication);
                publication.Author.Add(author);
                context.SaveChanges();
            }
        }*/

        /// <summary>
        /// Odebere autora ze seznamu autorů publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="authorId">údaje o autorovi</param>
        /*public void RemoveAuthorFromPublication(Publication publication, int authorId)
        {
            using (var context = new DbPublicationEntities())
            {
                Author author = context.Author.Find(authorId);
                publication.Author.Remove(author);
                author.Publication.Remove(publication);
                context.SaveChanges();
            }
        }*/

        /// <summary>
        /// Odstraní uložený záznam o autorovi. Lze použít pouze na autora bez publikací.
        /// </summary>
        /// <param name="authorId"></param>
        public void DeleteAuthor(int authorId)
        {
            using (var context = new DbPublicationEntities())
            {
                Author author = context.Author.Find(authorId);
                context.Author.Remove(author);
                context.SaveChanges();
            }
        }
    }
}
