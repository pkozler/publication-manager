using System;
using System.Collections.Generic;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída představuje menu pro volitelné přidání filtrů do výpisu seznamu publikací
    /// (seznam je možné filtrovat pomocí ID autora, roku vydání a typu publikace).
    /// </summary>
    class ListPublicationMenu : AMenu
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
        /// Seznam typů publikací, který slouží k uchování údajů specifických
        /// pro jednotlivé typy publikací a objektů datové vrstvy pro jejich obsluhu.
        /// </summary>
        private List<PublicationType> publicationTypes;

        /// <summary>
        /// Množina ID požadovaných autorů pro filtrování seznamu publikací.
        /// </summary>
        private HashSet<int> authorFilter = new HashSet<int>();

        /// <summary>
        /// Množina požadovaných letopočtů pro filtrování seznamu publikací.
        /// </summary>
        private HashSet<int> yearFilter = new HashSet<int>();

        /// <summary>
        /// Množina požadovaných typů publikace pro filtrování seznamu publikací.
        /// </summary>
        private HashSet<string> publicationTypeFilter = new HashSet<string>();

        /// <summary>
        /// Inicializuje menu zobrazení seznamu publikací.
        /// </summary>
        /// <param name="publicationTypes">typy publikací</param>
        /// <param name="publicationModel">správce publikací</param>
        public ListPublicationMenu(List<PublicationType> publicationTypes, PublicationModel publicationModel, AuthorModel authorModel)
        {
            this.publicationTypes = publicationTypes;
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;

            MenuLabel = "Menu zobrazení seznamu publikací";
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.A, new MenuItem() { Name = "Author", Description = "Přidá filtr pro autora se zadaným ID.", UIMethod = AddAuthorFilter} },
                { ConsoleKey.Y, new MenuItem() { Name = "Year", Description = "Přidá filtr pro zadaný rok vydání.", UIMethod = AddYearFilter} },
                { ConsoleKey.T, new MenuItem() { Name = "Type", Description = "Přidá filtr pro zadaný typ publikace.", UIMethod = AddTypeFilter} },
                { ConsoleKey.L, new MenuItem() { Name = "List", Description = "Vypíše seznam se zadanými filtry a odstraní seznam filtrů.", UIMethod = PrintFilteredPublicationList} },
            });
        }

        /// <summary>
        /// Vypíše seznam uložených autorů s jejich ID, načte od uživatele ID
        /// požadovaného autora a přidá příslušný filtr autora pro výpis seznamu publikací.
        /// </summary>
        public void AddAuthorFilter()
        {
            WriteLine("Seznam autorů:");
            WriteLine("ID\tJméno\tPříjmení");
            List<Author> authors = authorModel.GetAuthors();

            foreach (Author author in authors)
            {
                WriteLine($"{author.Id}\t{author.Name}\t{author.Surname}");
            }

            WriteLine("Zadejte ID autora pro filtrování publikací:");
            int id = ReadValidNumber("Zadejte celé číslo představující ID autora.");
            authorFilter.Add(id);
            WriteLine("Přidán filtr pro autora s ID {0}.", id);
        }

        /// <summary>
        /// Načte od uživatele požadovaný rok vydání publikace a přidá příslušný filtr
        /// letopočtu pro výpis seznamu publikací.
        /// </summary>
        public void AddYearFilter()
        {
            WriteLine("Zadejte rok vydání pro filtrování publikací:");
            int year = ReadValidNumber("Zadejte kladné celé číslo představující letopočet.");
            yearFilter.Add(year);
            WriteLine("Přidán filtr pro rok {0}", year);
        }

        /// <summary>
        /// Vypíše možné typy publikací spolu s číselným označením, načte od uživatele
        /// číselné označení a příslušný filtr typu pro výpis seznamu publikací.
        /// </summary>
        public void AddTypeFilter()
        {
            WritePublicationTypes("Dostupné typy publikací:", publicationTypes);
            int typeNumber = ReadValidNumber("Zadejte číslo označující typ publikace podle výše uvedeného seznamu.");
            publicationTypeFilter.Add(publicationTypes[typeNumber].Name);
            WriteLine("Přidán filtr pro typ publikace \"{0}\"", publicationTypes[typeNumber].Description);
        }

        /// <summary>
        /// Vypíše seznam publikací pokud bylo dokončeno přidávání filtrů.
        /// </summary>
        public void PrintFilteredPublicationList()
        {
            WriteLine("Seznam publikací:");
            WriteLine("ID\tBibTeX klíč\tTyp\tNázev\tRok\tAutoři");
            List<Publication> publications = publicationModel.GetPublications(
                authorFilter, yearFilter, publicationTypeFilter);

            foreach (Publication publication in publications)
            {
                Write($"{publication.Id}\t{publication.Entry}\t{PublicationType.GetTypeByName(publicationTypes, publication.Type).Description}\t{publication.Title}\t{publication.Year}\t");
                WriteAuthors(publication.Author);
            }

            authorFilter.Clear();
            yearFilter.Clear();
            publicationTypeFilter.Clear();
        }
    }
}
