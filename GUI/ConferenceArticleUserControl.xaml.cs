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

        /// <summary>
        /// Provede inicializaci komponent.
        /// </summary>
        public ConferenceArticleUserControl()
        {
            InitializeComponent();
        }

        public ConferenceArticleUserControl(APublicationModel conferenceArticleModel) : base()
        {
            this.conferenceArticleModel = conferenceArticleModel as ConferenceArticleModel;
        }

        public APublicationModel GetModel()
        {
            throw new NotImplementedException();
        }

        public void GetSpecificBibliography(Publication publication)
        {
            throw new NotImplementedException();
        }
    }
}
