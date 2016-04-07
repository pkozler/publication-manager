using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída představuje menu pro správu seznamu autorů jednotlivých publikací.
    /// </summary>
    class PublicationAuthorMenu : AMenu
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        private PublicationModel publicationModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;
        
        /// <summary>
        /// Číslo, které představuje ID aktuálně spravované publikace.
        /// </summary>
        private int publicationId;

        /// <summary>
        /// Inicializuje menu správy seznamu autorů publikace.
        /// </summary>
        public PublicationAuthorMenu(PublicationModel publicationModel, AuthorModel authorModel, int publicationId) : base()
        {
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;
            this.publicationId = publicationId;

            MenuLabel = "Menu správy autorů publikace č. " + publicationId;
            MenuItems = new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.C, new MenuItem() { Name = "Create", Description = "Přidá stávajícího autora se zadaným ID nebo nového autora " 
                    + "se zadaným jménem do seznamu autorů publikace.", UIMethod = AddAuthor } },
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odebere autora se zadaným ID ze seznamu autorů publikace. " 
                    + "Jméno autora zůstane uloženo pro budoucí použití.", UIMethod = RemoveAuthor } },
            };
        }

        /// <summary>
        /// Načte od uživatele údaje o novém autorovi a požádá o jeho přidání do seznamu.
        /// </summary>
        public void AddAuthor()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Načte od uživatele ID autora a požádá o jeho odebrání ze seznamu.
        /// </summary>
        public void RemoveAuthor()
        {
            throw new NotImplementedException();
        }
    }
}
