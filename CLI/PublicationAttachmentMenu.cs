using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída představuje menu pro správu seznamu příloh jednotlivých publikací.
    /// </summary>
    class PublicationAttachmentMenu : AMenu
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        private PublicationModel publicationModel;
        
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        private AttachmentModel attachmentModel;

        /// <summary>
        /// Číslo, které představuje ID aktuálně spravované publikace.
        /// </summary>
        private int publicationId;

        /// <summary>
        /// Inicializuje menu správy seznamu příloh publikace.
        /// </summary>
        public PublicationAttachmentMenu(PublicationModel publicationModel, AttachmentModel attachmentModel, int publicationId) : base()
        {
            this.publicationModel = publicationModel;
            this.attachmentModel = attachmentModel;
            this.publicationId = publicationId;

            MenuLabel = "Menu správy příloh publikace č. " + publicationId;
            MenuItems = new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.C, new MenuItem() { Name = "Create", Description = "Přidá nový soubor se zadanou cestou do seznamu příloh publikace.", UIMethod = CreateAttachment } },
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odebere existující soubor se zadaným ID ze seznamu příloh publikace.", UIMethod = DeleteAttachment } },
            };
        }

        /// <summary>
        /// Načte od uživatele údaje o nové příloze a požádá o její přidání do seznamu.
        /// </summary>
        public void CreateAttachment()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Načte od uživatele ID přílohy a požádá o její odebrání ze seznamu.
        /// </summary>
        public void DeleteAttachment()
        {
            throw new NotImplementedException();
        }
    }
}
