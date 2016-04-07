using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída představuje hlavní menu konzolového rozhraní aplikace.
    /// </summary>
    class MainMenu : AMenu
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
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        private AttachmentModel attachmentModel;

        /// <summary>
        /// Inicializuje hlavní menu.
        /// </summary>
        public MainMenu(PublicationModel publicationModel, AuthorModel authorModel, AttachmentModel attachmentModel) : base()
        {
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;

            MenuLabel = "Hlavní menu";
            MenuItems.Add(ConsoleKey.L, new MenuItem() { Name = "List", Description = "Vypíše seznam publikací se zadanými filtry.", UIMethod = GetPublicationList });
            MenuItems.Add(ConsoleKey.R, new MenuItem() { Name = "Read", Description = "Načte detail zadané publikace, kterou lze upravit nebo odstranit.", UIMethod = GetPublicationDetail });
            MenuItems.Add(ConsoleKey.C, new MenuItem() { Name = "Create", Description = "Vytvoří novou publikaci.", UIMethod = CreateNewPublication });
            MenuItems.Add(ConsoleKey.A, new MenuItem() { Name = "Authors", Description = "Vypíše seznam uložených autorů.", UIMethod = GetAuthorList });
            MenuItems.Add(ConsoleKey.I, new MenuItem() { Name = "Info", Description = "Vypíše stručný popis programu.", UIMethod = PrintInfo });
            MenuItems[ConsoleKey.H].Description = "Ukončí program.";
        }

        /// <summary>
        /// Zobrazí menu seznamu publikací.
        /// </summary>
        public void GetPublicationList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Načte od uživatele ID publikace, přijme a vypíše její bibliografické údaje
        /// a zobrazí menu správy publikace.
        /// </summary>
        public void GetPublicationDetail()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Načte od uživatele údaje pro záznam nové publikace a zažádá o její vytvoření.
        /// </summary>
        public void CreateNewPublication()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zobrazí menu seznamu autorů.
        /// </summary>
        public void GetAuthorList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Vypíše info o programu.
        /// </summary>
        public void PrintInfo()
        {
            Console.WriteLine("\nO programu:");
            Console.WriteLine("\tAplikace na evidenci vědeckých textů - testovací konzolové rozhraní (Verze 1.0)");
            Console.WriteLine("\tSemestrální práce z předmětu \"Programování v prostředí .NET\" (KIV/NET)");
            Console.WriteLine("\tCopyright (c) Petr Kozler (A13B0359P), 2016\n");
        }
        
        /// <summary>
        /// Po potvrzení ukončí program.
        /// </summary>
        public override void ExitMenu()
        {
            do
            {
                Console.WriteLine("\nOpravdu chcete ukončit program? (y/n)");
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    IsRunning = false;
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.N)
                {
                    PrintMenu();
                    break;
                }
            }
            while (true);
        }
    }
}
