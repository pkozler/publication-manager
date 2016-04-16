using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída definuje metodu pro řazení autorů.
    /// </summary>
    class AuthorComparer : IComparer<Author>
    {
        /// <summary>
        /// Vrátí výsledek porovnání autorů podle ID za účelem seřazení.
        /// </summary>
        /// <param name="x">první autor</param>
        /// <param name="y">druhý autor</param>
        /// <returns>výsledek porovnání</returns>
        public int Compare(Author x, Author y)
        {
            return x.Id - y.Id;
        }
    }
}
