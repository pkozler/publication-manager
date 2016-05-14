using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Data.Entity.Validation;
using Microsoft.Win32;
using Core;

namespace GUI
{
    /// <summary>
    /// Výčtový typ, který představuje akci, která byla provedena se záznamem
    /// zobrazené publikace před zavřením okna detailu publikace.
    /// </summary>
    public enum PublicationAction
    {
        Insert, // vložení záznamu
        Update, // úprava záznamu
        Delete, // odstranění záznamu
    }

    /// <summary>
    /// Tato třída obsahuje interakční logiku pro PublicationWindow.xaml
    /// představující okno, které slouží k prohlížení, vytváření a editaci
    /// detailních informací o konkrétní publikaci (včetně specifických údajů
    /// pro jednotlivé odlišné typy publikací, dále samotného obsahu publikace
    /// a jejího seznamu příloh), případně k trvalému odstranění publikace z evidence.
    /// </summary>
    public partial class PublicationWindow : Window
    {
        /// <summary>
        /// Uchovává požadavek odeslaný datové vrstvě před zavřením okna.
        /// </summary>
        public PublicationAction PerformedPublicationAction { get; private set; }

        /// <summary>
        /// Uchovává záznam aktuálně zobrazené publikace nebo údaje pro vytvoření nového.
        /// </summary>
        private Publication originalPublication;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        private AttachmentModel attachmentModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;

        /// <summary>
        /// Seznam typů publikací, který slouží k uchování údajů specifických
        /// pro jednotlivé typy publikací, objektů datové vrstvy pro jejich správu
        /// a metod pro vytváření příslušných formulářů GUI pro ovládání těchto objektů.
        /// </summary>
        private List<PublicationType> publicationTypes;
        
        /// <summary>
        /// Komponenta formuláře zobrazená podle aktuálně zvoleného prvku z nabídky
        /// podporovaných typů publikací, která slouží pro zadávání typově specifických údajů.
        /// </summary>
        private IPublicationForm currentBibliographyForm = null;

        /// <summary>
        /// Uchovává instanci generátoru chybových zpráv vytvořených při procesu validace
        /// bibliografických údajů zadaných ve formulářích.
        /// </summary>
        private ErrorBoxGenerator errorBoxGenerator = new ErrorBoxGenerator();

        /// <summary>
        /// Provede inicializaci datových objektů a grafických komponent.
        /// </summary>
        /// <param name="publicationTypes">správce publikací</param>
        /// <param name="authorModel">správce autorů</param>
        /// <param name="attachmentModel">správce příloh</param>
        /// <param name="originalPublication">zobrazená existující publikace nebo NULL při evidenci nové</param>
        public PublicationWindow(AuthorModel authorModel, AttachmentModel attachmentModel,
            List<PublicationType> publicationTypes, Publication originalPublication = null)
        {
            // inicializace společná pro oba režimy okna
            InitializeComponent();
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationTypes = publicationTypes;
            this.originalPublication = originalPublication;

            // načtení seznamu podporovaných typů publikací
            typeComboBox.ItemsSource = publicationTypes;

            // nastavení režimu vytvoření při nezadání existující publikace
            if (originalPublication == null)
            {
                typeComboBox.SelectedIndex = 0;
            }
            // nastavení režimu zobrazení/změny při zadání existující publikace
            else
            {
                initializeExistingPublicationMode();
            }
        }
        
