using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Core
{
    /// <summary>
    /// Třída představuje správce základních bibliografických údajů společných
    /// pro všechny typy publikací (zahrnují BibTeX klíč, název, rok a typ publikace).
    /// </summary>
    public class PublicationModel
    {
        /// <summary>
        /// Uchovává databázový kontext.
        /// </summary>
        protected DbPublicationEntities context;

        /// <summary>
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        public PublicationModel(DbPublicationEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Vrátí seznam evidovaných publikací filtrovaný podle předaných množin údajů
        /// (tedy vrátí pouze publikace s údaji, které jsou obsaženy v příslušných množinách).
        /// </summary>
        /// <param name="authorFilter">množina požadovaných autorů</param>
        /// <param name="yearFilter">množina požadovaných roků vydání</param>
        /// <param name="publicationTypeFilter">množina požadovaných typů publikací</param>
        /// <returns>filtrovaný seznam publikací</returns>
        public List<Publication> GetPublications(
            HashSet<int> authorFilter, HashSet<int> yearFilter, HashSet<string> publicationTypeFilter)
        {
            // výběr autorů ze seznamu evidovaných podle ID autorů ze zadané množiny
            var authors = from a in context.Author
                            where authorFilter.Contains(a.Id)
                            select a;
            
            // výběr publikací s filtrováním podle množin (pokud je některá množina prázdná, příslušná položka se nefiltruje)
            var publications = from p in context.Publication
                                where ((authors.Count() == 0) ? true : authors.Intersect(p.Author).Any())
                                && ((yearFilter.Count() == 0) ? true : yearFilter.Contains(p.Year))
                                && ((publicationTypeFilter.Count()) == 0 ? true : publicationTypeFilter.Contains(p.Type))
                                orderby p.Title
                                select p;

            List<Publication> publicationList = publications.ToList();
            publicationList.Sort(new PublicationComparer());

            return publicationList;
        }
        
        /// <summary>
        /// Načte základní údaje uložené publikace se zadaným ID.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace podle ID</returns>
        public Publication GetPublicationById(int id)
        {
            Publication publication = context.Publication.Find(id);

            if (publication == null)
            {
                throw new PublicationException(string.Format("Publikace s id {0} neexistuje.", id));
            }

            return publication;
        }
    }
}
