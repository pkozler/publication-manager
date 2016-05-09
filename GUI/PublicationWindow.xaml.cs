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
using Core;
using Microsoft.Win32;
using System.IO;

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
        
        public PublicationWindow(AuthorModel authorModel, AttachmentModel attachmentModel, List<PublicationType> publicationTypes, Publication publication = null)
        {
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;

            if (publication != null)
            {
                this.publication = publication;
                InitializeComponent();
                typeComboBox.ItemsSource = publicationTypes;
                attachmentDataGrid.IsEnabled = false;

                return;
            }

            InitializeComponent();

            attachments = new ObservableCollection<Attachment>(attachmentModel.GetAttachmentsByPublication(publication));
            attachmentDataGrid.ItemsSource = attachments;

            bibtexEntryTextBox.Text = publication.Entry;
            titleTextBox.Text = publication.Title;
            yearTextBox.Text = publication.Year.ToString();

            typeComboBox.ItemsSource = publicationTypes;
            typeComboBox.SelectedValue = PublicationType.GetTypeByName(publicationTypes, publication.Type);
            typeComboBox.IsEnabled = false;

            publicationAuthorListView.ItemsSource = publication.Author;
            contentTextBox.Text = publication.Text;
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
        
        private void saveTextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFile.FileName, contentTextBox.Text);
                    statusLabel.Content = "Text byl uložen do souboru " + saveFile.FileName;
                }
                catch (IOException)
                {
                    MessageBox.Show("Chyba při ukládání textu do výstupního souboru.",
                        "Chyba při zápisu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void loadTextButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    contentTextBox.Text = File.ReadAllText(openFile.FileName);
                    statusLabel.Content = "Text byl načten ze souboru " + openFile.FileName;
                }
                catch (IOException)
                {
                    MessageBox.Show("Chyba při načítání textu ze vstupního souboru.",
                        "Chyba při čtení", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (typeComboBox.SelectedValue != null)
            {
                PublicationType publicationType = typeComboBox.SelectedValue as PublicationType;

                typeSpecificBibliographyGroupBox.Content = publicationType.CreateForm(publicationType.Model) as UserControl;
            }*/
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

        }

        private void insertPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void editPublicationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deletePublicationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openAttachmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeAttachmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void attachmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeAttachmentButton.IsEnabled = attachmentDataGrid.SelectedItem != null;
        }
    }
}
