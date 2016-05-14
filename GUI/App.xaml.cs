using System;
using System.Collections.Generic;
using System.Windows;
using Core;

namespace GUI
{
    /// <summary>
    /// Tato třída obsahuje interakční logiku pro App.xaml
    /// a obsahuje metodu, která představuje vstupní bod programu
    /// a slouží k propojení datové vrstvy s uživatelským rozhraním.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Provede inicializaci objektů datové vrstvy a propojí je s hlavním oknem
        /// aplikace, které následně zobrazí.
        /// </summary>
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();

                // vytvoření databázového kontextu
                DbPublicationEntities context = new DbPublicationEntities();

                // vytvoření objektů pro správu dat společných pro všechny typy publikací
                PublicationModel publicationModel = new PublicationModel(context);
                AuthorModel authorModel = new AuthorModel(context);
                AttachmentModel attachmentModel = new AttachmentModel(context, args[0]);

                // vytvoření seznamu typů publikací s přidruženými dialogy a objekty pro správu dat
                List<PublicationType> publicationTypes = initializeTypes(context);
                
                new MainWindow(publicationModel, authorModel, 
                    attachmentModel, publicationTypes).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba při inicializaci");
                Current.Shutdown(-1);
            }
        }

        /// <summary>
        /// Inicializuje objekty datové vrstvy pro správu jednotlivých dostupných typů publikací
        /// a propojí je s odpovídajícími formuláři uživatelského rozhraní prostřednictvím instancí
        /// obalové třídy, které následně uloží do seznamu.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        /// <returns>seznam typů publikací s inicializovanými objekty pro jejich správu</returns>
        private static List<PublicationType> initializeTypes(DbPublicationEntities context)
        {
            // vytvoření objektů pro správu dat specifických pro jednotlivé dostupné typy publikací
            ConferenceArticleModel conferenceArticleModel
                = new ConferenceArticleModel(context, "Článek na konferenci");
            JournalArticleModel journalArticleModel
                = new JournalArticleModel(context, "Článek do časopisu");
            TechnicalReportModel technicalReportModel
                = new TechnicalReportModel(context, "Technická zpráva");
            QualificationThesisModel qualificationThesisModel
                = new QualificationThesisModel(context, "Kvalifikační práce", "Diplomová práce", "Disertační práce");

            /*
                přiřazení dialogů uživatelského rozhraní k jednotlivým dostupným typům publikací,
                propojení s příslušnými objekty datové vrstvy a uložení typů publikací do seznamu
            */
            List<PublicationType> publicationTypes = new List<PublicationType>()
            {
                new PublicationType(ConferenceArticleModel.NAME, conferenceArticleModel,
                    (model) => {
                        return new ConferenceArticleUserControl(model);
                    }),
                new PublicationType(JournalArticleModel.NAME, journalArticleModel,
                    (model) => {
                        return new JournalArticleUserControl(model);
                    }),
                new PublicationType(TechnicalReportModel.NAME, technicalReportModel,
                    (model) => {
                        return new TechnicalReportUserControl(model);
                    }),
                new PublicationType(QualificationThesisModel.NAME, qualificationThesisModel,
                    (model) => {
                        return new QualificationThesisUserControl(model);
                    }),
            };

            return publicationTypes;
        }
    }
}