        /// <summary>
        /// Inicializuje komponenty okna v režimu úpravy existujícího záznamu publikace.
        /// </summary>
        private void initializeExistingPublicationMode()
        {
            // aktivace tlačítek pro změnu záznamu publikace
            typeComboBox.IsEnabled = false;
            insertPublicationButton.IsEnabled = false;
            editPublicationButton.IsEnabled = true;
            deletePublicationButton.IsEnabled = true;
            attachmentDataGrid.IsEnabled = true;
            addAttachmentButton.IsEnabled = true;

            // výpis uložených bibliografických informací
            bibtexEntryTextBox.Text = originalPublication.Entry;
            titleTextBox.Text = originalPublication.Title;
            yearNumericUpDown.Value = originalPublication.Year;
            contentTextBox.Text = originalPublication.Text;

            // výpis uloženého seznamu autorů
            foreach (Author author in originalPublication.Author)
            {
                publicationAuthorListView.Items.Add(author);
            }

            // načtení a výpis uloženého seznamu příloh
            refreshAttachments();

            // nastavení typu publikace v nabídce, zobrazení odpovídajícího formuláře a výpis uložených informací
            typeComboBox.SelectedValue = PublicationType.GetTypeByName(publicationTypes, originalPublication.Type);
            setBibliographyForm();
            currentBibliographyForm.ViewPublication(originalPublication);
        }

        /// <summary>
        /// Zobrazí formulář pro zadávání typově specifických údajů podle aktuálně zvoleného
        /// prvku z nabídky podporovaných typů publikací.
        /// </summary>
        private void setBibliographyForm()
        {
            PublicationType publicationType = typeComboBox.SelectedValue as PublicationType;
            
            // odstranění předchozího zobrazeného formuláře z okna
            if (currentBibliographyForm != null)
            {
                typeSpecificBibliographyGrid.Children.Remove(currentBibliographyForm as UserControl);
            }

            // vytvoření nového formuláře podle zvoleného typu
            currentBibliographyForm = publicationType.CreateForm(publicationType.Model);

            UserControl currentBibliographyUserControl = currentBibliographyForm as UserControl;
            
            // zobrazení formuláře v okně
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
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Provede přepnutí formuláře při změně výběru prvku z nabídky podporovaných typů publikací.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typeComboBox.SelectedValue == null)
            {
                return;
            }

            setBibliographyForm();
        }

