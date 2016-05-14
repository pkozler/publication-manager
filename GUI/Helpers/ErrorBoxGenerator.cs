using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Třída slouží k vytváření chybových zpráv ze zadaných seznamů
    /// chybových hlášení o zadání neplatných vstupních dat ve formulářích
    /// a k zobrazování těchto zpráv v dialogovém okně.
    /// </summary>
    class ErrorBoxGenerator
    {
        /// <summary>
        /// Vytvoří souhrnnou chybovou zprávu podle zadaných seznamů
        /// nalezených chyb a zobrazí ji v dialogovém okně.
        /// </summary>
        /// <param name="errorHeader">úvod chybové zprávy</param>
        /// <param name="errorLists">seznam chybových hlášení</param>
        public void ShowErrors(string errorHeader, params List<string>[] errorLists)
        {
            StringBuilder sb = new StringBuilder(errorHeader).Append(Environment.NewLine);

            foreach (List<string> errorList in errorLists)
            {
                foreach (string error in errorList)
                {
                    sb.Append(" - ").Append(error).Append(Environment.NewLine);
                }
            }

            MessageBox.Show(sb.ToString(), "Neplatný vstup", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
