using System.Collections.Generic;
using System.Windows.Controls;
using Core;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro QualificationThesisUserControl.xaml
    /// představující část formuláře okna pro zobrazení a úpravu bibliografických údajů
    /// publikací, která je specifická pro typ publikace "kvalifikační práce".
    /// </summary>
    public partial class QualificationThesisUserControl : UserControl, IPublicationForm
    {
        /// <summary>
        /// Uchovává instanci správce kvalifikačních prací v datové vrstvě.
        /// </summary>
        private QualificationThesisModel qualificationThesisModel;

        /// <summary>
        /// Inicializuje komponenty.
        /// </summary>
        /// <param name="qualificationThesisModel">správce kvalifikačních prací</param>
        public QualificationThesisUserControl(APublicationModel qualificationThesisModel) : base()
        {
            this.qualificationThesisModel = qualificationThesisModel as QualificationThesisModel;

            InitializeComponent();
            masterThesisRadioButton.IsChecked = true;
        }

        /// <inheritDoc/>
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

        /// <inheritDoc/>
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

        /// <inheritDoc/>
        public void InsertPublication(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            qualificationThesisModel.CreatePublication(publication, authors[0], specificPublication as QualificationThesis);
        }

        /// <inheritDoc/>
        public void EditPublication(int publicationId, Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            qualificationThesisModel.UpdatePublication(publicationId, publication, authors[0], specificPublication as QualificationThesis);
        }

        /// <inheritDoc/>
        public void DeletePublication(int publicationId)
        {
            qualificationThesisModel.DeletePublication(publicationId);
        }
    }
}
