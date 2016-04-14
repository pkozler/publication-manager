using System.Collections.Generic;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída zobrazuje dialog pro zadání údajů o publikaci typu
    /// "kvalifikační práce".
    /// </summary>
    class QualificationThesisDialog : IPublishableDialog
    {
        /// <summary>
        /// Uchovává objekt datové vrstvy pro práci s příslušným typem publikace.
        /// </summary>
        private QualificationThesisModel model;

        /// <summary>
        /// Inicializuje objekt pro zobrazení dialogu.
        /// </summary>
        /// <param name="model">odpovídající objekt datové vrstvy</param>
        public QualificationThesisDialog(QualificationThesisModel model)
        {
            this.model = model;
        }

        /// <inheritDoc/>
        public void GetSpecificBibliography(Publication publication)
        {
            QualificationThesis qualificationThesis = new QualificationThesis();
            WriteLine("Místo vytvoření: " + qualificationThesis.Address);
            WriteLine("Název školy: " + qualificationThesis.School);
            WriteLine("Typ práce: " + 
                qualificationThesis.ThesisType == QualificationThesisModel.TYPE_PHD_THESIS ? 
                "disertační práce" : "diplomová práce");
        }

        /// <inheritDoc/>
        public void CreateSpecificBibliography(Publication publication, List<Author> authors)
        {
            QualificationThesis qualificationThesis = new QualificationThesis();
            WriteLine("Zadejte místo vytvoření:");
            qualificationThesis.Address = ReadNonEmptyString("Adresa nesmí být prázdná.");
            WriteLine("Zadejte název školy:");
            qualificationThesis.School = ReadNonEmptyString("Název nesmí být prázdný.");
            WriteLine("Zadejte číslo představující typ kvalifikační práce: (0 - diplomová práce / 1 - disertační práce)");
            int typeNumber = ReadValidNumber("Zadejte platné číslo typu.");
            qualificationThesis.ThesisType = typeNumber == 0 ?
                QualificationThesisModel.TYPE_MASTER_THESIS :
                QualificationThesisModel.TYPE_PHD_THESIS;

            // vytvoření záznamu z načtených informací
            model.CreatePublication(publication, authors[0], qualificationThesis);
        }

        /// <inheritDoc/>
        public void UpdateSpecificBibliography(int publicationId, Publication publication, List<Author> authors)
        {
            QualificationThesis qualificationThesis = publication.QualificationThesis;

            WriteLine("Zadejte nové místo vytvoření:");
            string address = ReadLine();

            if (!string.IsNullOrEmpty(address))
            {
                qualificationThesis.Address = address;
            }

            WriteLine("Zadejte nový název školy:");
            string school = ReadLine();

            if (!string.IsNullOrEmpty(school))
            {
                qualificationThesis.School = school;
            }

            WriteLine("Zadejte číslo představující nový typ kvalifikační práce: (0 - diplomová práce / 1 - disertační práce):");

            int typeNumber;
            if (int.TryParse(ReadLine(), out typeNumber))
            {
                qualificationThesis.ThesisType = typeNumber == 0 ?
                QualificationThesisModel.TYPE_MASTER_THESIS :
                QualificationThesisModel.TYPE_PHD_THESIS;
            }

            model.UpdatePublication(publicationId, publication, authors[0], qualificationThesis);
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
