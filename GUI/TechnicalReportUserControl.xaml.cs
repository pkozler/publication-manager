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
    /// Tato třída obsahuje interakční logiku pro TechnicalReportUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "technická zpráva".
    /// </summary>
    public partial class TechnicalReportUserControl : UserControl, IPublishableForm
    {
        private TechnicalReportModel technicalReportModel;
        
        public TechnicalReportUserControl(APublicationModel technicalReportModel) : base()
        {
            this.technicalReportModel = technicalReportModel as TechnicalReportModel;

            InitializeComponent();
        }

        public void ViewPublication(Publication publication)
        {
            TechnicalReport technicalReport = publication.TechnicalReport;

            addressTextBox.Text = technicalReport.Address;
            institutionTextBox.Text = technicalReport.Institution;
            numberTextBox.Text = technicalReport.Number;
        }

        public List<string> ValidatePublicationTypeSpecificBibliography(
            Publication publication, List<Author> authors, out ASpecificPublication specificPublication)
        {
            List<string> errors = new List<string>();
            specificPublication = new TechnicalReport();
            TechnicalReport technicalReport = specificPublication as TechnicalReport;

            if (string.IsNullOrWhiteSpace(addressTextBox.Text))
            {
                errors.Add("Místo vydání nesmí být prázdné.");
            }
            else
            {
                technicalReport.Address = addressTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(institutionTextBox.Text))
            {
                errors.Add("Jméno vydavatele nesmí být prázdné.");
            }
            else
            {
                technicalReport.Institution = institutionTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(numberTextBox.Text))
            {
                errors.Add("Číslo s označením zprávy nesmí být prázdné.");
            }
            else
            {
                technicalReport.Number = numberTextBox.Text;
            }
            
            return errors;
        }

        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            technicalReportModel.CreatePublication(publication, authors, specificPublication as TechnicalReport);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            technicalReportModel.UpdatePublication(publicationId, publication, authors, specificPublication as TechnicalReport);
        }

        public void DeletePublication(int publicationId)
        {
            technicalReportModel.DeletePublication(publicationId);
        }
    }
}
