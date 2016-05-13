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
        /// Uchovává databázový kontext.
        /// </summary>
        protected DbPublicationEntities context;

        /// <summary>
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        public AuthorModel(DbPublicationEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Vrátí seznam všech uložených autorů.
        /// </summary>
        /// <returns>seznam autorů</returns>
        public List<Author> GetAuthors()
        {
            // výběr všech autorů
            var authors = from a in context.Author
                            orderby a.Id
                            select a;
            
            return authors.ToList();
        }

        /// <summary>
        /// Vrátí uloženého autora podle ID.
        /// </summary>
        /// <param name="id">ID autora</param>
        /// <returns>autor</returns>
        public Author GetAuthorById(int id)
        {
            Author author = context.Author.Find(id);

            if (author == null)
            {
                throw new AuthorException(string.Format("Autor s id {0} neexistuje.", id));
            }

            return author;
        }
        
        /// <summary>
        /// Uloží údaje nového autora pro pozdější použití.
        /// </summary>
        /// <param name="author">údaje o autorovi</param>
        public void CreateAuthor(Author author)
        {
            context.Author.Add(author);
            context.SaveChanges();
        }

        /// <summary>
        /// Odstraní uložený záznam o autorovi. Lze použít pouze na autora bez publikací.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAuthor(int id)
        {
            Author author = context.Author.Find(id);

            if (author == null)
            {
                throw new AuthorException(string.Format("Autor s id {0} neexistuje.", id));
            }

            // prevence odstranění autora s publikacemi
            if (author.Publication.Count > 0)
            {
                throw new AuthorException(string.Format("Autora s id {0} nelze odstranit, protože nemá prázdný seznam publikací.", id));
            }

            context.Author.Remove(author);
            context.SaveChanges();
        }
    }
}
