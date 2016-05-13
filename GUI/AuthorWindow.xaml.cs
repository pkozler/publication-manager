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
using System.Data.Entity.Validation;

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
        /// Uchovává vybraného autora pro formulář publikace.
        /// </summary>
        public Author Author { get; private set; }

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;
        
        private Publication publication;

        /// <summary>
        /// Provede inicializaci komponent.
        /// </summary>
        public AuthorWindow(AuthorModel authorModel, Publication publication = null)
        {
            this.authorModel = authorModel;
            this.publication = publication;

            InitializeComponent();

            refreshAuthors();
        }
        
        /// <summary>
        /// Provede obsluhu stisku tlačítka pro zavření okna a návrat
        /// na seznam publikací.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void chooseAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (publication == null || authorDataGrid.SelectedItem == null)
            {
                return;
            }

            Author = authorDataGrid.SelectedItem as Author;
            DialogResult = true;
            Close();
        }

        private void deleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (authorDataGrid.SelectedItem == null)
            {
                return;
            }

            Author author = authorDataGrid.SelectedItem as Author;

            if (author.Publication.Count > 0)
            {
                return;
            }

            if (MessageBox.Show("Opravdu chcete odstranit vybraného autora?", "Odstranění autora",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                authorModel.DeleteAuthor(author.Id);
                refreshAuthors();
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při odstraňování záznamu autora z databáze: " + ex.Message);
            }
        }

        private void refreshAuthors()
        {
            List<Author> authorList = authorModel.GetAuthors();

            if (authorList.Count > 0)
            {
                authorListCountLabel.Content = authorList.Count + " celkem";
            }
            else
            {
                authorListCountLabel.Content = "žádní";
            }
            
            authorDataGrid.ItemsSource = null;
            authorDataGrid.ItemsSource = authorList;
        }

        private void refreshPublications(Author publicationAuthor)
        {
            authorPublicationDataGrid.ItemsSource = null;
            authorPublicationDataGrid.ItemsSource = publicationAuthor.Publication;
        }

        private void authorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (authorDataGrid.SelectedItem == null)
            {
                chooseAuthorButton.IsEnabled = false;

                return;
            }

            chooseAuthorButton.IsEnabled = publication != null;

            Author author = authorDataGrid.SelectedItem as Author;
            chooseAuthorButton.IsEnabled = true;

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

            refreshPublications(author);
        }
    }
}
