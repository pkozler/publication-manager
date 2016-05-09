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

        private List<PublicationType> publicationTypes;

        public PublicationWindow(AttachmentModel attachmentModel, List<PublicationType> publicationTypes)
        {
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;
            publication = new Publication();

            InitializeComponent();

            typeComboBox.ItemsSource = publicationTypes;
            attachmentDataGrid.IsEnabled = false;
        }

        public PublicationWindow(AttachmentModel attachmentModel, List<PublicationType> publicationTypes, Publication publication)
        {
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;
            this.publication = publication;

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
            /*if (typeComboBox.SelectedValue != null)
            {
                PublicationType publicationType = typeComboBox.SelectedValue as PublicationType;

                typeSpecificBibliographyGroupBox.Content = publicationType.CreateForm(publicationType.Model) as UserControl;
            }*/
        }

        private void loadTextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void newAuthorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void savedAuthorButton_Click(object sender, RoutedEventArgs e)
        {

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

        }
    }
}
