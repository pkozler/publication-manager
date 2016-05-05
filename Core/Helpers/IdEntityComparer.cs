using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Třída definuje metodu pro řazení entit s ID.
    /// </summary>
    class IdEntityComparer : IComparer<IdEntity>
    {
        /// <summary>
        /// Vrátí výsledek porovnání entity podle ID za účelem seřazení.
        /// </summary>
        /// <param name="x">první entita</param>
        /// <param name="y">druhá entita</param>
        /// <returns>výsledek porovnání</returns>
        public int Compare(IdEntity x, IdEntity y)
        {
            return x.Id - y.Id;
        }
    }
}
