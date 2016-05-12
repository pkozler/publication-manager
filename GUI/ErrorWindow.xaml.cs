using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(string errorMessage, params List<string>[] errorLists)
        {
            InitializeComponent();

            printErrors(errorMessage, errorLists);
        }

        private void printErrors(string errorMessage, params List<string>[] errorLists)
        {
            errorLabel.Content = errorMessage;

            StringBuilder sb = new StringBuilder();

            foreach (List<string> errorList in errorLists)
            {
                foreach (string error in errorList)
                {
                    sb.Append(" - ").Append(error).Append("\n");
                }
            }

            errorListTextBlock.Text = sb.ToString();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
