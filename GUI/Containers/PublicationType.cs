using System.Collections.Generic;
using Core;

namespace GUI
{
    public delegate IPublishableForm CreatePublicationUserControl(APublicationModel model);

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

        public readonly CreatePublicationUserControl CreateForm;

        /// <summary>
        /// Vytvoří přepravku pro informace o typu publikace.
        /// </summary>
        /// <param name="dialog">objekt datové vrstvy pro správu bibliografických údajů</param>
        public PublicationType(string name, APublicationModel model, CreatePublicationUserControl createForm)
        {
            Name = name;
            Model = model;
            CreateForm = createForm;
        }
        
        /// <summary>
        /// Vrátí název publikace.
        /// </summary>
        /// <returns>název</returns>
        public override string ToString()
        {
            return Name;
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
