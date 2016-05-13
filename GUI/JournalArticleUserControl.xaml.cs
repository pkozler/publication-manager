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
    /// Tato třída obsahuje interakční logiku pro JournalArticleUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "článek do časopisu".
    /// </summary>
    public partial class JournalArticleUserControl : UserControl, IPublishableForm
    {
        private JournalArticleModel journalArticleModel;

        private PageNumberHelper pageNumberHelper = new PageNumberHelper();

        public JournalArticleUserControl(APublicationModel journalArticleModel) : base()
        {
            this.journalArticleModel = journalArticleModel as JournalArticleModel;

            InitializeComponent();
            pageSingleRadioButton.IsChecked = true;
        }

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

            journalArticle.FromPage = pageNumberHelper.
                validateFromPageNumber(errors, fromPageNumericUpDown);
            journalArticle.ToPage = pageNumberHelper.
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

        private void setPageInterval(object sender, RoutedEventArgs e)
        {
            pageNumberHelper.SetNumericUpDownControls(
                pageSingleRadioButton, pageRangeRadioButton, fromPageNumericUpDown, toPageNumericUpDown);
        }
    }
}
