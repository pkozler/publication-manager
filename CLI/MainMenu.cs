using System;
using System.Collections.Generic;
using System.IO;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

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
        /// Seznam typů publikací, který slouží k uchování údajů specifických
        /// pro jednotlivé typy publikací a objektů datové vrstvy pro jejich obsluhu.
        /// </summary>
        private List<PublicationType> publicationTypes;

        /// <summary>
        /// Inicializuje hlavní menu.
        /// </summary>
        /// <param name="publicationTypes">typy publikací</param>
        /// <param name="publicationModel">správce publikací</param>
        /// <param name="authorModel">správce autorů</param>
        /// <param name="attachmentModel">správce příloh</param>
        public MainMenu(List<PublicationType> publicationTypes, PublicationModel publicationModel, AuthorModel authorModel, AttachmentModel attachmentModel)
        {
            this.publicationTypes = publicationTypes;
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;

            MenuLabel = "Hlavní menu";
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.L, new MenuItem() { Name = "List", Description = "Zobrazí menu výpisu publikací se zadanými filtry.", UIMethod = GetPublicationList } },
                { ConsoleKey.R, new MenuItem() { Name = "Read", Description = "Načte detail zadané publikace, kterou lze upravit nebo odstranit.", UIMethod = GetPublicationDetail } },
                { ConsoleKey.C, new MenuItem() { Name = "Create", Description = "Vytvoří novou publikaci.", UIMethod = CreateNewPublication } },
                { ConsoleKey.A, new MenuItem() { Name = "Authors", Description = "Zobrazí menu výpisu uložených autorů.", UIMethod = GetAuthorList } },
                { ConsoleKey.I, new MenuItem() { Name = "Info", Description = "Vypíše stručný popis programu.", UIMethod = PrintInfo } },
            });
            
            MenuItems[ConsoleKey.H].Description = "Ukončí program.";
        }

        /// <summary>
        /// Zobrazí menu seznamu publikací.
        /// </summary>
        public void GetPublicationList()
        {
            ListPublicationMenu menu = new ListPublicationMenu(
                publicationTypes, publicationModel, authorModel);
            menu.Start();
        }

        /// <summary>
        /// Načte od uživatele ID publikace, přijme a vypíše její bibliografické údaje
        /// a zobrazí menu správy publikace.
        /// </summary>
        public void GetPublicationDetail()
        {
            WriteLine("Zadejte ID publikace pro zobrazení bibliografických údajů:");

            int id = ReadValidNumber("Zadejte kladné celé číslo představující ID existující publikace.");
            
            try
            {
                PublicationMainMenu menu = new PublicationMainMenu(
                publicationTypes, publicationModel, authorModel, attachmentModel, id);
                menu.Start();
            }
            catch (PublicationException e)
            {
                WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Načte od uživatele údaje pro záznam nové publikace a zažádá o její vytvoření.
        /// </summary>
        public void CreateNewPublication()
        {
            Publication publication = new Publication();
            WriteLine("Zadejte název BibTeX záznamu publikace:");
            publication.Entry = ReadNonEmptyString("Název nesmí být prázdný.");

            WriteLine("Zadejte křestní jméno nového autora NEBO "
                + "číslo představující ID již použitého autora:");
            List<Author> authors = ReadAuthorList(authorModel,
                "Zadejte křestní jméno dalšího nového autora NEBO "
                + "číslo představující ID již použitého autora NEBO "
                + "potvrďte hotový seznam autorů klávesou ENTER (tj. vložením prázdného řádku):",
                "Zadejte příjmení nového autora:");

            WriteLine("Zadejte skutečný název publikace:");
            publication.Title = ReadNonEmptyString("Název nesmí být prázdný.");
            WriteLine("Zadejte rok vydání publikace:");
            int year = ReadValidNumber("Zadejte celé číslo představující rok vydání");
            publication.Year = year;
            WriteLine("Zadejte cestu k textovému souboru publikace pro import textu "
                + "nebo prázdný řádek pro zadání z konzole: (pouze pro testovací účely)");

            string path = ReadLine().Trim();
            if (string.IsNullOrEmpty(path))
            {
                WriteLine("Vložte text publikace: (\\n pro nový řádek)");
                publication.Text = ReadLine().Replace("\\n", Environment.NewLine); ;
            }
            else
            {
                using (var reader = new StreamReader(path))
                {
                    publication.Text = reader.ReadToEnd();
                }
            }
            
            WritePublicationTypes("Dostupné typy publikací:", publicationTypes);
            int typeNumber = ReadValidNumber("Zadejte číslo označující typ publikace podle výše uvedeného seznamu.");
            publication.Type = publicationTypes[typeNumber].Name;

            // předání načítání údajů dialogu pro zvolený typ publikace
            publicationTypes[typeNumber].Dialog.CreateSpecificBibliography(publication, authors);
            WriteLine("Publikace byla úspěšně vytvořena.");
        }

        /// <summary>
        /// Zobrazí menu seznamu autorů.
        /// </summary>
        public void GetAuthorList()
        {
            ListAuthorMenu menu = new ListAuthorMenu(authorModel);
            menu.Start();
        }

        /// <summary>
        /// Vypíše info o programu.
        /// </summary>
        public void PrintInfo()
        {
            WriteLine("\nO programu:");
            WriteLine("\tAplikace na evidenci vědeckých textů - testovací konzolové rozhraní (Verze 1.0)");
            WriteLine("\tSemestrální práce z předmětu \"Programování v prostředí .NET\" (KIV/NET)");
            WriteLine("\tCopyright (c) Petr Kozler (A13B0359P), 2016\n");
        }
        
        /// <summary>
        /// Po potvrzení ukončí program.
        /// </summary>
        public override void ExitMenu()
        {
            if (ReadYesNoAnswer("Opravdu chcete ukončit program?"))
            {
                IsRunning = false;
            }
        }
    }
}
