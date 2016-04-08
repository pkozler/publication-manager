namespace CLI
{
    /// <summary>
    /// Třída slouží k uchování informací o podporovaném typu publikace
    /// a jeho sdružení s odpovídající třídou pro zadání příslušných
    /// specifických bibliografických údajů.
    /// </summary>
    class PublicationType
    {
        /// <summary>
        /// Uchovává přesný název typu publikace v databázi. 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Uchovává stručný popis typu publikace pro výpis v uživatelském rozhraní.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Uchovává objekt pro načítání specifických údajů publikace z uživatelského vstupu.
        /// </summary>
        public readonly IPublishableDialog Dialog;

        /// <summary>
        /// Vytvoří přepravku pro informace o typu publikace.
        /// </summary>
        /// <param name="name">název typu v databázi</param>
        /// <param name="description">popis v uživatelském rozhraní</param>
        /// <param name="dialog">objekt pro načítání bibliografických údajů</param>
        public PublicationType(string name, string description, IPublishableDialog dialog)
        {
            Name = name;
            Description = description;
            Dialog = dialog;
        }
        
        /// <summary>
        /// Vrátí název publikace.
        /// </summary>
        /// <returns>název</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
