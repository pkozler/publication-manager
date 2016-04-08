using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída zobrazuje dialog pro zadání údajů o publikaci typu
    /// "článek na konferenci".
    /// </summary>
    class ConferenceArticleDialog : IPublishableDialog
    {
        /// <summary>
        /// Uchovává objekt datové vrstvy pro práci s příslušným typem publikace.
        /// </summary>
        private ConferenceArticleModel model;

        /// <summary>
        /// Inicializuje objekt pro zobrazení dialogu.
        /// </summary>
        /// <param name="model">odpovídající objekt datové vrstvy</param>
        public ConferenceArticleDialog(ConferenceArticleModel model)
        {
            this.model = model;
        }

        /// <inheritDoc/>
        public void SetTypeSpecificBibliography(int? publicationId)
        {
            throw new NotImplementedException();
        }
    }
}
