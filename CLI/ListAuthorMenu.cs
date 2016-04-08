using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída představuje menu, které slouží k ručnímu odstraňování již nepoužívaných jmen autorů
    /// automaticky ukládaných při vytváření záznamů o publikacích.
    /// </summary>
    class ListAuthorMenu : AMenu
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;

        /// <summary>
        /// Inicializuje menu správy seznamu uložených autorů.
        /// </summary>
        /// <param name="authorModel">správce autorů</param>
        public ListAuthorMenu(AuthorModel authorModel)
        {
            this.authorModel = authorModel;

            MenuLabel = "Menu správy seznamu autorů";
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odstraní autora se zadaným ID. "
                    + "Odstranit je možné pouze autory, kterým není přiřazena žádná publikace.", UIMethod = RemoveAuthor } },
            });
        }

        /// <summary>
        /// Načte od uživatele ID autora a vyžádá odstranění tohoto autora.
        /// </summary>
        public void RemoveAuthor()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Přijme a vypíše seznam autorů a jejich publikací.
        /// </summary>
        public void GetAuthorList()
        {
            throw new NotImplementedException();
        }
    }
}
