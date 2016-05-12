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
        
        public ConferenceArticleUserControl(APublicationModel conferenceArticleModel) : base()
        {
            InitializeComponent();
            this.conferenceArticleModel = conferenceArticleModel as ConferenceArticleModel;
        }

        public void ViewPublication(Publication publication)
        {
            ConferenceArticle conferenceArticle = publication.ConferenceArticle;

            bookTitleTextBox.Text = conferenceArticle.BookTitle;
            addressTextBox.Text = conferenceArticle.Address;
            publisherTextBox.Text = conferenceArticle.Publisher;
            fromPageTextBox.Text = conferenceArticle.FromPage.ToString();
            toPageTextBox.Text = conferenceArticle.ToPage.ToString();
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

            int fromPage;
            bool isFromPageEmpty = string.IsNullOrWhiteSpace(fromPageTextBox.Text);
            bool isFromPageValid = int.TryParse(fromPageTextBox.Text, out fromPage) && fromPage > 0;

            if (!isFromPageEmpty)
            {
                if (!isFromPageValid)
                {

                    errors.Add("Číslo počáteční strany sborníku s citovaným textem musí být platné kladné celé číslo.");
                }
                else
                {
                    conferenceArticle.FromPage = fromPage;
                }
            }

            int toPage;
            bool isToPageEmpty = string.IsNullOrWhiteSpace(toPageTextBox.Text);
            bool isToPageValid = int.TryParse(toPageTextBox.Text, out toPage) && toPage > 0;

            if (!isToPageEmpty)
            {
                if (!isToPageValid)
                {
                    errors.Add("Číslo poslední strany sborníku s citovaným textem musí být platné kladné celé číslo.");
                }
                else
                {
                    conferenceArticle.ToPage = toPage;
                }
            }

            if (isToPageEmpty && isFromPageValid)
            {
                conferenceArticle.ToPage = fromPage;
            }
            else if (isFromPageEmpty && isToPageValid)
            {
                conferenceArticle.FromPage = 1;
            }
            else
            {
                errors.Add("Musí být uvedena alespoň jedna ze stran sborníku, které ohraničují citovaný text.");
            }

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
    }
}
