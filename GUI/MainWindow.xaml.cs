using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Core;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro MainWindow.xaml
    /// představující přimární okno aplikace, které slouží k výpisu
    /// seznamu vědeckých publikací podle nastavených filtrů a zobrazení
    /// vygenerovaných citací a odpovídajících BibTeX záznamů u zvolených
    /// publikací a dále umožňuje vkládání nových publikací, správu existujících
    /// a jejich export do dokumentů ve formátu HTML vhodném pro umístění
    /// na webové stránky.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        private PublicationModel publicationModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        private AttachmentModel attachmentModel;

        /// <summary>
        /// Seznam typů publikací, který slouží k uchování údajů specifických
        /// pro jednotlivé typy publikací a objektů datové vrstvy pro jejich obsluhu.
        /// </summary>
        private List<PublicationType> publicationTypes;

        private ObservableCollection<Publication> publications;
        
        /// <summary>
        /// Provede inicializaci datových objektů a grafických komponent.
        /// </summary>
        public MainWindow()
        {
            try
            {
                PublicationModelFactory factory = new PublicationModelFactory();
                publicationModel = factory.PublicationModel;
                authorModel = factory.AuthorModel;
                attachmentModel = factory.AttachmentModel;
                publicationTypes = factory.PublicationTypes;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Chyba při inicializaci");
            }
            
            InitializeComponent();

            publications = new ObservableCollection<Publication>(publicationModel.GetPublications());
            publicationDataGrid.ItemsSource = publications;

            authorFilterListView.ItemsSource = authorModel.GetAuthors();
            typeFilterListView.ItemsSource = publicationTypes;
        }

        /// <summary>
        /// Provede obsluhu stisku položky menu pro ukončení aplikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Zobrazí potvrzovací dialog při pokusu o ukončení aplikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete ukončit aplikaci?", "Ukončení aplikace",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Provede obsluhu stisku položky menu pro zobrazení okna pro správu
        /// seznamu uložených autorů.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manageAuthorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AuthorWindow(authorModel).ShowDialog();
        }

        /// <summary>
        /// Provede obsluhu stisku položky menu pro zobrazení dialogového okna
        /// s popisem programu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void insertPublicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new PublicationWindow(authorModel, attachmentModel, publicationTypes).ShowDialog();
        }

        private void viewPublicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (publicationDataGrid.SelectedItem == null)
            {
                return;
            }

            new PublicationWindow(authorModel, attachmentModel, publicationTypes,
                publicationDataGrid.SelectedItem as Publication).ShowDialog();
        }

        private void exportPublicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (publicationDataGrid.SelectedItem == null)
            {
                return;
            }

            string templatePath = null;
            string htmlPath = null;

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Uložit do HTML";
            saveFile.Filter = string.Format("HTML dokument ({0})|{0}", "*.html");

            if (saveFile.ShowDialog() == true)
            {
                htmlPath = saveFile.FileName;
            }
            else
            {
                return;
            }

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Načíst soubor šablony";
            openFile.Filter = string.Format("Soubor šablony ({0})|{0}", "*.st");

            if (openFile.ShowDialog() == true)
            {
                templatePath = openFile.FileName;
            }

            Publication publication = publicationDataGrid.SelectedItem as Publication;
            PublicationType.GetTypeByName(publicationTypes, publication.Type).Model
                .ExportPublicationToHtmlDocument(publication, templatePath, htmlPath);
        }

        private void clearAuthorFilterButton_Click(object sender, RoutedEventArgs e)
        {
            authorFilterListView.SelectedItems.Clear();
        }

        private void clearYearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            yearFilterTextbox.Clear();
        }

        private void clearTypeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            typeFilterListView.SelectedItems.Clear();
        }

        private void filterPublicationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HashSet<int> authorFilter = new HashSet<int>();
            HashSet<int> yearFilter = new HashSet<int>();
            HashSet<string> publicationTypeFilter = new HashSet<string>();

            foreach (Author author in authorFilterListView.SelectedItems)
            {
                authorFilter.Add(author.Id);
            }

            string[] lines = yearFilterTextbox.Text.Split('\n');

            foreach (string line in lines)
            {
                int year;

                if (!int.TryParse(line, out year))
                {
                    continue;
                }

                yearFilter.Add(year);
            }

            foreach (PublicationType type in typeFilterListView.SelectedItems)
            {
                publicationTypeFilter.Add(type.Name);
            }

            List<Publication> publicationList = publicationModel.GetPublications(
                authorFilter, yearFilter, publicationTypeFilter);
            publications.Clear();

            foreach (Publication publication in publicationList)
            {
                publications.Add(publication);
            }
        }
        
        private void setButtonsEnabled(bool enabled)
        {
            viewPublicationButton.IsEnabled = enabled;
            viewPublicationMenuItem.IsEnabled = enabled;
            exportPublicationButton.IsEnabled = enabled;
            exportPublicationMenuItem.IsEnabled = enabled;
        }

        private void publicationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (publicationDataGrid.SelectedItem == null)
            {
                setButtonsEnabled(false);
                return;
            }

            setButtonsEnabled(true);
            Publication publication = publicationDataGrid.SelectedItem as Publication;
            publicationCitationTextBox.Text = PublicationType.GetTypeByName(publicationTypes, publication.Type)
                .Model.GeneratePublicationIsoCitation(publication);
            publicationBibtexTextBox.Text = PublicationType.GetTypeByName(publicationTypes, publication.Type)
                .Model.GeneratePublicationBibtexEntry(publication);
        }
    }
}
