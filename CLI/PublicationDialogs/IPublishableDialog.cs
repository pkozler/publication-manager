namespace CLI
{
    /// <summary>
    /// Rozhraní představuje obecný dialog konzolového rozhraní pro zadání
    /// údajů specifických pro konkrétní typ publikace.
    /// </summary>
    interface IPublishableDialog
    {
        //void CreateSpecificBibliography();

        /// <summary>
        /// Načte od uživatele specifické údaje pro konkrétní typ publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        void UpdateSpecificBibliography(int? publicationId);
    }
}
