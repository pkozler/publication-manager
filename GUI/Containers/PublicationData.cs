using System.Collections.Generic;
using Core;

namespace GUI
{
    /// <summary>
    /// Třída představuje přepravku pro dočasné uchování dat potřebných
    /// pro vytvoření nebo úpravu publikace a jejich předávání jako
    /// návratové hodnoty metody pro validaci vstupních údajů z formulářů.
    /// </summary>
    public class PublicationData
    {
        /// <summary>
        /// Uchovává základní údaje publikace.
        /// </summary>
        public readonly Publication Publication;

        /// <summary>
        /// Uchovává seznam autorů publikace.
        /// </summary>
        public readonly List<Author> Authors;

        /// <summary>
        /// Uchovává doplňkové údaje pro daný typ publikace.
        /// </summary>
        public readonly ASpecificPublication SpecificPublication;

        /// <summary>
        /// Vytvoří přepravku k uchování zadaných dat pro publikaci.
        /// </summary>
        /// <param name="publication">základní údaje publikace</param>
        /// <param name="authors">seznam autorů publikace</param>
        /// <param name="specificPublication">doplňkové údaje pro daný typ publikace</param>
        public PublicationData(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            Publication = publication;
            Authors = authors;
            SpecificPublication = specificPublication;
        }
    }
}
