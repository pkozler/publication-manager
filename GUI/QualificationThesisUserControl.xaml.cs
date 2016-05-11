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
    /// Tato třída obsahuje interakční logiku pro QualificationThesisUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "kvalifikační práce".
    /// </summary>
    public partial class QualificationThesisUserControl : UserControl, IPublishableForm
    {
        private QualificationThesisModel qualificationThesisModel;
        
        public QualificationThesisUserControl(APublicationModel qualificationThesisModel) : base()
        {
            InitializeComponent();
            this.qualificationThesisModel = qualificationThesisModel as QualificationThesisModel;
        }

        public void ViewPublication(Publication publication)
        {
            QualificationThesis qualificationThesis = publication.QualificationThesis;

            addressTextBox.Text = qualificationThesis.Address;
            schoolTextBox.Text = qualificationThesis.School;

            if (qualificationThesis.ThesisType == QualificationThesisModel.TYPE_MASTER_THESIS)
            {
                masterThesisRadioButton.IsChecked = true;
            }
            else if (qualificationThesis.ThesisType == QualificationThesisModel.TYPE_PHD_THESIS)
            {
                phdThesisRadioButton.IsChecked = true;
            }
        }

        private QualificationThesis getPublicationTypeSpecificBibliography()
        {
            QualificationThesis qualificationThesis = new QualificationThesis();

            qualificationThesis.Address = addressTextBox.Text;
            qualificationThesis.School = schoolTextBox.Text;

            if (masterThesisRadioButton.IsChecked == true)
            {
                qualificationThesis.ThesisType = QualificationThesisModel.TYPE_MASTER_THESIS;
            }
            else if (phdThesisRadioButton.IsChecked == true)
            {
                qualificationThesis.ThesisType = QualificationThesisModel.TYPE_PHD_THESIS;
            }

            return qualificationThesis;
        }

        public void InsertPublication(Publication publication, List<Author> authors)
        {
            QualificationThesis qualificationThesis = getPublicationTypeSpecificBibliography();
            qualificationThesisModel.CreatePublication(publication, authors[0], qualificationThesis);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors)
        {
            QualificationThesis qualificationThesis = getPublicationTypeSpecificBibliography();
            qualificationThesisModel.UpdatePublication(publicationId, publication, authors[0], qualificationThesis);
        }

        public void DeletePublication(int publicationId)
        {
            qualificationThesisModel.DeletePublication(publicationId);
        }
    }
}
