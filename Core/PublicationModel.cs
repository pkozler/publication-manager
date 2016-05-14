using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Třída představuje správce základních bibliografických údajů společných
    /// pro všechny typy publikací (zahrnují BibTeX klíč, název, rok a typ publikace)
    /// určený především pro výpis seznamu s volitelným filtrováním podle některých údajů.
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
        /// Vybere uložené autory podle ID ze zadané množiny.
        /// </summary>
        /// <param name="authorFilter">filtr autorů (množina ID)</param>
        /// <returns>filtrovaná kolekce autorů</returns>
        private IQueryable<Author> getAuthorsFromIds(HashSet<int> authorFilter)
        {
            return from a in context.Author
                   where authorFilter.Contains(a.Id)
                   select a;
        }
        
        /// <summary>
        /// Provede filtrování seznamu publikací podle zadaných filtrů představovaných
        /// množinami povolených údajů publikací.
        /// </summary>
        /// <param name="authors">povolení autoři (kolekce sestavená podle filtru ID autorů)</param>
        /// <param name="yearFilter">filtr letopočtů</param>
        /// <param name="publicationTypeFilter">filtr typů publikací</param>
        /// <returns></returns>
        private IOrderedQueryable<Publication> getFilteredPublications(
            IQueryable<Author> authors, HashSet<int> yearFilter, HashSet<string> publicationTypeFilter)
        {
            return from p in context.Publication
                   // publikace je začleněna do kolekce, pokud mezi její autory patří alespoň jeden ze zadaných
                   where ((authors.Count() == 0) ? true : authors.Intersect(p.Author).Any())
                   // rok vydání publikace musí patřit mezi zadané
                   && ((yearFilter.Count() == 0) ? true : yearFilter.Contains(p.Year))
                   // typ publikace musí patřit mezi zadané
                   && ((publicationTypeFilter.Count()) == 0 ? true : publicationTypeFilter.Contains(p.Type))
                   orderby p.Id
                   select p;
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
            authorFilter = authorFilter ?? new HashSet<int>();
            yearFilter = yearFilter ?? new HashSet<int>();
            publicationTypeFilter = publicationTypeFilter ?? new HashSet<string>();

            // výběr autorů ze seznamu evidovaných podle ID autorů ze zadané množiny
            var authors = getAuthorsFromIds(authorFilter);
            // výběr publikací s filtrováním podle množin (pokud je některá množina prázdná, příslušná položka se nefiltruje)
            var publications = getFilteredPublications(authors, yearFilter, publicationTypeFilter);
            
            return publications.ToList();
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

        /// <summary>
        /// Načte seznam všech roků vydání použitých v uložených záznamech publikací tak,
        /// aby prvky seznamu byly jedinečné a vzestupně seřazené.
        /// </summary>
        /// <returns>seřazený seznam unikátních čísel představujících roky vydání</returns>
        public List<int> GetYears()
        {
            // výběr sloupce publikací s odstraněním duplicit a seřazením
            return context.Publication.OrderBy(p => p.Year).Select(p => p.Year).Distinct().ToList();
        }
    }
}
