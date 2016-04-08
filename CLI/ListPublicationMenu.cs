using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

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
        public ListPublicationMenu(List<PublicationType> publicationTypes, PublicationModel publicationModel)
        {
            this.publicationTypes = publicationTypes;
            this.publicationModel = publicationModel;

            MenuLabel = "Menu zobrazení seznamu publikací";
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.A, new MenuItem() { Name = "Author", Description = "Přidá filtr pro autora se zadaným ID.", UIMethod = AddAuthorFilter} },
                { ConsoleKey.Y, new MenuItem() { Name = "Year", Description = "Přidá filtr pro zadaný rok vydání.", UIMethod = AddYearFilter} },
                { ConsoleKey.T, new MenuItem() { Name = "Type", Description = "Přidá filtr pro zadaný typ publikace.", UIMethod = AddTypeFilter} },
            });
        }

        /// <summary>
        /// Vypíše seznam uložených autorů s jejich ID, načte od uživatele ID
        /// požadovaného autora a přidá příslušný filtr autora pro výpis seznamu publikací.
        /// </summary>
        public void AddAuthorFilter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Načte od uživatele požadovaný rok vydání publikace a přidá příslušný filtr
        /// letopočtu pro výpis seznamu publikací.
        /// </summary>
        public void AddYearFilter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Vypíše možné typy publikací spolu s číselným označením, načte od uživatele
        /// číselné označení a příslušný filtr typu pro výpis seznamu publikací.
        /// </summary>
        public void AddTypeFilter()
        {
            throw new NotImplementedException();

            Console.WriteLine("Dostupné typy publikací:");
            
            for (int i = 0; i < publicationTypes.Count; i++)
            {
                Console.WriteLine($"\t{i} - {publicationTypes[i].Description}");
            }
        }

        /// <summary>
        /// Vypíše seznam publikací pokud bylo dokončeno přidávání filtrů.
        /// </summary>
        public void PrintFilteredPublicationList()
        {
            throw new NotImplementedException();

            authorFilter.Clear();
            yearFilter.Clear();
            publicationTypeFilter.Clear();
        }

        /// <summary>
        /// Vypíše nápovědu.
        /// </summary>
        public override void PrintMenu()
        {
            base.PrintMenu();
            Console.WriteLine("Pro výpis bez filtrů stiskněte klávesu Enter.");
        }
    }
}
