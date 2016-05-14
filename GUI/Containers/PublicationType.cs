using System.Collections.Generic;
using Core;

namespace GUI
{
    /// <summary>
    /// Delegát představující metodu pro vytváření formuláře v GUI pro zadání
    /// specifických bibliografických údajů, odpovídajícího danému typu publikace.
    /// </summary>
    /// <param name="model">správce údajů publikace daného typu</param>
    /// <returns>instance formuláře pro zadání údajů</returns>
    public delegate IPublicationForm CreatePublicationUserControl(APublicationModel model);

    /// <summary>
    /// Třída slouží k uchování informací o podporovaném typu publikace
    /// a jeho sdružení s odpovídajícím objektem pro správu příslušných
    /// specifických bibliografických údajů.
    /// </summary>
    public class PublicationType
    {
        /// <summary>
        /// Uchovává přesný název typu publikace v databázi. 
        /// </summary>
        public readonly string Name;
        
        /// <summary>
        /// Uchovává objekt datové vrstvy pro správu specifických údajů publikace daného typu.
        /// </summary>
        public readonly APublicationModel Model;

        /// <summary>
        /// Uchovává metodu pro vytvoření odpovídajícího formuláře v GUI pro zadání specifických údajů.
        /// </summary>
        public readonly CreatePublicationUserControl CreateForm;

        /// <summary>
        /// Vytvoří přepravku pro informace o typu publikace.
        /// </summary>
        /// <param name="name">název typu v databázi</param>
        /// <param name="model">objekt pro správu údajů daného typu publikace</param>
        /// <param name="createForm">metoda pro vytvoření formuláře v GUI pro zadání údajů</param>
        public PublicationType(string name, APublicationModel model, CreatePublicationUserControl createForm)
        {
            Name = name;
            Model = model;
            CreateForm = createForm;
        }

        /// <summary>
        /// Vrátí popis publikace pro zobrazení v uživatelském rozhraní.
        /// </summary>
        /// <returns>popis publikace</returns>
        public override string ToString()
        {
            return Model.TypeDescription;
        }
        
        /// <summary>
        /// Nalezne typ publikace se zadaným názvem pro databázi.
        /// </summary>
        /// <param name="publicationTypes">seznam typů</param>
        /// <param name="name">název</param>
        /// <returns>nalezený typ nebo null, pokud typ se zadaným názvem neexistuje</returns>
        public static PublicationType GetTypeByName(List<PublicationType> publicationTypes, string name)
        {
            foreach (PublicationType type in publicationTypes)
            {
                if (name == type.Name)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
