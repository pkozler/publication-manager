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
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Core;
using System.Data.Entity.Validation;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro PublicationWindow.xaml
    /// představující okno, které slouží k zobrazování, zadávání a editaci
    /// podrobných údajů o konkrétní publikaci (včetně specifických informací
    /// pro jednotlivé odlišné typy publikací, dále samotného obsahu publikace
    /// a seznamu příloh), případně k trvalému odstranění publikace z evidence.
    /// </summary>
    public partial class PublicationWindow : Window
    {
        private Publication originalPublication;

        private ObservableCollection<Attachment> attachments;
        
        private AttachmentModel attachmentModel;

        private AuthorModel authorModel;

        private List<PublicationType> publicationTypes;
        
        private IPublishableForm currentBibliographyForm = null;
        
        public PublicationWindow(AuthorModel authorModel, AttachmentModel attachmentModel,
            List<PublicationType> publicationTypes, Publication originalPublication = null)
        {
            InitializeComponent();

            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;
            this.originalPublication = originalPublication;
            typeComboBox.ItemsSource = publicationTypes;

            if (originalPublication == null)
            {
                initializeNewPublicationMode();
            }
            else
            {
                initializeExistingPublicationMode();
            }
        }

        private void initializeNewPublicationMode()
        {
            originalPublication = new Publication();
            typeComboBox.SelectedIndex = 0;
        }

        private void initializeExistingPublicationMode()
        {
            typeComboBox.IsEnabled = false;
            insertPublicationButton.IsEnabled = false;
            editPublicationButton.IsEnabled = true;
            deletePublicationButton.IsEnabled = true;
            attachmentDataGrid.IsEnabled = true;
            addAttachmentButton.IsEnabled = true;

            bibtexEntryTextBox.Text = originalPublication.Entry;
            titleTextBox.Text = originalPublication.Title;
            yearNumericUpDown.Value = originalPublication.Year;
            contentTextBox.Text = originalPublication.Text;

            foreach (Author author in originalPublication.Author)
            {
                publicationAuthorListView.Items.Add(author);
            }

            attachments = new ObservableCollection<Attachment>(
                attachmentModel.GetAttachmentsByPublication(originalPublication));
            attachmentDataGrid.ItemsSource = attachments;

            typeComboBox.SelectedValue = PublicationType.GetTypeByName(publicationTypes, originalPublication.Type);
            setBibliographyForm();
            currentBibliographyForm.ViewPublication(originalPublication);
        }

        private void setBibliographyForm()
        {
            PublicationType publicationType = typeComboBox.SelectedValue as PublicationType;
            
            if (currentBibliographyForm != null)
            {
                typeSpecificBibliographyGrid.Children.Remove(currentBibliographyForm as UserControl);
            }

            currentBibliographyForm = publicationType.CreateForm(publicationType.Model);
            UserControl currentBibliographyUserControl = currentBibliographyForm as UserControl;

            currentBibliographyUserControl.Margin = new Thickness(0, 0, 0, 0);
            Grid.SetRow(currentBibliographyUserControl, 0);
            Grid.SetColumn(currentBibliographyUserControl, 0);
            typeSpecificBibliographyGrid.Children.Add(currentBibliographyUserControl);
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
        
        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typeComboBox.SelectedValue == null)
            {
                return;
            }

            setBibliographyForm();
        }

        private void copyTextButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(contentTextBox.Text);
        }

        private void pasteTextButton_Click(object sender, RoutedEventArgs e)
        {
            contentTextBox.Text = Clipboard.GetText();
        }

        private void newAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorDialogWindow authorDialog = new AuthorDialogWindow();
            authorDialog.ShowDialog();

            if (authorDialog.DialogResult == true)
            {
                publicationAuthorListView.Items.Add(authorDialog.Author);
            }
        }

        private void savedAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorWindow authorDialog = new AuthorWindow(authorModel, originalPublication);
            authorDialog.ShowDialog();

            if (authorDialog.DialogResult == true)
            {
                publicationAuthorListView.Items.Add(authorDialog.Author);
            }
        }

        private void removeAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (publicationAuthorListView.SelectedItem == null)
            {
                return;
            }

            Author author = publicationAuthorListView.SelectedItem as Author;
            publicationAuthorListView.Items.Remove(author);
        }

        private void publicationAuthorListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeAuthorButton.IsEnabled = publicationAuthorListView.SelectedItem != null
                && publicationAuthorListView.Items.Count > 1;
        }

        private List<string> validatePublicationBibliography(out Publication publication)
        {
            List<string> errors = new List<string>();
            publication = new Publication();

            if (string.IsNullOrWhiteSpace(bibtexEntryTextBox.Text))
            {
                errors.Add("BibTeX klíč nesmí být prázdný.");
            }
            else
            {
                publication.Entry = bibtexEntryTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                errors.Add("Název publikace nesmí být prázdný.");
            }
            else
            {
                publication.Title = titleTextBox.Text;
            }

            int year = (int) yearNumericUpDown.Value;
            // omezení hodnot pro případ změny datového typu sloupce pro letopočet v databázi na typ "datum"
            if (year < 0 || year > 9999)
            {
                errors.Add("Rok vydání nesmí být menší než 0 ani větší než 9999.");
            }
            else
            {
                publication.Year = year;
            }

            if (string.IsNullOrWhiteSpace(contentTextBox.Text))
            {
                errors.Add("Obsah publikace nesmí být prázdný.");
            }
            else
            {
                publication.Text = contentTextBox.Text;
            }

            if (typeComboBox.SelectedItem == null)
            {
                errors.Add("Musí být vybrán typ publikace.");
            }
            else
            {
                publication.Type = (typeComboBox.SelectedItem as PublicationType).Name;
            }
            
            return errors;
        }

        private List<string> validateAuthorList(out List<Author> authors)
        {
            List<string> errors = new List<string>();
            authors = new List<Author>();

            if (publicationAuthorListView.Items.Count < 1)
            {
                errors.Add("Musí být uveden alespoň jeden autor publikace.");
            }
            
            foreach (Author author in publicationAuthorListView.Items)
            {
                authors.Add(author);
            }

            return errors;
        }

        private PublicationData getValidPublicationData(string errorMessage)
        {
            Publication publication;
            List<Author> authors;
            ASpecificPublication specificPublication;

            List<string> publicationErrors = validatePublicationBibliography(out publication);
            List<string> authorErrors = validateAuthorList(out authors);
            List<string> specificPublicationErrors = currentBibliographyForm.
                ValidatePublicationTypeSpecificBibliography(publication, authors, out specificPublication);

            if (publicationErrors.Count > 0 || authorErrors.Count > 0 || specificPublicationErrors.Count > 0)
            {
                new ErrorWindow(errorMessage, publicationErrors, authorErrors, specificPublicationErrors).ShowDialog();

                return null;
            }

            return new PublicationData(publication, authors, specificPublication);
        }

        private void insertPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            PublicationData publicationData = getValidPublicationData("Neplatné údaje pro novou publikaci:");

            if (publicationData == null)
            {
                return;
            }

            try
            {
                currentBibliographyForm.InsertPublication(
                    publicationData.Publication, publicationData.Authors, publicationData.SpecificPublication);
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při vkládání záznamu publikace do databáze: " + ex.Message);
            }
            
            Close();
        }

        private void editPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            PublicationData publicationData = getValidPublicationData("Neplatné nové údaje pro publikaci:");

            if (publicationData == null)
            {
                return;
            }
            
            try
            {
                currentBibliographyForm.EditPublication(originalPublication.Id,
                    publicationData.Publication, publicationData.Authors, publicationData.SpecificPublication);
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při editaci záznamu publikace v databázi: " + ex.Message);
            }

            Close();
        }

        private void deletePublicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete odstranit vybranou publikaci?", "Odstranění publikace",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                currentBibliographyForm.DeletePublication(originalPublication.Id);
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při odstraňování záznamu publikace z databáze: " + ex.Message);
            }

            Close();
        }

        private void refreshAttachments()
        {
            List<Attachment> attachmentList = attachmentModel.GetAttachmentsByPublication(originalPublication);
            attachments.Clear();

            foreach (Attachment a in attachmentList)
            {
                attachments.Add(a);
            }
        }

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() != true)
            {
                return;
            }
            
            try
            {
                attachmentModel.AddAttachmentToPublication(originalPublication, openFile.FileName);
                statusLabel.Content = "Soubor připojen." + openFile.FileName;
            }
            catch (IOException)
            {
                MessageBox.Show("Chyba při připojování nového souboru.",
                    "Chyba při čtení", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void copyAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (attachmentDataGrid.SelectedItem == null)
            {
                return;
            }
            
            SaveFileDialog saveFile = new SaveFileDialog();

            if (saveFile.ShowDialog() != true)
            {
                return;
            }

            Attachment attachment = attachmentDataGrid.SelectedItem as Attachment;

            try
            {
                attachmentModel.CopyAttachmentOfPublication(originalPublication, saveFile.FileName, attachment.Id);
                statusLabel.Content = "Soubor zkopírován." + saveFile.FileName;
            }
            catch (IOException)
            {
                MessageBox.Show("Chyba při kopírování připojeného souboru.",
                    "Chyba při čtení", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void removeAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (attachmentDataGrid.SelectedItem == null)
            {
                return;
            }

            Attachment attachment = attachmentDataGrid.SelectedItem as Attachment;

            if (MessageBox.Show("Opravdu chcete odstranit vybranou přílohu?", "Odstranění přílohy",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                attachmentModel.RemoveAttachmentFromPublication(originalPublication, attachment.Id);
                refreshAttachments();
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při odstraňování záznamu autora z databáze: " + ex.Message);
            }
        }

        private void attachmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            copyAttachmentButton.IsEnabled = attachmentDataGrid.SelectedItem != null;
            removeAttachmentButton.IsEnabled = attachmentDataGrid.SelectedItem != null;
        }
    }
}
