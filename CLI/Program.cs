using System;
using System.Collections.Generic;
using Core;

namespace CLI
{
    /// <summary>
    /// Spouštěcí třída konzolového uživatelského rozhraní.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Inicializuje objekty pro správu jednotlivých dostupných typů publikací.
        /// </summary>
        /// <param name="publicationModel">správce základních údajů publikací</param>
        /// <returns>seznam typů publikací s inicializovanými objekty pro jejich správu</returns>
        private static List<PublicationType> initializePublicationTypes()
        {
            // vytvoření objektů pro správu dat specifických pro jednotlivé dostupné typy publikací
            ConferenceArticleModel conferenceArticleModel = new ConferenceArticleModel();
            JournalArticleModel journalArticleModel = new JournalArticleModel();
            TechnicalReportModel technicalReportModel = new TechnicalReportModel();
            QualificationThesisModel qualificationThesisModel = new QualificationThesisModel();

            /*
                přiřazení dialogů uživatelského rozhraní k jednotlivým dostupným typům publikací,
                propojení s příslušnými objekty datové vrstvy a uložení typů publikací do seznamu
            */
            return new List<PublicationType>()
            {
                new PublicationType(ConferenceArticleModel.NAME, 
                    "Článek na konferenci", "Templates/conference-article.st", new ConferenceArticleDialog(conferenceArticleModel)),
                new PublicationType(JournalArticleModel.NAME, 
                    "Článek do časopisu", "Templates/journal-article.st", new JournalArticleDialog(journalArticleModel)),
                new PublicationType(TechnicalReportModel.NAME, 
                    "Technická zpráva", "Templates/technical-report.st", new TechnicalReportDialog(technicalReportModel)),
                new PublicationType(QualificationThesisModel.NAME, 
                    "Kvalifikační práce", "Templates/qualification-thesis.st", new QualificationThesisDialog(qualificationThesisModel)),
            };
        }

        /// <summary>
        /// Provede inicializaci objektů datové vrstvy a spustí program v režimu hlavního menu.
        /// </summary>
        /// <param name="args">parametry příkazového řádku</param>
        static void Main(string[] args)
        {
            // vytvoření objektů pro správu dat společných pro všechny typy publikací
            PublicationModel publicationModel = new PublicationModel();
            AuthorModel authorModel = new AuthorModel();
            AttachmentModel attachmentModel = new AttachmentModel();

            // vytvoření seznamu typů publikací s přidruženými dialogy a objekty pro správu dat
            List<PublicationType> publicationTypes = initializePublicationTypes();

            // vytvoření instance hlavního menu, která je propojena s vytvořenými objekty datové vrstvy
            MainMenu mainMenu = new MainMenu(publicationTypes, publicationModel, authorModel, attachmentModel);

            // spuštění načítání příkazů
            mainMenu.Start();
        }
    }
}