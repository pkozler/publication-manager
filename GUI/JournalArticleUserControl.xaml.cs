using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro JournalArticleUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "článek do časopisu".
    /// </summary>
    public partial class JournalArticleUserControl : UserControl, IPublicationForm
    {
        /// <summary>
        /// Uchovává instanci správce článků do časopisu v datové vrstvě.
        /// </summary>
        private JournalArticleModel journalArticleModel;

        /// <summary>
        /// Uchovává instanci validátoru pro hodnoty čísel stran textu.
        /// </summary>
        private PageNumberValidator pageNumberValidator = new PageNumberValidator();

        /// <summary>
        /// Inicializuje komponenty.
        /// </summary>
        /// <param name="journalArticleModel">správce článků do časopisu</param>
        public JournalArticleUserControl(APublicationModel journalArticleModel) : base()
        {
            this.journalArticleModel = journalArticleModel as JournalArticleModel;

            InitializeComponent();
            pageSingleRadioButton.IsChecked = true;
        }

        /// <inheritDoc/>
        public void ViewPublication(Publication publication)
        {
            JournalArticle journalArticle = publication.JournalArticle;

            journalTitleTextBox.Text = journalArticle.JournalTitle;
            numberTextBox.Text = journalArticle.Number;
            fromPageNumericUpDown.Value = journalArticle.FromPage;
            toPageNumericUpDown.Value = journalArticle.ToPage;

            if (journalArticle.FromPage == journalArticle.ToPage)
            {
                pageSingleRadioButton.IsChecked = true;
            }
            else
            {
                pageRangeRadioButton.IsChecked = true;
            }

            issnTextBox.Text = journalArticle.ISSN;
        }

        /// <inheritDoc/>
        public List<string> ValidatePublicationTypeSpecificBibliography(
            Publication publication, List<Author> authors, out ASpecificPublication specificPublication)
        {
            List<string> errors = new List<string>();
            specificPublication = new JournalArticle();
            JournalArticle journalArticle = specificPublication as JournalArticle;

            if (string.IsNullOrWhiteSpace(journalTitleTextBox.Text))
            {
                errors.Add("Název časopisu nesmí být prázdný.");
            }
            else
            {
                journalArticle.JournalTitle = journalTitleTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(numberTextBox.Text))
            {
                errors.Add("Číslo časopisu nesmí být prázdné.");
            }
            else
            {
                journalArticle.Number = numberTextBox.Text;
            }

            journalArticle.FromPage = pageNumberValidator.
                validateFromPageNumber(errors, fromPageNumericUpDown);
            journalArticle.ToPage = pageNumberValidator.
                validateToPageNumber(errors, toPageNumericUpDown, pageSingleRadioButton,
                pageRangeRadioButton, journalArticle.FromPage);

            if (string.IsNullOrWhiteSpace(issnTextBox.Text))
            {
                errors.Add("ISSN nesmí být prázdné.");
            }
            else
            {
                journalArticle.ISSN = issnTextBox.Text;
            }
            
            return errors;
        }

        /// <inheritDoc/>
        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            journalArticleModel.CreatePublication(publication, authors, specificPublication as JournalArticle);
        }

        /// <inheritDoc/>
        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            journalArticleModel.UpdatePublication(publicationId, publication, authors, specificPublication as JournalArticle);
        }

        /// <inheritDoc/>
        public void DeletePublication(int publicationId)
        {
            journalArticleModel.DeletePublication(publicationId);
        }

        /// <summary>
        /// Nastaví způsob zadání čísel stran textu (jednostránkový nebo rozsah).
        /// </summary>
        /// <param name="sender">původce události</param>
        /// <param name="e">data události</param>
        private void setPageInterval(object sender, RoutedEventArgs e)
        {
            pageNumberValidator.SetNumericUpDownControls(
                pageSingleRadioButton, pageRangeRadioButton, fromPageNumericUpDown, toPageNumericUpDown);
        }
    }
}
