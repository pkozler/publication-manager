﻿using System.Collections.Generic;
using Core;

namespace CLI
{
    /// <summary>
    /// Rozhraní představuje obecný dialog konzolového rozhraní pro zadání
    /// údajů specifických pro konkrétní typ publikace.
    /// </summary>
    public interface IPublishableDialog
    {
        /// <summary>
        /// Vrátí objekt datové vrstvy pro správu příslušného typu publikací.
        /// </summary>
        /// <returns>objekt pro správu</returns>
        APublicationModel GetModel();

        /// <summary>
        /// Vypíše uložené specifické údaje aktuální zobrazené publikace konkrétního typu.
        /// </summary>
        /// <param name="publication">aktuální publikace</param>
        void GetSpecificBibliography(Publication publication);

        /// <summary>
        /// Načte od uživatele specifické údaje pro vytvoření konkrétního typu publikace.
        /// </summary>
        /// <param name="publication">základní údaje publikace</param>
        /// <param name="authors">seznam autorů publikace</param>
        void CreateSpecificBibliography(Publication publication, List<Author> authors);

        /// <summary>
        /// Načte od uživatele specifické údaje pro úpravu konkrétního typu publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="publication">nové základní údaje</param>
        /// <param name="authors">nový seznam autorů</param>
        void UpdateSpecificBibliography(int publicationId, Publication publication, List<Author> authors);

        /// <summary>
        /// Spustí generování citace a provede její výpis.
        /// </summary>
        /// <param name="publication">aktuální publikace</param>
        void PrintSpecificIsoCitation(Publication publication);

        /// <summary>
        /// Spustí generování BibTeX záznamu a provede jeho výpis.
        /// </summary>
        /// <param name="publication">aktuální publikace</param>
        void PrintSpecificBibtexEntry(Publication publication);

        /// <summary>
        /// Spustí generování HTML dokumentu a provede jeho výpis.
        /// </summary>
        /// <param name="publication">aktuální publikace</param>
        /// <param name="templatePath">cesta ke vstupnímu souboru se šablonou</param>
        /// <param name="htmlPath">cesta k výstupnímu HTML souboru</param>
        void PrintSpecificHtmlDocument(Publication publication, string templatePath, string htmlPath);

        /// <summary>
        /// Odstraní všechny údaje aktuálně zobrazené publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        void DeleteSpecificBibliography(int publicationId);
    }
}
