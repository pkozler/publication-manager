﻿using System.Collections.Generic;

namespace CLI
{
    /// <summary>
    /// Třída slouží k uchování informací o podporovaném typu publikace
    /// a jeho sdružení s odpovídající třídou pro zadání příslušných
    /// specifických bibliografických údajů.
    /// </summary>
    public class PublicationType
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
        /// <param name="dialog">objekt pro načítání bibliografických údajů</param>
        public PublicationType(string name, IPublishableDialog dialog)
        {
            Name = name;
            Description = dialog.GetModel().TypeDescription;
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
