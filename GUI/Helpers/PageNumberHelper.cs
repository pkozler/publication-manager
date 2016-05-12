using System.Collections.Generic;
using System.Windows.Controls;
using TAlex.WPF.Controls;

namespace GUI
{
    class PageNumberHelper
    {
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
