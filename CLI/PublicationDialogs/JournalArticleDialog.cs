using System.Collections.Generic;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída zobrazuje dialog pro zadání údajů o publikaci typu
    /// "článek v časopise".
    /// </summary>
    class JournalArticleDialog : IPublishableDialog
    {
        /// <summary>
        /// Uchovává objekt datové vrstvy pro práci s příslušným typem publikace.
        /// </summary>
        private JournalArticleModel model;

        /// <summary>
        /// Inicializuje objekt pro zobrazení dialogu.
        /// </summary>
        /// <param name="model">odpovídající objekt datové vrstvy</param>
        public JournalArticleDialog(JournalArticleModel model)
        {
            this.model = model;
        }

        /// <inheritDoc/>
        public void GetSpecificBibliography(Publication publication)
        {
            JournalArticle journalArticle = new JournalArticle();
            WriteLine("Časopis: " + journalArticle.JournalTitle);
            WriteLine("Číslo: " + journalArticle.Number);
            WriteLine(journalArticle.FromPage == journalArticle.ToPage ?
                (string.Format("Strana: {0}", journalArticle.FromPage)) :
                (string.Format("Strany: {0} - {1}", journalArticle.FromPage, journalArticle.ToPage)));
            WriteLine("ISBN: " + journalArticle.ISSN);
        }

        /// <inheritDoc/>
        public void CreateSpecificBibliography(Publication publication, List<Author> authors)
        {
            JournalArticle journalArticle = new JournalArticle();
            WriteLine("Zadejte řetězec obsahující název časopisu, případně místo vydání a nakladatele:");
            journalArticle.JournalTitle = ReadNonEmptyString("Název nesmí být prázdný.");
            WriteLine("Zadejte označení čísla časopisu:");
            journalArticle.Number = ReadNonEmptyString("Číslo nesmí být prázdné.");
            WriteLine("Citace od strany:");
            journalArticle.FromPage = ReadValidNumber("Zadejte číslo počáteční strany citace.");
            WriteLine("Citace do strany:");
            journalArticle.ToPage = ReadValidNumber("Zadejte číslo poslední strany citace.");
            WriteLine("Zadejte ISSN:");
            journalArticle.ISSN = ReadNonEmptyString("ISSN nesmí být prázdné.");

            // vytvoření záznamu z načtených informací
            model.CreatePublication(publication, authors, journalArticle);
        }

        /// <inheritDoc/>
        public void UpdateSpecificBibliography(int publicationId, Publication publication, List<Author> authors)
        {
            JournalArticle journalArticle = publication.JournalArticle;

            WriteLine("Zadejte nový název časopisu, případně místo vydání a nakladatele:");
            string journalTitle = ReadLine();

            if (!string.IsNullOrEmpty(journalTitle))
            {
                journalArticle.JournalTitle = journalTitle;
            }

            WriteLine("Zadejte nové označení čísla časopisu:");
            string number = ReadLine();

            if (!string.IsNullOrEmpty(number))
            {
                journalArticle.Number = number;
            }

            WriteLine("Nová počáteční strana citace:");
            int fromPage;
            if (int.TryParse(ReadLine(), out fromPage))
            {
                journalArticle.FromPage = fromPage;
            }

            WriteLine("Nová poslední strana citace:");
            int toPage;
            if (int.TryParse(ReadLine(), out toPage))
            {
                journalArticle.ToPage = toPage;
            }

            WriteLine("Zadejte nové ISSN:");
            string issn = ReadLine();

            if (!string.IsNullOrEmpty(issn))
            {
                journalArticle.ISSN = issn;
            }

            model.UpdatePublication(publicationId, publication, authors, journalArticle);
        }

        /// <inheritDoc/>
        public void DeleteSpecificBibliography(int publicationId)
        {
            model.DeletePublication(publicationId);
        }

        /// <inheritDoc/>
        public void PrintSpecificIsoCitation(Publication publication)
        {
            WriteLine(model.GeneratePublicationIsoCitation(publication));
        }

        /// <inheritDoc/>
        public void PrintSpecificBibtexEntry(Publication publication)
        {
            WriteLine(model.GeneratePublicationBibtexEntry(publication));
        }

        /// <inheritDoc/>
        public void PrintSpecificHtmlDocument(Publication publication)
        {
            WriteLine(model.ExportPublicationToHtmlDocument(publication));
        }
    }
}
