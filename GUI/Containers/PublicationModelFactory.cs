using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Core;

namespace GUI
{
    class PublicationModelFactory
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        public readonly PublicationModel PublicationModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        public readonly AuthorModel AuthorModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        public readonly AttachmentModel AttachmentModel;

        /// <summary>
        /// Seznam typů publikací, který slouží k uchování údajů specifických
        /// pro jednotlivé typy publikací a objektů datové vrstvy pro jejich obsluhu.
        /// </summary>
        public readonly List<PublicationType> PublicationTypes;

        /// <summary>
        /// Provede inicializaci objektů datové vrstvy.
        /// </summary>
        public PublicationModelFactory()
        {
            // vytvoření databázového kontextu
            DbPublicationEntities context = new DbPublicationEntities();

            // vytvoření objektů pro správu dat společných pro všechny typy publikací
            PublicationModel = new PublicationModel(context);
            AuthorModel = new AuthorModel(context);
            AttachmentModel = new AttachmentModel(context);

            // vytvoření seznamu typů publikací s přidruženými dialogy a objekty pro správu dat
            PublicationTypes = initializePublicationTypes(context);
        }

        /// <summary>
        /// Inicializuje objekty pro správu jednotlivých dostupných typů publikací.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        /// <returns>seznam typů publikací s inicializovanými objekty pro jejich správu</returns>
        private List<PublicationType> initializePublicationTypes(DbPublicationEntities context)
        {
            // vytvoření objektů pro správu dat specifických pro jednotlivé dostupné typy publikací
            ConferenceArticleModel conferenceArticleModel = new ConferenceArticleModel(context, "Článek na konferenci");
            JournalArticleModel journalArticleModel = new JournalArticleModel(context, "Článek do časopisu");
            TechnicalReportModel technicalReportModel = new TechnicalReportModel(context, "Technická zpráva");
            QualificationThesisModel qualificationThesisModel = new QualificationThesisModel(context, "Kvalifikační práce");

#if DEBUG
            initializeData(conferenceArticleModel, journalArticleModel, technicalReportModel, qualificationThesisModel);
#endif

            /*
                přiřazení dialogů uživatelského rozhraní k jednotlivým dostupným typům publikací,
                propojení s příslušnými objekty datové vrstvy a uložení typů publikací do seznamu
            */
            return new List<PublicationType>()
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
        }

        /// <summary>
        /// Načte testovací text zadané publikace.
        /// </summary>
        /// <param name="entry">publikace</param>
        private string ReadPublicationText(Publication publication)
        {
            const string DIRECTORY = "../../resources/example-texts/";
            const string EXTENSION = ".txt";

            return File.ReadAllText(DIRECTORY + publication.Entry + EXTENSION);
        }
        
        /// <summary>
        /// Naplní databázi testovacími daty.
        /// </summary>
        private void initializeData(ConferenceArticleModel conferenceArticleModel,
            JournalArticleModel journalArticleModel,
            TechnicalReportModel technicalReportModel,
            QualificationThesisModel qualificationThesisModel)
        {
            Publication publication;
            List<Author> authors;
            ConferenceArticle conferenceArticle;
            JournalArticle journalArticle;
            TechnicalReport technicalReport;
            QualificationThesis qualificationThesis;
            
            publication = new Publication();
            publication.Entry = "logika-matematika";
            publication.Title = "Logika není matematika";
            publication.Year = 2005;
            publication.Text = ReadPublicationText(publication);
            publication.Type = ConferenceArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Ludmila", Surname = "Dostálová" },
            };
            conferenceArticle = new ConferenceArticle();
            conferenceArticle.BookTitle = "Univerzita: tradiční a netradiční ve výuce";
            conferenceArticle.Address = "Plzeň";
            conferenceArticle.Publisher = "Čeněk";
            conferenceArticle.FromPage = 162;
            conferenceArticle.ToPage = 170;
            conferenceArticle.ISBN = "80‑86898‑60‑1";
            conferenceArticleModel.CreatePublication(publication, authors, conferenceArticle);

            publication = new Publication();
            publication.Entry = "aktualni-trendy";
            publication.Title = "Aktuální trendy v oblasti elektroniky";
            publication.Year = 2006;
            publication.Text = ReadPublicationText(publication);
            publication.Type = JournalArticleModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Petr", Surname = "Beneš" },
            };
            journalArticle = new JournalArticle();
            journalArticle.JournalTitle = "Sdělovací technika: telekomunikace, elektronika, multimédia. Praha: Sdělovací technika";
            journalArticle.Number = "54(12)";
            journalArticle.FromPage = 3;
            journalArticle.ToPage = 6;
            journalArticle.ISSN = "0036‑9942";
            journalArticleModel.CreatePublication(publication, authors, journalArticle);

            publication = new Publication();
            publication.Entry = "directed-design";
            publication.Title = "Directed Design of Thermal Insulation System of Contemporary Buildings Based on Hygrothermal Analysis - Progress Report";
            publication.Year = 2010;
            publication.Text = ReadPublicationText(publication);
            publication.Type = TechnicalReportModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "R.", Surname ="Černý" },
            };
            technicalReport = new TechnicalReport();
            technicalReport.Address = "Praha";
            technicalReport.Institution = "ČVUT v Praze, Fakulta stavební, Katedra materiálového inženýrství a chemie";
            technicalReport.Number = "p. 51 [Research Report ME10112/1]";
            technicalReportModel.CreatePublication(publication, authors, technicalReport);

            publication = new Publication();
            publication.Entry = "analyza-vlastnosti";
            publication.Title = "Analýza prostorových a formálních vlastností středověkých obléhacích táborů";
            publication.Year = 2010;
            publication.Text = ReadPublicationText(publication);
            publication.Type = QualificationThesisModel.NAME;
            authors = new List<Author>()
            {
                new Author() { Name = "Petr", Surname = "Koscelník" },
            };
            qualificationThesis = new QualificationThesis();
            qualificationThesis.Address = "Plzeň";
            qualificationThesis.School = "Západočeská univerzita v Plzni. Fakulta filozofická";
            qualificationThesis.ThesisType = QualificationThesisModel.TYPE_MASTER_THESIS;
            qualificationThesisModel.CreatePublication(publication, authors[0], qualificationThesis);
        }
    }
}
