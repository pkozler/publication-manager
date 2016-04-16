using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída definuje metodu pro řazení publikací.
    /// </summary>
    class PublicationComparer : IComparer<Publication>
    {
        /// <summary>
        /// Vrátí výsledek porovnání publikací podle ID za účelem seřazení.
        /// </summary>
        /// <param name="x">první publikace</param>
        /// <param name="y">druhá publikace</param>
        /// <returns>výsledek porovnání</returns>
        public int Compare(Publication x, Publication y)
        {
            return x.Id - y.Id;
        }
    }
}
