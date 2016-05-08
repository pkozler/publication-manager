using System.Windows;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro AboutWindow.xaml
    /// představující dialogové okno pro popis a copyright programu.
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Provede inicializaci komponent.
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Provede obsluhu stisku tlačítka pro zavření okna a návrat
        /// na seznam publikací.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
