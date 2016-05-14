using System.Collections.Generic;
using Core;

namespace GUI
{
    /// <summary>
    /// Rozhraní poskytuje metody určené obslužným třídám komponent formulářů
    /// pro zadávání doplňujících bibliografických údajů, které jsou specifické
    /// pro jednotlivé podporované typy publikací. Tyto metody slouží ke komunikaci se základními
    /// částmi GUI bez nutnosti v těchto částech znát jednotlivé konkrétní typy publikací
    /// nebo s nimi manipulovat, což je úlohou příslušných specializovaných formulářů. 
    /// </summary>
    public interface IPublicationForm
    {
        /// <summary>
        /// Provede validaci vstupních dat ve formuláři typově specifických bibliografických údajů,
        /// platné vstupy uloží do přepravky a vytvoří seznam hlášení případných nalezených chyb.
        /// </summary>
        /// <param name="publication">základní údaje publikace</param>
        /// <param name="authors">seznam autorů publikace</param>
        /// <param name="specificPublication">přepravka zadaných specifických údajů konkrétního typu publikace</param>
        /// <returns>seznam případných chybových hlášení</returns>
        List<string> ValidatePublicationTypeSpecificBibliography(
            Publication publication, List<Author> authors, out ASpecificPublication specificPublication);

        /// <summary>
        /// Vypíše bibliografické údaje zadané publikace do formulářů
        /// okna pro zobrazení a editaci publikace.
        /// </summary>
        /// <param name="publication">publikace</param>
        void ViewPublication(Publication publication);

        /// <summary>
        /// Požádá objekt datové vrstvy o vložení nového záznamu publikace
        /// se zadanými bibliografickými údaji.
        /// </summary>
        /// <param name="publication">základní údaje publikace</param>
        /// <param name="authors">seznam autorů publikace</param>
        /// <param name="specificPublication">specifické údaje konkrétního typu publikace</param>
        void InsertPublication(Publication publication, List<Author> authors,
            ASpecificPublication specificPublication);

        /// <summary>
        /// Požádá objekt datové vrstvy o úpravu záznamu existující publikace se zadaným ID
        /// podle zadaných bibliografických údajů.
        /// </summary>
        /// <param name="publicationId">ID existující publikace</param>
        /// <param name="publication">základní údaje publikace</param>
        /// <param name="authors">seznam autorů publikace</param>
        /// <param name="specificPublication">specifické údaje konkrétního typu publikace</param>
        void EditPublication(int publicationId, Publication publication, List<Author> authors,
            ASpecificPublication specificPublication);

        /// <summary>
        /// Požádá objekt datové vrstvy o odstranění záznamu existující publikace se zadaným ID.
        /// </summary>
        /// <param name="publicationId">ID existující publikace</param>
        void DeletePublication(int publicationId);
    }
}
