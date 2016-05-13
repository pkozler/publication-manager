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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core;
using System.Text.RegularExpressions;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro ConferenceArticleUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "článek na konferenci".
    /// </summary>
    public partial class ConferenceArticleUserControl : UserControl, IPublishableForm
    {
        private ConferenceArticleModel conferenceArticleModel;

        private PageNumberHelper pageNumberHelper = new PageNumberHelper();

        public ConferenceArticleUserControl(APublicationModel conferenceArticleModel) : base()
        {
            this.conferenceArticleModel = conferenceArticleModel as ConferenceArticleModel;

            InitializeComponent();
            pageSingleRadioButton.IsChecked = true;
            isbnRadioButton.IsChecked = true;
        }

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

            conferenceArticle.FromPage = pageNumberHelper.
                validateFromPageNumber(errors, fromPageNumericUpDown);
            conferenceArticle.ToPage = pageNumberHelper.
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

        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            conferenceArticleModel.CreatePublication(publication, authors, specificPublication as ConferenceArticle);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            conferenceArticleModel.UpdatePublication(publicationId, publication, authors, specificPublication as ConferenceArticle);
        }

        public void DeletePublication(int publicationId)
        {
            conferenceArticleModel.DeletePublication(publicationId);
        }
        
        private void setPageInterval(object sender, RoutedEventArgs e)
        {
            pageNumberHelper.SetNumericUpDownControls(
                pageSingleRadioButton, pageRangeRadioButton, fromPageNumericUpDown, toPageNumericUpDown);
        }
    }
}
