using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace GUI
{
    class PageNumberValidator
    {
        public void ValidatePageNumbers(List<string> errors,
            string fromPageText, string toPageText, out int fromPageResult, out int toPageResult)
        {
            fromPageResult = 0;
            toPageResult = 0;

            int fromPage;
            // test, zda je pole s číslem strany prázdné
            bool isFromPageEmpty = string.IsNullOrWhiteSpace(fromPageText);
            // test, zda pole s číslem strany obsahuje platné číslo
            bool isFromPageValid = int.TryParse(fromPageText, out fromPage) && fromPage > 0;

            // číslo počáteční strany bylo zadáno
            if (!isFromPageEmpty)
            {
                if (!isFromPageValid)
                {
                    errors.Add("Číslo počáteční strany s citovaným textem musí být platné kladné celé číslo.");
                }
                else
                {
                    fromPageResult = fromPage;
                }
            }

            int toPage;
            bool isToPageEmpty = string.IsNullOrWhiteSpace(toPageText);
            bool isToPageValid = int.TryParse(toPageText, out toPage) && toPage > 0;

            // číslo poslední strany bylo zadáno
            if (!isToPageEmpty)
            {
                if (!isToPageValid)
                {
                    errors.Add("Číslo poslední strany s citovaným textem musí být platné kladné celé číslo.");
                }
                else
                {
                    toPageResult = toPage;
                }
            }

            // číslo počáteční strany platné, číslo poslední vynecháno - bude použito stejné číslo jako pro první stranu
            if (isToPageEmpty && isFromPageValid)
            {
                toPageResult = fromPage;
            }
            // číslo poslední strany platné, číslo počáteční vynecháno - bude nastaveno na první stranu
            else if (isFromPageEmpty && isToPageValid)
            {
                fromPageResult = 1;
            }
            else
            {
                errors.Add("Musí být uvedena alespoň jedna ze stran, které ohraničují citovaný text.");
            }
        }

        /// <summary>
        /// Zkontroluje uživatelský vstup na výskyt neplatných znaků v textových polích.
        /// </summary>
        /// <param name="text">zadaný řetězec</param>
        /// <returns>TRUE, pokud je vstup platný, jinak FALSE</returns>
        private bool isValidPage(string text)
        {
            // hledání neplatných znaků
            Regex invalidText = new Regex("[^0-9]+");

            return !invalidText.IsMatch(text);
        }

        /// <summary>
        /// Ošetřuje naplatný vstup do textových polí pro zadání čísla.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        public void HandlePageTextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            // kontrola formátu, pokud byl vložen řetězec
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));

                // zrušení změn, pokud je vložený řetězec neplatný
                if (!isValidPage(text))
                {
                    e.CancelCommand();
                }
            }
            // zrušení změn, pokud nebyl vložen řetězec
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Zabraňuje zadání neplatného čísla uživatelem do příslušného textového pole.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        public void PreviewPageTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !isValidPage(e.Text);
        }
    }
}