        /// <summary>
        /// Zkopíruje do schránky obsah zobrazené publikace z příslušného textového pole.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void copyTextButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(contentTextBox.Text);
            statusLabel.Content = "Text publikace byl zkopírován do schránky.";
        }

        /// <summary>
        /// Vloží ze schránky obsah zobrazené publikace do příslušného textového pole.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void pasteTextButton_Click(object sender, RoutedEventArgs e)
        {
            contentTextBox.Text = Clipboard.GetText();
            statusLabel.Content = "Text publikace byl vložen ze schránky.";
        }

        /// <summary>
        /// Zobrazí dialogové okno pro zadání osobních údajů (jména a příjmení)
        /// nového (zatím nepoužitého) autora a přidá tohoto autora do seznamu
        /// autorů zobrazené publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void newAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazení okna
            AuthorDialogWindow authorDialog = new AuthorDialogWindow();
            authorDialog.ShowDialog();

            if (authorDialog.DialogResult != true)
            {
                return;
            }

            // přidání nového autora do seznamu
            publicationAuthorListView.Items.Add(authorDialog.NewAuthor);
            statusLabel.Content = "Přidána nová osoba do seznamu autorů publikace.";
        }

        /// <summary>
        /// Zobrazí dialogové okno pro výběr uloženého (již použitého) autora
        /// a přidá tohoto autora do seznamu autorů zobrazené publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void savedAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazení okna
            AuthorWindow authorDialog = new AuthorWindow(authorModel, true);
            authorDialog.ShowDialog();

            if (authorDialog.DialogResult != true
                || publicationAuthorListView.Items.Contains(authorDialog.SavedAuthor))
            {
                return;
            }
            
            // přidání existujícího autora do seznamu
            publicationAuthorListView.Items.Add(authorDialog.SavedAuthor);
            statusLabel.Content = "Přidána uložená osoba do seznamu autorů publikace.";
        }

        /// <summary>
        /// Odebere vybraného autora ze seznamu autorů zobrazené publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void removeAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            // zrušení akce, pokud autor nebyl vybrán
            if (publicationAuthorListView.SelectedItem == null)
            {
                return;
            }
            
            Author author = publicationAuthorListView.SelectedItem as Author;

            // odebrání autora ze seznamu
            publicationAuthorListView.Items.Remove(author);
            statusLabel.Content = "Odstraněna osoba ze seznamu autorů publikace.";
        }

        /// <summary>
        /// Je-li vybrán autor, aktivuje tlačítko pro jeho odebrání ze seznamu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void publicationAuthorListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeAuthorButton.IsEnabled = publicationAuthorListView.SelectedItem != null;
        }

        /// <summary>
        /// Provede validaci vstupních dat ve formuláři základních bibliografických údajů,
        /// platné vstupy uloží do přepravky a vytvoří seznam hlášení případných nalezených chyb.
        /// </summary>
        /// <param name="publication">přepravka zadaných základních údajů publikace</param>
        /// <returns>seznam nalezených chyb</returns>
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

        /// <summary>
        /// Provede validaci vstupních dat v seznamu autorů, platné vstupy uloží do seznamu
        /// a vytvoří seznam hlášení případných nalezených chyb.
        /// </summary>
        /// <param name="authors">seznam pro uložení zadaných autorů</param>
        /// <returns>seznam nalezených chyb</returns>
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

        /// <summary>
        /// Spustí kompletní validaci zadaných vstupních dat pro vytvoření nebo úpravu záznamu
        /// evidované publikace a vrátí buď přepravku uchovávající zadaná data, jsou-li všechny
        /// vstupy platné, nebo hodnotu NULL v případě, že byly při validaci nalezeny chyby.
        /// Při nalezení neplatných vstupů předtím také zobrazí dialogové okno s výpisem hlášení
        /// všech nalezených chyb.
        /// </summary>
        /// <param name="errorHeader">úvod chybové zprávy</param>
        /// <returns>přepravka zadaných vstupních dat publikace</returns>
        private PublicationData getValidPublicationData(string errorHeader)
        {
            Publication publication;
            List<Author> authors;
            ASpecificPublication specificPublication;

            // validace s uložením platných vstupů do přepravek a chybových hlášení do seznamů
            List<string> publicationErrors = validatePublicationBibliography(out publication);
            List<string> authorErrors = validateAuthorList(out authors);
            List<string> specificPublicationErrors = currentBibliographyForm.
                ValidatePublicationTypeSpecificBibliography(publication, authors, out specificPublication);

            // zobrazení chybové zprávy při nalezení neplatných vstupů
            if (publicationErrors.Count > 0 || authorErrors.Count > 0 || specificPublicationErrors.Count > 0)
            {
                errorBoxGenerator.ShowErrors(errorHeader, publicationErrors, authorErrors, specificPublicationErrors);

                return null;
            }
            
            // vytvoření přepravky pro obalení zadaných platných dat
            return new PublicationData(publication, authors, specificPublication);
        }

        /// <summary>
        /// Požádá datovou vrstvu o vložení nového záznamu publikace se zadanými vstupními daty.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void insertPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            // validace zadaných dat
            PublicationData publicationData = getValidPublicationData("Byly zadány neplatné údaje pro novou publikaci:");

            // zrušení akce v případě neplatných dat
            if (publicationData == null)
            {
                return;
            }

            try
            {
                // požadavek obsluhy příslušného formuláře pro zvolený typ publikace na vložení nového záznamu
                currentBibliographyForm.InsertPublication(
                    publicationData.Publication, publicationData.Authors, publicationData.SpecificPublication);
                PerformedPublicationAction = PublicationAction.Insert;
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při vkládání záznamu publikace do databáze: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Požádá datovou vrstvu o úpravu existujícího záznamu zobrazené publikace podle zadaných vstupních dat.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void editPublicationButton_Click(object sender, RoutedEventArgs e)
        {
            // validace zadaných dat
            PublicationData publicationData = getValidPublicationData("Byly zadány neplatné nové údaje pro publikaci:");

            // zrušení akce v případě neplatných dat
            if (publicationData == null)
            {
                return;
            }
            
            try
            {
                // požadavek obsluhy příslušného formuláře pro daný typ publikace na úpravu existujícího záznamu
                currentBibliographyForm.EditPublication(originalPublication.Id,
                    publicationData.Publication, publicationData.Authors, publicationData.SpecificPublication);
                PerformedPublicationAction = PublicationAction.Update;
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při editaci záznamu publikace v databázi: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Požádá datovou vrstvu o odstranění existujícího záznamu zobrazené publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void deletePublicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete odstranit vybranou publikaci?", "Odstranění publikace",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                // požadavek obsluhy příslušného formuláře pro daný typ publikace na odstranění existujícího záznamu
                currentBibliographyForm.DeletePublication(originalPublication.Id);
                PerformedPublicationAction = PublicationAction.Delete;
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při odstraňování záznamu publikace z databáze: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Aktualizuje seznam příloh aktuálně zobrazené publikace v příslušné komponentě GUI.
        /// </summary>
        private void refreshAttachments()
        {
            // načtení aktuálního seznamu příloh publikace z datové vrstvy a propojení s komponentou GUI
            attachmentDataGrid.ItemsSource = null;
            attachmentDataGrid.ItemsSource = attachmentModel.GetAttachmentsByPublication(originalPublication);
        }

        /// <summary>
        /// Uloží kopii souboru na zadané cestě do datového adresáře aplikace
        /// a následně ji přidá do seznamu příloh zobrazené publikace.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            // zrušení akce, pokud není zvolena zdrojová cesta
            if (openFile.ShowDialog() != true)
            {
                return;
            }

            try
            {
                // přidání přílohy a obnova seznamu příloh publikace
                attachmentModel.AddAttachmentToPublication(originalPublication, openFile.FileName);
                refreshAttachments();
                statusLabel.Content = "Soubor připojen: " + openFile.FileName;
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při vkládání záznamu přílohy do databáze: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Chyba při připojování nového souboru: " + ex.Message,
                    "Chyba při čtení", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zkopíruje vybraný soubor v seznamu příloh zobrazené publikace
        /// z datového adresáře aplikace na zadanou cestu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void copyAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (attachmentDataGrid.SelectedItem == null)
            {
                return;
            }
            
            SaveFileDialog saveFile = new SaveFileDialog();

            // zrušení akce, pokud není zadána cílová cesta
            if (saveFile.ShowDialog() != true)
            {
                return;
            }

            Attachment attachment = attachmentDataGrid.SelectedItem as Attachment;

            try
            {
                // zkopírování přílohy
                attachmentModel.CopyAttachmentOfPublication(originalPublication, saveFile.FileName, attachment.Id);
                statusLabel.Content = "Soubor zkopírován: " + saveFile.FileName;
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při načítání záznamu přílohy z databáze: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Chyba při kopírování připojeného souboru: " + ex.Message,
                    "Chyba při zápisu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Odstraní vybraný soubor v seznamu příloh zobrazené publikace z datového adresáře aplikace
        /// a následně ho odebere z tohoto seznamu.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
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
                // odebrání přílohy a obnova seznamu příloh publikace
                attachmentModel.RemoveAttachmentFromPublication(originalPublication, attachment.Id);
                refreshAttachments();
                statusLabel.Content = "Soubor odstraněn: " + attachment.Path;
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Chyba při odstraňování záznamu přílohy z databáze: " + ex.Message,
                    "Chyba v databázi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Chyba při odstraňování připojeného souboru: " + ex.Message,
                    "Chyba při zápisu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Aktualizuje stav aktivace tlačítek pro stažení a odebrání přílohy
        /// při změně výběru v seznamu příloh publikace. Tlačítka jsou aktivní
        /// právě tehdy, pokud je výběr neprázdný.
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void attachmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            copyAttachmentButton.IsEnabled = attachmentDataGrid.SelectedItem != null;
            removeAttachmentButton.IsEnabled = attachmentDataGrid.SelectedItem != null;
        }
    }
}
