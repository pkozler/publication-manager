namespace CLI
{
    /// <summary>
    /// Rozhraní představuje obecný dialog konzolového rozhraní pro zadání
    /// údajů specifických pro konkrétní typ publikace.
    /// </summary>
    interface IPublishableDialog
    {
        /// <summary>
        /// Načte od uživatele specifické údaje pro konkrétní typ publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        void SetTypeSpecificBibliography(int? publicationId);
    }
}
