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

        /// <summary>
        /// Provede inicializaci komponent.
        /// </summary>
        public QualificationThesisUserControl()
        {
            //InitializeComponent();
        }

        public QualificationThesisUserControl(APublicationModel qualificationThesisModel) : base()
        {
            this.qualificationThesisModel = qualificationThesisModel as QualificationThesisModel;
        }

        public APublicationModel GetModel()
        {
            throw new NotImplementedException();
        }

        public void GetSpecificBibliography(Publication publication)
        {
            throw new NotImplementedException();
        }

        public void SetModel(APublicationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
