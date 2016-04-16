using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída definuje metodu pro řazení příloh.
    /// </summary>
    class AttachmentComparer : IComparer<Attachment>
    {
        /// <summary>
        /// Vrátí výsledek porovnání příloh podle ID za účelem seřazení
        /// (ID příslušné publikace není zahrnuto, předpokládá se pouze
        /// výběr příloh jedné konkrétní publikace).
        /// </summary>
        /// <param name="x">první příloha</param>
        /// <param name="y">druhá příloha</param>
        /// <returns>výsledek porovnání</returns>
        public int Compare(Attachment x, Attachment y)
        {
            return x.Id - y.Id;
        }
    }
}
