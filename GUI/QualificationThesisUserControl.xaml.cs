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
            this.qualificationThesisModel = qualificationThesisModel as QualificationThesisModel;

            InitializeComponent();
            masterThesisRadioButton.IsChecked = true;
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

        public List<string> ValidatePublicationTypeSpecificBibliography(
            Publication publication, List<Author> authors, out ASpecificPublication specificPublication)
        {
            List<string> errors = new List<string>();
            specificPublication = new QualificationThesis();
            QualificationThesis qualificationThesis = specificPublication as QualificationThesis;

            if (authors.Count != 1)
            {
                errors.Add("Kvalifikační práce nesmí mít více autorů.");
            }

            if (string.IsNullOrWhiteSpace(addressTextBox.Text))
            {
                errors.Add("Místo vytvoření nesmí být prázdné.");
            }
            else
            {
                qualificationThesis.Address = addressTextBox.Text;
            }

            if (string.IsNullOrWhiteSpace(schoolTextBox.Text))
            {
                errors.Add("Název školy nesmí být prázdný.");
            }
            else
            {
                qualificationThesis.School = schoolTextBox.Text;
            }

            if (masterThesisRadioButton.IsChecked == true)
            {
                qualificationThesis.ThesisType = QualificationThesisModel.TYPE_MASTER_THESIS;
            }
            else if (phdThesisRadioButton.IsChecked == true)
            {
                qualificationThesis.ThesisType = QualificationThesisModel.TYPE_PHD_THESIS;
            }
            else
            {
                errors.Add("Musí být vybrán typ kvalifikační práce.");
            }
            
            return errors;
        }

        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            qualificationThesisModel.CreatePublication(publication, authors[0], specificPublication as QualificationThesis);
        }

        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            qualificationThesisModel.UpdatePublication(publicationId, publication, authors[0], specificPublication as QualificationThesis);
        }

        public void DeletePublication(int publicationId)
        {
            qualificationThesisModel.DeletePublication(publicationId);
        }
    }
}
