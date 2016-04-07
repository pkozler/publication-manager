using Core;

namespace CLI
{
    /// <summary>
    /// Spouštěcí třída konzolového uživatelského rozhraní.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Spustí program v režimu hlavního menu.
        /// </summary>
        /// <param name="args">parametry příkazového řádku</param>
        static void Main(string[] args)
        {
            // vytvoření objektů pro správu dat
            ConferenceArticleModel conferenceArticleModel = new ConferenceArticleModel();
            JournalArticleModel journalArticleModel = new JournalArticleModel();
            TechnicalReportModel technicalReportModel = new TechnicalReportModel();
            QualificationThesisModel qualificationThesisModel = new QualificationThesisModel();
            PublicationModel publicationModel = new PublicationModel(
                conferenceArticleModel, 
                journalArticleModel, 
                technicalReportModel, 
                qualificationThesisModel);
            AuthorModel authorModel = new AuthorModel();
            AttachmentModel attachmentModel = new AttachmentModel();

            // vytvoření menu a spuštění načítání příkazů
            MainMenu mainMenu = new MainMenu(publicationModel, authorModel, attachmentModel);
            mainMenu.Start();
        }
    }
}