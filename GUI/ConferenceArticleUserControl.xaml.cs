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

        private ConferenceArticle getPublicationTypeSpecificBibliography()
        {
            ConferenceArticle conferenceArticle = new ConferenceArticle();

            conferenceArticle.BookTitle = bookTitleTextBox.Text;
            conferenceArticle.Address = addressTextBox.Text;
            conferenceArticle.Publisher = publisherTextBox.Text;
            conferenceArticle.FromPage = int.Parse(fromPageTextBox.Text);
            conferenceArticle.ToPage = int.Parse(toPageTextBox.Text);
            identificationTextBox.Text = string.IsNullOrEmpty(conferenceArticle.ISSN) ?
                conferenceArticle.ISBN : conferenceArticle.ISSN;

            if (isbnRadioButton.IsChecked == true)
            {
                conferenceArticle.ISBN = identificationTextBox.Text;
            }
            else if (issnRadioButton.IsChecked == true)
            {
                conferenceArticle.ISSN = identificationTextBox.Text;
            }

            return conferenceArticle;
        }

        public void InsertPublication(Publication publication, List<Author> authors)
        {
            ConferenceArticle conferenceArticle = getPublicationTypeSpecificBibliography();
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors)
        {
            ConferenceArticle conferenceArticle = getPublicationTypeSpecificBibliography();
            conferenceArticleModel.UpdatePublication(publicationId, publication, authors, conferenceArticle);
        }

        public void DeletePublication(int publicationId)
        {
            conferenceArticleModel.DeletePublication(publicationId);
        }
    }
}
