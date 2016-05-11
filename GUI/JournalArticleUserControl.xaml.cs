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

        private JournalArticle getPublicationTypeSpecificBibliography()
        {
            JournalArticle journalArticle = new JournalArticle();

            journalArticle.JournalTitle = journalTitleTextBox.Text;
            journalArticle.Number = numberTextBox.Text;
            journalArticle.FromPage = int.Parse(fromPageTextBox.Text);
            journalArticle.ToPage = int.Parse(toPageTextBox.Text);
            journalArticle.ISSN = issnTextBox.Text;

            return journalArticle;
        }

        public void InsertPublication(Publication publication, List<Author> authors)
        {
            JournalArticle journalArticle = getPublicationTypeSpecificBibliography();
            journalArticleModel.CreatePublication(publication, authors, journalArticle);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors)
        {
            JournalArticle journalArticle = getPublicationTypeSpecificBibliography();
            journalArticleModel.UpdatePublication(publicationId, publication, authors, journalArticle);
        }

        public void DeletePublication(int publicationId)
        {
            journalArticleModel.DeletePublication(publicationId);
        }
    }
}
