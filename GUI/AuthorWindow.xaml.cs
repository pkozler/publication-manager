using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Core;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro AuthorWindow.xaml
    /// představující okno, které slouží k výpisu seznamu uložených
    /// autorů existujících publikací (jsou ukládáni automaticky
    /// při vkládání publikací za účelem poskytnutí možnosti přiřazovat
    /// použité autory dalším publikacím opakovaně) s možností ručního
    /// odstranění autorů, kteří aktuálně nemají přiřazeny žádné publikace
    /// (např. z toho důvodu, že všechny jejich publikace byly odstraněny).
    /// </summary>
    public partial class AuthorWindow : Window
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;

        private ObservableCollection<Author> authors;

        /// <summary>
        /// Provede inicializaci komponent.
        /// </summary>
        public AuthorWindow(AuthorModel authorModel)
        {
            this.authorModel = authorModel;

            InitializeComponent();

            authors = new ObservableCollection<Author>(authorModel.GetAuthors());

            if (authors.Count > 0)
            {
                authorListCountLabel.Content = authors.Count + " celkem";
            }
            else
            {
                authorListCountLabel.Content = "žádní";
            }

            authorDataGrid.ItemsSource = authors;
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

        private void chooseAuthorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void authorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (authorDataGrid.SelectedItem == null)
            {
                return;
            }

            Author author = authorDataGrid.SelectedItem as Author;

            if (author.Publication.Count > 0)
            {
                authorPublicationCountLabel.Content = author.Publication.Count + " celkem";
                deleteAuthorButton.IsEnabled = false;
            }
            else
            {
                authorPublicationCountLabel.Content = "žádné";
                deleteAuthorButton.IsEnabled = true;
            }

            authorPublicationDataGrid.ItemsSource = author.Publication;
        }
    }
}
