using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída představuje menu pro úpravu obsahů jednotlivých publikací.
    /// </summary>
    class PublicationTextMenu : AMenu
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        private PublicationModel publicationModel;
        
        /// <summary>
        /// Číslo, které představuje ID aktuálně spravované publikace.
        /// </summary>
        private int publicationId;

        /// <summary>
        /// Inicializuje menu úpravy obsahu publikace.
        /// </summary>
        /// <param name="publicationModel">správce publikací</param>
        public PublicationTextMenu(PublicationModel publicationModel, int publicationId)
        {
            this.publicationModel = publicationModel;
            this.publicationId = publicationId;

            MenuLabel = "Menu správy obsahu publikace č. " + publicationId;
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.U, new MenuItem() { Name = "Update", Description = "Nahradí stávající obsah publikace novým obsahem. " 
                    + "Nový obsah může být buď importován z textového souboru, nebo zadán přímo z konzole (pouze pro testovací účely).", UIMethod = UpdateText } },
            });
        }

        /// <summary>
        /// Načte od uživatele cestu k textovému souboru s textem publikace
        /// nebo samotný text publikace a požádá o nahrazení textu v záznamu.
        /// </summary>
        public void UpdateText()
        {
            throw new NotImplementedException();
        }
    }
}
