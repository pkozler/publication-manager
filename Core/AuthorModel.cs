using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Author> GetAuthorsByPublication(int publicationId)
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
        }

        /// <summary>
        /// Přidá nového autora do seznamu autorů publikace a uloží jeho údaje
        /// pro pozdější použití.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="author">údaje o autorovi</param>
        public void CreateAuthorOfPublication(int publicationId, Author author)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Přidá existujícího autora do seznamu autorů publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="author">údaje o autorovi</param>
        public void AddAuthorToPublication(int publicationId, Author author)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Odebere autora ze seznamu autorů publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="authorId">údaje o autorovi</param>
        public void RemoveAuthorFromPublication(int publicationId, int authorId)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Odstraní uložený záznam o autorovi. Lze použít pouze na autora bez publikací.
        /// </summary>
        /// <param name="authorId"></param>
        public void DeleteAuthor(int authorId)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
    }
}
