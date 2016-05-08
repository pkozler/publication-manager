using System;
using System.Collections.Generic;
using Core;
using System.IO;

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
        /// <param name="context">databázový kontext</param>
        /// <returns>seznam typů publikací s inicializovanými objekty pro jejich správu</returns>
        private static List<PublicationType> initializePublicationTypes(DbPublicationEntities context)
        {
            // vytvoření objektů pro správu dat specifických pro jednotlivé dostupné typy publikací
            ConferenceArticleModel conferenceArticleModel = new ConferenceArticleModel(context, "Článek na konferenci");
            JournalArticleModel journalArticleModel = new JournalArticleModel(context, "Článek do časopisu");
            TechnicalReportModel technicalReportModel = new TechnicalReportModel(context, "Technická zpráva");
            QualificationThesisModel qualificationThesisModel = new QualificationThesisModel(context, "Kvalifikační práce");

            /*
                přiřazení dialogů uživatelského rozhraní k jednotlivým dostupným typům publikací,
                propojení s příslušnými objekty datové vrstvy a uložení typů publikací do seznamu
            */
            return new List<PublicationType>()
            {
                new PublicationType(ConferenceArticleModel.NAME, new ConferenceArticleDialog(conferenceArticleModel)),
                new PublicationType(JournalArticleModel.NAME, new JournalArticleDialog(journalArticleModel)),
                new PublicationType(TechnicalReportModel.NAME, new TechnicalReportDialog(technicalReportModel)),
                new PublicationType(QualificationThesisModel.NAME, new QualificationThesisDialog(qualificationThesisModel)),
            };
        }

        /// <summary>
        /// Provede inicializaci objektů datové vrstvy a spustí program v režimu hlavního menu.
        /// </summary>
        /// <param name="args">parametry příkazového řádku</param>
        static void Main(string[] args)
        {
            // vytvoření databázového kontextu
            DbPublicationEntities context = new DbPublicationEntities();

            // vytvoření objektů pro správu dat společných pro všechny typy publikací
            PublicationModel publicationModel = new PublicationModel(context);
            AuthorModel authorModel = new AuthorModel(context);
            AttachmentModel attachmentModel = new AttachmentModel(context);

            // vytvoření seznamu typů publikací s přidruženými dialogy a objekty pro správu dat
            List<PublicationType> publicationTypes = initializePublicationTypes(context);

            // vytvoření instance hlavního menu, která je propojena s vytvořenými objekty datové vrstvy
            MainMenu mainMenu = new MainMenu(publicationTypes, publicationModel, authorModel, attachmentModel);

            // spuštění načítání příkazů
            mainMenu.Start();
        }
    }
}