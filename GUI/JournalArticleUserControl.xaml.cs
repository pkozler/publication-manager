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
    /// Tato třída obsahuje interakční logiku pro JournalArticleUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "článek do časopisu".
    /// </summary>
    public partial class JournalArticleUserControl : UserControl, IPublishableForm
    {
        private JournalArticleModel journalArticleModel;
        
        public JournalArticleUserControl(APublicationModel journalArticleModel) : base()
        {
            InitializeComponent();
            this.journalArticleModel = journalArticleModel as JournalArticleModel;
        }

        public void ViewPublication(Publication publication)
        {
            JournalArticle journalArticle = publication.JournalArticle;

            journalTitleTextBox.Text = journalArticle.JournalTitle;
            numberTextBox.Text = journalArticle.Number;
            fromPageTextBox.Text = journalArticle.FromPage.ToString();
            toPageTextBox.Text = journalArticle.ToPage.ToString();
            issnTextBox.Text = journalArticle.ISSN;
        }

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

            int fromPage;
            bool isFromPageEmpty = string.IsNullOrWhiteSpace(fromPageTextBox.Text);
            bool isFromPageValid = int.TryParse(fromPageTextBox.Text, out fromPage) && fromPage > 0;

            if (!isFromPageEmpty)
            {
                if (!isFromPageValid)
                {

                    errors.Add("Číslo počáteční strany časopisu s citovaným textem musí být platné kladné celé číslo.");
                }
                else
                {
                    journalArticle.FromPage = fromPage;
                }
            }

            int toPage;
            bool isToPageEmpty = string.IsNullOrWhiteSpace(toPageTextBox.Text);
            bool isToPageValid = int.TryParse(toPageTextBox.Text, out toPage) && toPage > 0;

            if (!isToPageEmpty)
            {
                if (!isToPageValid)
                {
                    errors.Add("Číslo poslední strany časopisu s citovaným textem musí být platné kladné celé číslo.");
                }
                else
                {
                    journalArticle.ToPage = toPage;
                }
            }

            if (isToPageEmpty && isFromPageValid)
            {
                journalArticle.ToPage = fromPage;
            }
            else if (isFromPageEmpty && isToPageValid)
            {
                journalArticle.FromPage = 1;
            }
            else
            {
                errors.Add("Musí být uvedena alespoň jedna ze stran časopisu, které ohraničují citovaný text.");
            }

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

        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            journalArticleModel.CreatePublication(publication, authors, specificPublication as JournalArticle);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            journalArticleModel.UpdatePublication(publicationId, publication, authors, specificPublication as JournalArticle);
        }

        public void DeletePublication(int publicationId)
        {
            journalArticleModel.DeletePublication(publicationId);
        }
    }
}
