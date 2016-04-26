using System;
using System.Collections.Generic;
using System.Linq;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

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

            GetAuthorList();
        }

        /// <summary>
        /// Načte od uživatele ID autora a vyžádá odstranění tohoto autora.
        /// </summary>
        public void RemoveAuthor()
        {
            WriteLine("Zadejte ID autora k odstranění:");
            int id = ReadValidNumber("Zadejte kladné celé číslo představující ID existujícího autora bez publikací.");

            if (ReadYesNoAnswer("Opravdu chcete odstranit autora?"))
            {
                authorModel.DeleteAuthor(id);
            }

            GetAuthorList();
        }

        /// <summary>
        /// Přijme a vypíše seznam autorů a jejich publikací.
        /// </summary>
        public void GetAuthorList()
        {
            WriteLine("Seznam autorů:");
            WriteLine("ID\tJméno\tPříjmení");
            List<Author> authors = authorModel.GetAuthors();
            
            foreach (Author author in authors)
            {
                WriteLine($"{author.Id}\t{author.Name}\t{author.Surname}");
                Write("Seznam publikací: ");

                Publication[] publications = author.Publication.ToArray();

                if (publications.Length > 0)
                {
                    Write($"{publications[0].Id} ({publications[0].Title})");
                }
                
                for (int i = 1; i < publications.Length; i++)
                {
                    Write($", {publications[i].Id} ({publications[i].Title})");
                }
                
                WriteLine();
            }
        }
    }
}
