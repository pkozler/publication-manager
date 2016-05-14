using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Core;

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
        /// pro jednotlivé typy publikací, objektů datové vrstvy pro jejich správu
        /// a metod pro vytváření příslušných formulářů GUI pro ovládání těchto objektů.
        /// </summary>
        private List<PublicationType> publicationTypes;

        /// <summary>
        /// Provede inicializaci datových objektů a grafických komponent.
        /// </summary>
        /// <param name="publicationModel">správce publikací</param>
        /// <param name="authorModel">správce autorů</param>
        /// <param name="attachmentModel">správce příloh</param>
        /// <param name="publicationTypes">seznam podporovaných typů publikací</param>
        public MainWindow(PublicationModel publicationModel, AuthorModel authorModel, 
            AttachmentModel attachmentModel, List<PublicationType> publicationTypes)
        {
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;

            InitializeComponent();

            /*
                načtení aktuálního nefiltrovaného seznamu publikací, aktuálního seznamu autorů
                a seznamu podporovaných typů publikací (seznam autorů a seznam typů spolu
                s textovým polem pro zadání roků slouží k nastavení filtrů výpisu publikací)
            */
            refreshPublications();
            refreshAuthors();
            typeFilterListView.ItemsSource = publicationTypes;
        }

        /// <summary>
        /// Aktualizuje seznam publikací podle zadaných filtrů v příslušné komponentě GUI.
        /// </summary>
        /// <param name="authorFilter">filtr autorů</param>
        /// <param name="yearFilter">filtr letopočtů</param>
        /// <param name="publicationTypeFilter">filtr typů</param>
        private void refreshPublications(
            HashSet<int> authorFilter = null, HashSet<int> yearFilter = null, HashSet<string> publicationTypeFilter = null)
        {
            // načtení aktuálního filtrovaného seznamu publikací z datové vrstvy a propojení s komponentou GUI
            publicationDataGrid.ItemsSource = null;
            publicationDataGrid.ItemsSource = publicationModel.
                GetPublications(authorFilter, yearFilter, publicationTypeFilter);
        }

        /// <summary>
        /// Aktualizuje seznam autorů pro filtrování publikací v příslušné komponentě GUI.
        /// </summary>
        private void refreshAuthors()
        {
            // načtení aktuálního seznamu autorů z datové vrstvy a propojení s komponentou GUI
            authorFilterListView.ItemsSource = null;
            authorFilterListView.ItemsSource = authorModel.GetAuthors();
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
        /// Provede obsluhu stisku položky menu pro zobrazení okna pro správu
        /// seznamu uložených autorů.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void manageAuthorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AuthorWindow(authorModel).ShowDialog();

            // obnova seznamu autorů pro filtrování
            refreshAuthors();
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

        /// <summary>
        /// Zobrazí okno pro vytvoření nového záznamu evidované publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void insertPublicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // zobrazení okna detailu publikace v režimu vytvoření nového záznamu
            PublicationWindow publicationWindow = new PublicationWindow(
                authorModel, attachmentModel, publicationTypes);
            publicationWindow.ShowDialog();

            if (publicationWindow.DialogResult != true)
            {
                return;
            }

            // obnova nefiltrovaného seznamu publikací a seznamu autorů pro filtrování
            refreshPublications();
            refreshAuthors();

            if (publicationWindow.PerformedPublicationAction == PublicationAction.Insert)
            {
                statusLabel.Content = "Dokončeno vytvoření nové publikace.";
            }
        }

        /// <summary>
        /// Zobrazí okno pro výpis, úpravu nebo odstranění existujícího záznamu evidované publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void viewPublicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (publicationDataGrid.SelectedItem == null)
            {
                return;
            }

            Publication publication = publicationDataGrid.SelectedItem as Publication;

            // zobrazení okna detailu publikace v režimu změny existujícího záznamu
            PublicationWindow publicationWindow = new PublicationWindow(
                authorModel, attachmentModel, publicationTypes, publication);
            publicationWindow.ShowDialog();

            if (publicationWindow.DialogResult != true)
            {
                return;
            }

            // obnova nefiltrovaného seznamu publikací a seznamu autorů pro filtrování
            refreshPublications();
            refreshAuthors();

            switch (publicationWindow.PerformedPublicationAction)
            {
                case PublicationAction.Update:
                    {
                        statusLabel.Content = $"Dokončena úprava publikace s ID {publication.Id}.";
                        break;
                    }
                case PublicationAction.Delete:
                    {
                        statusLabel.Content = $"Dokončeno odstranění publikace s ID {publication.Id}.";
                        break;
                    }
            }
        }

        /// <summary>
        /// Načte cestu ke zdrojovému souboru šablony prostřednictvím zobrazeného dialogového okna.
        /// </summary>
        /// <returns>cesta k souboru šablony nebo NULL při zrušení okna</returns>
        private string getTemplatePathFromDialog()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Načíst soubor šablony (NEPOVINNÉ)";
            openFile.Filter = string.Format("Soubor šablony ({0})|{0}", "*."
                + APublicationModel.DEFAULT_TEMPLATE_EXTENSION);

            if (openFile.ShowDialog() == true)
            {
                return openFile.FileName;
            }

            return null;
        }

        /// <summary>
        /// Načte cestu k cílovému HTML souboru prostřednictvím zobrazeného dialogového okna.
        /// </summary>
        /// <returns>cesta k HTML souboru nebo NULL při zrušení okna</returns>
        private string getHtmlPathFromDialog()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Uložit do HTML dokumentu";
            saveFile.Filter = string.Format("HTML dokument ({0})|{0}", "*."
                + APublicationModel.DEFAULT_HTML_EXTENSION);

            if (saveFile.ShowDialog() == true)
            {
                return saveFile.FileName;
            }

            return null;
        }

        /// <summary>
        /// Exportuje evidovanou publikaci do souboru ve formátu HTML dokumentu na zadané cestě.
        /// Před samotným exportem zobrazí dialogové okno pro výběr souboru šablony HTML kódu, do kterého
        /// budou vložena data publikace (výběr není povinný, při zrušení okna se zvolí výchozí šablona)
        /// a následně dialogové okno pro zadání cílové cesty pro uložení výsledného HTML dokumentu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void exportPublicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (publicationDataGrid.SelectedItem == null)
            {
                return;
            }

            // načtení cest k šabloně a k dokumentu (cesta k šabloně je nepovinná)
            string templatePath = getTemplatePathFromDialog();
            string htmlPath = getHtmlPathFromDialog();
            
            // zrušení exportu, pokud nebyla určena cesta k výslednému dokumentu
            if (htmlPath == null)
            {
                return;
            }

            Publication publication = publicationDataGrid.SelectedItem as Publication;

            // požadavek datové vrstvě na vytvoření zadaného dokumentu podle zadané šablony
            PublicationType.GetTypeByName(publicationTypes, publication.Type).Model
                .ExportPublicationToHtmlDocument(publication, templatePath, htmlPath);
            statusLabel.Content = $"Dokončen export publikace s ID {publication.Id} do HTML dokumentu.";
        }

        /// <summary>
        /// Zruší výběr ve filtru autorů pro výpis publikací od všech autorů.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void clearAuthorFilterButton_Click(object sender, RoutedEventArgs e)
        {
            authorFilterListView.SelectedItems.Clear();
        }

        /// <summary>
        /// Zruší zadání ve filtru letopočtů pro výpis publikací libovolného roku vydání.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void clearYearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            yearFilterTextbox.Clear();
        }

        /// <summary>
        /// Zruší výběr ve filtru typů pro výpis publikací všech typů.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void clearTypeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            typeFilterListView.SelectedItems.Clear();
        }

        /// <summary>
        /// Obnoví výpis publikací podle zadaných filtrů.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void filterPublicationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HashSet<int> authorFilter = new HashSet<int>();
            HashSet<int> yearFilter = new HashSet<int>();
            HashSet<string> publicationTypeFilter = new HashSet<string>();

            // vytvoření filtru autorů z vybraných položek seznamu
            foreach (Author author in authorFilterListView.SelectedItems)
            {
                authorFilter.Add(author.Id);
            }

            string[] lines = yearFilterTextbox.Text.Split(new string[]
            {
                Environment.NewLine,
            }, StringSplitOptions.RemoveEmptyEntries);

            // vytvoření filtru letopočtů ze zadaných platných řádků v textovém poli
            foreach (string line in lines)
            {
                int year;
                if (int.TryParse(line, out year))
                {
                    yearFilter.Add(year);
                }
            }

            // vytvoření filtru typů z vybraných položek seznamu
            foreach (PublicationType type in typeFilterListView.SelectedItems)
            {
                publicationTypeFilter.Add(type.Name);
            }

            // obnova seznamu publikací podle zadaných filtrů
            refreshPublications(authorFilter, yearFilter, publicationTypeFilter);
            statusLabel.Content = "Obnoven seznam publikací se zadanými filtry.";
        }
        
        /// <summary>
        /// Nastaví zadaný stav aktivace tlačítek pro zobrazení a export publikace.
        /// Tlačítka mají být aktivována právě tehdy, je-li vybrán záznam v seznamu publikací.
        /// </summary>
        /// <param name="enabled">TRUE pro aktivaci tlačítek, jinak FALSE</param>
        private void setButtonsEnabled(bool enabled)
        {
            viewPublicationButton.IsEnabled = enabled;
            viewPublicationMenuItem.IsEnabled = enabled;
            exportPublicationButton.IsEnabled = enabled;
            exportPublicationMenuItem.IsEnabled = enabled;
        }

        /// <summary>
        /// Aktualizuje stav aktivace tlačítek a výpis citace podle normy a odpovídajícího BibTeX záznamu
        /// při změně výběru ve vypsaném seznamu publikací.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void publicationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // vyprázdnění textových polí a deaktivace tlačítek zobrazení a exportu při prázdném výběru
            if (publicationDataGrid.SelectedItem == null)
            {
                publicationCitationTextBox.Text = "";
                publicationBibtexTextBox.Text = "";
                setButtonsEnabled(false);

                return;
            }
            
            Publication publication = publicationDataGrid.SelectedItem as Publication;

            /*
                požadavek datové vrstvě na vygenerování citace a BibTeX záznamu, jejich výpis do textových polí
                a aktivace tlačítek zobrazení a exportu při výběru publikace v seznamu.
            */
            publicationCitationTextBox.Text = PublicationType.GetTypeByName(publicationTypes, publication.Type)
                .Model.GeneratePublicationIsoCitation(publication);
            publicationBibtexTextBox.Text = PublicationType.GetTypeByName(publicationTypes, publication.Type)
                .Model.GeneratePublicationBibtexEntry(publication);
            setButtonsEnabled(true);
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
    }
}
