using System;
using System.Collections.Generic;
using System.IO;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída zobrazuje dialog pro zadání údajů o publikaci typu
    /// "technická zpráva".
    /// </summary>
    class TechnicalReportDialog : IPublishableDialog
    {
        /// <summary>
        /// Uchovává objekt datové vrstvy pro práci s příslušným typem publikace.
        /// </summary>
        private TechnicalReportModel model;

        /// <summary>
        /// Inicializuje objekt pro zobrazení dialogu.
        /// </summary>
        /// <param name="model">odpovídající objekt datové vrstvy</param>
        public TechnicalReportDialog(TechnicalReportModel model)
        {
            this.model = model;
        }

        /// <inheritDoc/>
        public APublicationModel GetModel()
        {
            return model as APublicationModel;
        }

        /// <inheritDoc/>
        public void GetSpecificBibliography(Publication publication)
        {
            TechnicalReport technicalReport = publication.TechnicalReport;
            WriteLine("Místo vydání: " + technicalReport.Address);
            WriteLine("Vydavatel: " + technicalReport.Institution);
            WriteLine("Označení: " + technicalReport.Number);
        }

        /// <inheritDoc/>
        public void CreateSpecificBibliography(Publication publication, List<Author> authors)
        {
            TechnicalReport technicalReport = new TechnicalReport();
            WriteLine("Zadejte místo vydání:");
            technicalReport.Address = ReadNonEmptyString("Adresa nesmí být prázdná.");
            WriteLine("Zadejte vydavatele:");
            technicalReport.Institution = ReadNonEmptyString("Název nesmí být prázdný.");
            WriteLine("Zadejte řetězec obsahující označení a číslo výzkumné zprávy:");
            technicalReport.Number = ReadNonEmptyString("označení nesmí být prázdné.");

            // vytvoření záznamu z načtených informací
            model.CreatePublication(publication, authors, technicalReport);
        }

        /// <inheritDoc/>
        public void UpdateSpecificBibliography(int publicationId, Publication publication, List<Author> authors)
        {
            TechnicalReport technicalReport = new TechnicalReport();

            WriteLine("Zadejte nové místo vydání:");
            string address = ReadLine();

            if (!string.IsNullOrEmpty(address))
            {
                technicalReport.Address = address;
            }

            WriteLine("Zadejte nového vydavatele:");
            string institution = ReadLine();

            if (!string.IsNullOrEmpty(institution))
            {
                technicalReport.Institution = institution;
            }

            WriteLine("Zadejte nové označení, případně číslo výzkumné zprávy:");
            string number = ReadLine();

            if (!string.IsNullOrEmpty(number))
            {
                technicalReport.Number = number;
            }

            model.UpdatePublication(publicationId, publication, authors, technicalReport);
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
        public void PrintSpecificHtmlDocument(Publication publication, string templatePath, string htmlPath)
        {
            string html = model.ExportPublicationToHtmlDocument(publication, templatePath, htmlPath);

            if (html != null)
            {
                WriteLine(html);
            }
        }
    }
}
