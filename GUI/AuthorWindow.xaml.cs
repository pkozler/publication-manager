using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Validation;
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
        /// Uchovává vybraného autora pro formulář publikace.
        /// </summary>
        public Author SavedAuthor { get; private set; }

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;

        /// <summary>
        /// Indikuje, zda je zvolena publikace pro změnu seznamu autorů.
        /// </summary>
        private bool isPublicationChosen;

        /// <summary>
        /// Provede inicializaci datových objektů a grafických komponent.
        /// </summary>
        /// <param name="authorModel">správce autorů</param>
        /// <param name="publication">TRUE při zobrazení z okna detailu publikace, jinak FALSE</param>
        public AuthorWindow(AuthorModel authorModel, bool isPublicationChosen = false)
        {
            this.authorModel = authorModel;
            this.isPublicationChosen = isPublicationChosen;

            InitializeComponent();
            // načtení aktuálního seznamu autorů
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

        /// <summary>
        /// Vybere autora pro přidání do seznamu autorů zobrazené publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void chooseAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPublicationChosen || authorDataGrid.SelectedItem == null)
            {
                return;
            }

            SavedAuthor = authorDataGrid.SelectedItem as Author;
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Odstraní záznam o vybraném autorovi ze seznamu použitých autorů.
        /// Je možné použít pouze v případě, že k danému autorovi není
        /// přiřazena žádná publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void deleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (authorDataGrid.SelectedItem == null)
            {
                return;
            }

            Author author = authorDataGrid.SelectedItem as Author;

            // zrušení akce, pokud seznam publikací vybraného autora není prázdný
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
                // požadavek datové vrstvě na odstranění záznamu autora a obnova seznamu v komponentě GUI
                authorModel.DeleteAuthor(author.Id);
                refreshAuthors();
                statusLabel.Content = $"Odstraněn autor s ID {author.Id}.";
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při odstraňování záznamu autora z databáze: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Aktualizuje seznam autorů v příslušné komponentě GUI a informaci
        /// o jejich počtu v jejím popisku.
        /// </summary>
        private void refreshAuthors()
        {
            // načtení aktuálního seznamu z datové vrstvy
            List<Author> authorList = authorModel.GetAuthors();

            if (authorList.Count > 0)
            {
                authorListCountLabel.Content = authorList.Count + " celkem";
            }
            else
            {
                authorListCountLabel.Content = "žádní";
            }
            
            // propojení aktuálního seznamu s komponentou GUI
            authorDataGrid.ItemsSource = null;
            authorDataGrid.ItemsSource = authorList;
        }

        /// <summary>
        /// Aktualizuje seznam publikací vybraného autora v příslušné komponentě GUI a informaci
        /// o jejich počtu v jejím popisku.
        /// </summary>
        /// <param name="publicationAuthor">vybraný autor</param>
        private void refreshPublications(Author publicationAuthor)
        {
            // propojení seznamu aktuálně vybraného autora s komponentou GUI
            authorPublicationDataGrid.ItemsSource = null;
            authorPublicationDataGrid.ItemsSource = publicationAuthor.Publication;
        }

        /// <summary>
        /// Aktualizuje stav komponent GUI při změně výběru autora v seznamu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void authorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // deaktivace tlačítek a vyprázdnění seznamu publikací při prázdném výběru
            if (authorDataGrid.SelectedItem == null)
            {
                chooseAuthorButton.IsEnabled = false;
                deleteAuthorButton.IsEnabled = false;
                authorPublicationDataGrid.ItemsSource = null;
                authorPublicationCountLabel.Content = "žádné";

                return;
            }
            
            Author author = authorDataGrid.SelectedItem as Author;

            // deaktivace tlačítka odstranění autora, pokud nemá prázdný seznam publikací
            if (author.Publication.Count > 0)
            {
                authorPublicationCountLabel.Content = author.Publication.Count + " celkem";
                deleteAuthorButton.IsEnabled = false;
            }
            // aktivace tlačítka odstranění autora, pokud má prázdný seznam publikací
            else
            {
                authorPublicationCountLabel.Content = "žádné";
                deleteAuthorButton.IsEnabled = true;
            }

            /*
                aktivace tlačítka výběru autora, pokud je zvolena publikace k úpravě seznamu autorů
                (okno je otevřeno z obrazovky detailu publikace) a obnova seznamu publikací autora
                (výměna seznamu publikací předchozího vybraného autora za seznam aktuálně zvoleného).
            */
            chooseAuthorButton.IsEnabled = isPublicationChosen;
            refreshPublications(author);
        }
    }
}
