using System.Collections.Generic;
using System.Windows.Controls;
using TAlex.WPF.Controls;

namespace GUI
{
    /// <summary>
    /// Třída představuje validátor číselných hodnot vložených do formulářových polích,
    /// která slouží k zadání čísel stran publikace (např. sborník nebo časopis v případě článků),
    /// na kterých se nachází citovaný text.
    /// </summary>
    class PageNumberValidator
    {
        /// <summary>
        /// Provede validaci hodnoty zadané jako číslo počáteční strany s citovaným textem.
        /// V případě zadání platné hodnoty vrátí tuto hodnotu, v případě neplatné hodnotu 0
        /// a do předaného chybového seznamu přidá hlášení o chybě.
        /// </summary>
        /// <param name="errors">seznam chyb</param>
        /// <param name="fromPageNumericUpDown">komponenta pro zadání čísla počáteční strany</param>
        /// <returns>hodnota zadaná v komponentě, pokud je platná, jinak 0</returns>
        public int validateFromPageNumber(List<string> errors, NumericUpDown fromPageNumericUpDown)
        {
            int fromPage = (int)fromPageNumericUpDown.Value;

            if (fromPage < 1)
            {
                errors.Add("Číslo strany musí být kladné.");

                return 0;
            }

            return fromPage;
        }

        /// <summary>
        /// Provede validaci hodnoty zadané jako číslo poslední strany s citovaným textem.
        /// V případě zadání platné hodnoty vrátí tuto hodnotu, v případě neplatné hodnotu 0
        /// a do předaného chybového seznamu přidá hlášení o chybě. V případě, že je komponenta
        /// pro zadání čísla poslední strany deaktivována, vrátí hodnotu rovnou číslu počáteční
        /// strany, což slouží jako indikátor, že je citovaný text jednostránkový.
        /// </summary>
        /// <param name="errors">seznam chyb</param>
        /// <param name="toPageNumericUpDown">komponenta pro zadání čísla poslední strany</param>
        /// <param name="pageSingleRadioButton">tlačítko přepínače sloužící k deaktivaci zadání posledního čísla</param>
        /// <param name="pageRangeRadioButton">tlačítko přepínače sloužící k aktivaci zadání posledního čísla</param>
        /// <param name="fromPage">validátorem ověřené číslo počáteční strany</param>
        /// <returns>hodnota zadaná v komponentě, pokud je hodnota platná a komponenta aktivovaná,
        /// jinak buď 0, nebo hodnota shodná s číslem počáteční strany</returns>
        public int validateToPageNumber(List<string> errors, NumericUpDown toPageNumericUpDown,
            RadioButton pageSingleRadioButton, RadioButton pageRangeRadioButton, int fromPage)
        {
            if (pageSingleRadioButton.IsChecked == true
                && pageRangeRadioButton.IsChecked != true)
            {
                return fromPage;
            }

            int toPage = (int)toPageNumericUpDown.Value;

            if (toPage < fromPage)
            {
                errors.Add("Číslo poslední strany citace nesmí být menší než číslo strany první.");

                return 0;
            }
            
            return toPage;
        }

        /// <summary>
        /// Nastaví hodnoty aktivace komponent GUI pro zadávání čísel stran textu (při zvolení zadání
        /// jednostránkového textu je deaktivována komponenta pro zadání čísla poslední strany).
        /// Slouží k vyčlenění funkcionality společné pro několik formulářů na jediné místo.
        /// </summary>
        /// <param name="pageSingleRadioButton">tlačítko přepínače sloužící k deaktivaci zadání posledního čísla</param>
        /// <param name="pageRangeRadioButton">tlačítko přepínače sloužící k aktivaci zadání posledního čísla</param>
        /// <param name="fromPageNumericUpDown">komponenta pro zadání čísla počáteční strany</param>
        /// <param name="toPageNumericUpDown">komponenta pro zadání čísla poslední strany</param>
        public void SetNumericUpDownControls(
            RadioButton pageSingleRadioButton, RadioButton pageRangeRadioButton, 
            NumericUpDown fromPageNumericUpDown, NumericUpDown toPageNumericUpDown)
        {
            if (pageSingleRadioButton.IsChecked == true)
            {
                fromPageNumericUpDown.IsEnabled = true;
                toPageNumericUpDown.IsEnabled = false;
            }
            else if (pageRangeRadioButton.IsChecked == true)
            {
                fromPageNumericUpDown.IsEnabled = true;
                toPageNumericUpDown.IsEnabled = true;
            }
            else
            {
                fromPageNumericUpDown.IsEnabled = false;
                toPageNumericUpDown.IsEnabled = false;
            }
        }
    }
}
