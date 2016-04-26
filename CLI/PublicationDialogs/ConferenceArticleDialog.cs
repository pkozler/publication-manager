using System.Collections.Generic;
using System.IO;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída zobrazuje dialog pro zadání údajů o publikaci typu
    /// "článek na konferenci".
    /// </summary>
    class ConferenceArticleDialog : IPublishableDialog
    {
        /// <summary>
        /// Uchovává objekt datové vrstvy pro práci s příslušným typem publikace.
        /// </summary>
        private ConferenceArticleModel model;
        
        /// <summary>
        /// Inicializuje objekt pro zobrazení dialogu.
        /// </summary>
        /// <param name="model">odpovídající objekt datové vrstvy</param>
        public ConferenceArticleDialog(ConferenceArticleModel model)
        {
            this.model = model;
        }

        /// <inheritDoc/>
        public void GetSpecificBibliography(Publication publication)
        {
            ConferenceArticle conferenceArticle = publication.ConferenceArticle;
            WriteLine("Zadejte název sborníku konference: " + conferenceArticle.BookTitle);
            WriteLine("Místo vydání: " + conferenceArticle.Address);
            WriteLine("Nakladatel: " + conferenceArticle.Publisher);
            WriteLine(conferenceArticle.FromPage == conferenceArticle.ToPage ? 
                (string.Format("Strana: {0}", conferenceArticle.FromPage)) : 
                (string.Format("Strany: {0} - {1}", conferenceArticle.FromPage, conferenceArticle.ToPage)));
            WriteLine(!string.IsNullOrEmpty(conferenceArticle.ISBN) ? 
                ("ISBN: " + conferenceArticle.ISBN) : 
                ("ISSN: " + conferenceArticle.ISSN));
        }

        /// <inheritDoc/>
        public void CreateSpecificBibliography(Publication publication, List<Author> authors)
        {
            ConferenceArticle conferenceArticle = new ConferenceArticle();
            WriteLine("Zadejte název sborníku konference:");
            conferenceArticle.BookTitle = ReadNonEmptyString("Název sborníku nesmí být prázdný.");
            WriteLine("Zadejte místo vydání:");
            conferenceArticle.Address = ReadNonEmptyString("Adresa nesmí být prázdná.");
            WriteLine("Zadejte nakladatele:");
            conferenceArticle.Publisher = ReadNonEmptyString("Název vydavatele nesmí být prázdný.");
            WriteLine("Citace od strany:");
            conferenceArticle.FromPage = ReadValidNumber("Zadejte číslo počáteční strany citace.");
            WriteLine("Citace do strany:");

            int toPage = ReadValidNumber("Zadejte číslo poslední strany citace.");
            while (toPage < conferenceArticle.FromPage)
            {
                WriteLine("Poslední číslo nesmí být menší než počáteční.");
                toPage = ReadValidNumber("Zadejte číslo poslední strany citace.");
            }
            conferenceArticle.ToPage = toPage;

            WriteLine("Zadejte ISBN nebo ponechte prázdný řádek pro zadání ISSN:");
            string isbn = ReadLine();

            if (!string.IsNullOrEmpty(isbn))
            {
                conferenceArticle.ISBN = isbn;
            }
            else
            {
                WriteLine("Zadejte ISSN:");
                conferenceArticle.ISSN = ReadNonEmptyString("ISSN nesmí být prázdné.");
            }

            // vytvoření záznamu z načtených informací
            model.CreatePublication(publication, authors, conferenceArticle);
        }

        /// <inheritDoc/>
        public void UpdateSpecificBibliography(int publicationId, Publication publication, List<Author> authors)
        {
            ConferenceArticle conferenceArticle = new ConferenceArticle();
            WriteLine("Zadejte nový název sborníku konference:");
            string bookTitle = ReadLine().Trim();

            if (!string.IsNullOrEmpty(bookTitle))
            {
                conferenceArticle.BookTitle = bookTitle;
            }

            WriteLine("Zadejte nové místo vydání:");
            string address = ReadLine().Trim();

            if (!string.IsNullOrEmpty(address))
            {
                conferenceArticle.Address = address;
            }

            WriteLine("Zadejte nového nakladatele:");
            string publisher = ReadLine().Trim();

            if (!string.IsNullOrEmpty(publisher))
            {
                conferenceArticle.Publisher = publisher;
            }

            WriteLine("Nová počáteční strana citace:");
            int fromPage;
            if (int.TryParse(ReadLine(), out fromPage))
            {
                conferenceArticle.FromPage = fromPage;
            }

            WriteLine("Nová poslední strana citace:");
            int toPage;
            if (int.TryParse(ReadLine(), out toPage) && toPage >= conferenceArticle.FromPage)
            {
                conferenceArticle.ToPage = toPage;
            }

            WriteLine("Zadejte nové ISBN nebo ponechte prázdný řádek pro zadání ISSN:");
            string isbn = ReadLine();

            if (!string.IsNullOrEmpty(isbn))
            {
                conferenceArticle.ISBN = isbn;
            }
            else
            {
                WriteLine("Zadejte nové ISSN:");
                string issn = ReadLine();

                if (!string.IsNullOrEmpty(issn))
                {
                    conferenceArticle.ISSN = issn;
                }
            }

            model.UpdatePublication(publicationId, publication, authors, conferenceArticle);
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
        public void PrintSpecificHtmlDocument(Publication publication, string typeDescription, string template, string htmlPath)
        {
            string html = model.ExportPublicationToHtmlDocument(publication, typeDescription, template);

            if (string.IsNullOrWhiteSpace(htmlPath))
            {
                WriteLine(html);
            }
            else
            {
                File.WriteAllText(htmlPath, html);
            }
        }
    }
}
