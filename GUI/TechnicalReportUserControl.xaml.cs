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
            InitializeComponent();
            this.technicalReportModel = technicalReportModel as TechnicalReportModel;
        }

        public void ViewPublication(Publication publication)
        {
            TechnicalReport technicalReport = publication.TechnicalReport;

            addressTextBox.Text = technicalReport.Address;
            institutionTextBox.Text = technicalReport.Institution;
            numberTextBox.Text = technicalReport.Number;
        }

        private TechnicalReport getPublicationTypeSpecificBibliography()
        {
            TechnicalReport technicalReport = new TechnicalReport();

            technicalReport.Address = addressTextBox.Text;
            technicalReport.Institution = institutionTextBox.Text;
            technicalReport.Number = numberTextBox.Text;

            return technicalReport;
        }

        public void InsertPublication(Publication publication, List<Author> authors)
        {
            TechnicalReport technicalReport = getPublicationTypeSpecificBibliography();
            technicalReportModel.CreatePublication(publication, authors, technicalReport);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors)
        {
            TechnicalReport technicalReport = getPublicationTypeSpecificBibliography();
            technicalReportModel.UpdatePublication(publicationId, publication, authors, technicalReport);
        }

        public void DeletePublication(int publicationId)
        {
            technicalReportModel.DeletePublication(publicationId);
        }
    }
}
