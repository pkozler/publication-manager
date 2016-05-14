using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro ConferenceArticleUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "článek na konferenci".
    /// </summary>
    public partial class ConferenceArticleUserControl : UserControl, IPublicationForm
    {
        /// <summary>
        /// Uchovává instanci správce článků na konferenci v datové vrstvě.
        /// </summary>
        private ConferenceArticleModel conferenceArticleModel;

        /// <summary>
        /// Uchovává instanci validátoru pro hodnoty čísel stran textu.
        /// </summary>
        private PageNumberValidator pageNumberValidator = new PageNumberValidator();

        /// <summary>
        /// Inicializuje komponenty.
        /// </summary>
        /// <param name="conferenceArticleModel">správce článků na konferenci</param>
        public ConferenceArticleUserControl(APublicationModel conferenceArticleModel) : base()
        {
            this.conferenceArticleModel = conferenceArticleModel as ConferenceArticleModel;

            InitializeComponent();
            pageSingleRadioButton.IsChecked = true;
            isbnRadioButton.IsChecked = true;
        }

        /// <inheritDoc/>
        public void ViewPublication(Publication publication)
        {
            ConferenceArticle conferenceArticle = publication.ConferenceArticle;

            bookTitleTextBox.Text = conferenceArticle.BookTitle;
            addressTextBox.Text = conferenceArticle.Address;
            publisherTextBox.Text = conferenceArticle.Publisher;
            fromPageNumericUpDown.Value = conferenceArticle.FromPage;
            toPageNumericUpDown.Value = conferenceArticle.ToPage;

            if (conferenceArticle.FromPage == conferenceArticle.ToPage)
            {
                pageSingleRadioButton.IsChecked = true;
            }
            else
            {
                pageRangeRadioButton.IsChecked = true;
            }

            identificationTextBox.Text = string.IsNullOrEmpty(conferenceArticle.ISSN) ?
                conferenceArticle.ISBN : conferenceArticle.ISSN;

            if (!string.IsNullOrEmpty(conferenceArticle.ISBN))
            {
                isbnRadioButton.IsChecked = true;
            }
            else if (!string.IsNullOrEmpty(conferenceArticle.ISSN))
            {
                issnRadioButton.IsChecked = true;
            }
        }

        /// <inheritDoc/>
        public List<string> ValidatePublicationTypeSpecificBibliography(
            Publication publication, List<Author> authors, out ASpecificPublication specificPublication)
        {
            List<string> errors = new List<string>();
            specificPublication = new ConferenceArticle();
            ConferenceArticle conferenceArticle = specificPublication as ConferenceArticle;
            
            if (string.IsNullOrWhiteSpace(bookTitleTextBox.Text))
            {
                errors.Add("Název sborníku nesmí být prázdný.");
            }
            else
            {
                conferenceArticle.BookTitle = bookTitleTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(addressTextBox.Text))
            {
                errors.Add("Místo vydání nesmí být prázdné.");
            }
            else
            {
                conferenceArticle.Address = addressTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(publisherTextBox.Text))
            {
                errors.Add("Jméno vydavatele nesmí být prázdné.");
            }
            else
            {
                conferenceArticle.Publisher = publisherTextBox.Text;
            }

            conferenceArticle.FromPage = pageNumberValidator.
                validateFromPageNumber(errors, fromPageNumericUpDown);
            conferenceArticle.ToPage = pageNumberValidator.
                validateToPageNumber(errors, toPageNumericUpDown, pageSingleRadioButton,
                pageRangeRadioButton, conferenceArticle.FromPage);
            
            if (string.IsNullOrWhiteSpace(identificationTextBox.Text))
            {
                errors.Add("Identifikační číslo (ISBN/ISSN) nesmí být prázdné.");
            }
            else
            {
                if (isbnRadioButton.IsChecked == true)
                {
                    conferenceArticle.ISBN = identificationTextBox.Text;
                }
                else if (issnRadioButton.IsChecked == true)
                {
                    conferenceArticle.ISSN = identificationTextBox.Text;
                }
                else
                {
                    errors.Add("Musí být vybrán typ identifikačního čísla.");
                }
            }
            
            return errors;
        }

        /// <inheritDoc/>
        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            conferenceArticleModel.CreatePublication(publication, authors, specificPublication as ConferenceArticle);
        }

        /// <inheritDoc/>
        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            conferenceArticleModel.UpdatePublication(publicationId, publication, authors, specificPublication as ConferenceArticle);
        }

        /// <inheritDoc/>
        public void DeletePublication(int publicationId)
        {
            conferenceArticleModel.DeletePublication(publicationId);
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
