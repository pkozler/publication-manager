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
        private Publication publication;

        private ObservableCollection<Attachment> attachments;
        
        private AttachmentModel attachmentModel;

        private AuthorModel authorModel;

        private List<PublicationType> publicationTypes;
        
        private IPublishableForm currentBibliographyForm = null;
        
        public PublicationWindow(AuthorModel authorModel, AttachmentModel attachmentModel,
            List<PublicationType> publicationTypes, Publication publication = null)
        {
            InitializeComponent();

            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;
            this.publication = publication;

            if (publication == null)
            {
                typeComboBox.ItemsSource = publicationTypes;
                attachmentDataGrid.IsEnabled = false;

                return;
            }

            insertPublicationButton.IsEnabled = false;
            editPublicationButton.IsEnabled = true;
            deletePublicationButton.IsEnabled = true;
            addAttachmentButton.IsEnabled = true;
            copyAttachmentButton.IsEnabled = true;
            removeAttachmentButton.IsEnabled = true;

            attachments = new ObservableCollection<Attachment>(
                attachmentModel.GetAttachmentsByPublication(publication));
            attachmentDataGrid.ItemsSource = attachments;

            bibtexEntryTextBox.Text = publication.Entry;
            titleTextBox.Text = publication.Title;
            yearTextBox.Text = publication.Year.ToString();

            typeComboBox.ItemsSource = publicationTypes;
            typeComboBox.SelectedValue = PublicationType.GetTypeByName(publicationTypes, publication.Type);
            typeComboBox.IsEnabled = false;

            publicationAuthorListView.ItemsSource = publication.Author;
            contentTextBox.Text = publication.Text;

            setBibliographyForm();
            currentBibliographyForm.ViewPublication(publication);
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
            AuthorWindow authorDialog = new AuthorWindow(authorModel);
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

        private Publication getPublicationBibliography()
        {
            Publication publication = new Publication();

            publication.Entry = bibtexEntryTextBox.Text;
            publication.Title = titleTextBox.Text;
            publication.Year = int.Parse(yearTextBox.Text);
            publication.Type = (typeComboBox.SelectedItem as PublicationType).Name;

            return publication;
        }

        private List<Author> getAuthorList()
        {
            List<Author> authors = new List<Author>();

            foreach (Author author in publicationAuthorListView.Items)
            {
                authors.Add(author);
            }

            return authors;
        }

        private void insertPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            Publication publication = getPublicationBibliography();
            List<Author> authors = getAuthorList();
            currentBibliographyForm.InsertPublication(publication, authors);
            Close();
        }

        private void editPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            Publication publication = getPublicationBibliography();
            List<Author> authors = getAuthorList();
            currentBibliographyForm.EditPublication(this.publication.Id, publication, authors);
            Close();
        }

        private void deletePublicationButton_Click(object sender, RoutedEventArgs e)
        {
            currentBibliographyForm.DeletePublication(publication.Id);
            Close();
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
                attachmentModel.AddAttachmentToPublication(publication, openFile.FileName);
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
                attachmentModel.CopyAttachmentOfPublication(publication, saveFile.FileName, attachment.Id);
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
            attachmentModel.RemoveAttachmentFromPublication(publication, attachment.Id);
        }

        private void attachmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeAttachmentButton.IsEnabled = attachmentDataGrid.SelectedItem != null;
        }
    }
}
