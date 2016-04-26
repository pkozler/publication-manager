using System;
using System.Collections.Generic;
using System.IO;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída představuje vstupní menu pro správu jednotlivých publikací.
    /// </summary>
    class PublicationMainMenu : AMenu
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        private PublicationModel publicationModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů autorů.
        /// </summary>
        private AuthorModel authorModel;

        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        private AttachmentModel attachmentModel;

        /// <summary>
        /// Seznam typů publikací, který slouží k uchování údajů specifických
        /// pro jednotlivé typy publikací a objektů datové vrstvy pro jejich obsluhu.
        /// </summary>
        private List<PublicationType> publicationTypes;

        /// <summary>
        /// Číslo, které představuje ID aktuálně zobrazené publikace.
        /// </summary>
        private int publicationId;

        /// <summary>
        /// Inicializuje menu správy publikace.
        /// </summary>
        /// <param name="publicationTypes">typy publikací</param>
        /// <param name="publicationModel">správce publikací</param>
        /// <param name="authorModel">správce autorů</param>
        /// <param name="attachmentModel">správce příloh</param>
        /// <param name="publicationId">ID publikace</param>
        public PublicationMainMenu(List<PublicationType> publicationTypes, PublicationModel publicationModel, AuthorModel authorModel, AttachmentModel attachmentModel, int publicationId)
        {
            this.publicationTypes = publicationTypes;
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationId = publicationId;

            MenuLabel = "Menu správy publikace č. " + publicationId;
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.R, new MenuItem() { Name = "Read", Description = "Znovu vypíše bibliografické údaje zobrazené publikace.", UIMethod = GetBibliography } },
                { ConsoleKey.U, new MenuItem() { Name = "Update", Description = "Spustí průvodce úpravou zobrazené publikace.", UIMethod = UpdateBibliography } },
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odstraní zobrazenou publikaci (vyžaduje potvrzení).", UIMethod = DeletePublication } },
                { ConsoleKey.T, new MenuItem() { Name = "Text", Description = "Vypíše obsah (hlavní text) zobrazené publikace.", UIMethod = PrintContentText } },
                { ConsoleKey.F, new MenuItem() { Name = "Files", Description = "Přepne do menu správy příloh (umožňuje přidávat a odebírat přílohy publikace).", UIMethod = UpdateAttachments } },
                { ConsoleKey.I, new MenuItem() { Name = "ISO", Description = "Vygeneruje citaci zobrazené publikace podle normy ČSN ISO 690.", UIMethod = PrintIsoCitation } },
                { ConsoleKey.B, new MenuItem() { Name = "BibTeX", Description = "Vygeneruje BibTeX záznam zobrazené publikace odpovídající citaci podle normy.", UIMethod = PrintBibtexEntry } },
                { ConsoleKey.E, new MenuItem() { Name = "Export", Description = "Exportuje zobrazenou publikaci do souboru ve formátu HTML.", UIMethod = PrintHtmlDocument } },
            });
            
            GetBibliography();
        }

        /// <summary>
        /// Přijme a vypíše bibliografické údaje o publikaci.
        /// </summary>
        public void GetBibliography()
        {
            Publication publication = publicationModel.GetPublicationById(publicationId);
            WriteLine("--- Výpis údajů publikace s ID {0} ---", publicationId);
            WriteLine("Klíč pro uložení BibTeX záznamu: " + publication.Entry);
            Write("Autoři: ");
            WriteAuthors(publication.Author);
            WriteLine("Název publikace: " + publication.Title);
            WriteLine("Rok vydání: " + publication.Year);
            PublicationType publicationType = PublicationType.GetTypeByName(
                publicationTypes, publication.Type);
            WriteLine("Typ publikace: " + publicationType.Description);
            publicationType.Dialog.GetSpecificBibliography(publication);
        }

        /// <summary>
        /// Načte od uživatele nové bibliografické údaje o publikaci a zažádá o úpravu údajů v záznamu.
        /// </summary>
        public void UpdateBibliography()
        {
            Publication publication = new Publication();
            WriteLine("Zadejte nový název BibTeX záznamu publikace:");
            string entry = ReadLine().Trim();

            if (!string.IsNullOrEmpty(entry))
            {
                publication.Entry = entry;
            }
            
            List<Author> authors = null;

            if (ReadYesNoAnswer("Chcete nastavit nový seznam autorů?"))
            {
                WriteLine("Zadejte křestní jméno nového autora NEBO "
                + "číslo představující ID již použitého autora:");
                authors = ReadAuthorList(authorModel,
                    "Zadejte křestní jméno dalšího nového autora NEBO "
                    + "číslo představující ID již použitého autora NEBO "
                    + "potvrďte hotový seznam autorů klávesou ENTER (tj. vložením prázdného řádku):",
                    "Zadejte příjmení nového autora:");
            }

            WriteLine("Zadejte nový skutečný název publikace:");
            string title = ReadLine().Trim();

            if (!string.IsNullOrEmpty(title))
            {
                publication.Title = title;
            }

            WriteLine("Zadejte nový rok vydání publikace:");

            int year;
            if (int.TryParse(ReadLine(), out year))
            {
                publication.Year = year;
            }
            
            if (ReadYesNoAnswer("Chcete upravit text publikace?"))
            {
                WriteLine("Zadejte cestu k textovému souboru publikace pro import textu "
                + "nebo prázdný řádek pro zadání z konzole: (pouze pro testovací účely)");

                string path = ReadLine().Trim();
                if (string.IsNullOrEmpty(path))
                {
                    WriteLine("Vložte text publikace: (\\n pro nový řádek)");
                    publication.Text = ReadLine().Replace("\\n", Environment.NewLine); ;
                }
                else
                {
                    using (var reader = new StreamReader(path))
                    {
                        publication.Text = reader.ReadToEnd();
                    }
                }
            }
            
            // předání načítání údajů dialogu pro zvolený typ publikace
            PublicationType.GetTypeByName(publicationTypes, publicationModel.GetPublicationById(publicationId).Type)
                .Dialog.UpdateSpecificBibliography(publicationId, publication, authors);
            WriteLine("Publikace s ID {0} byla úspěšně upravena.", publicationId);
        }

        /// <summary>
        /// Vypíše hlavní text publikace.
        /// </summary>
        public void PrintContentText()
        {
            Publication publication = publicationModel.GetPublicationById(publicationId);
            WriteLine(publication.Text);
        }
        
        /// <summary>
        /// Zobrazí menu správy seznamu příloh publikace.
        /// </summary>
        public void UpdateAttachments()
        {
            PublicationAttachmentMenu menu = new PublicationAttachmentMenu(
                publicationModel, attachmentModel, publicationId);
            menu.Start();
        }

        /// <summary>
        /// Požádá o vygenerování citace publikace podle ISO normy a tuto citaci vypíše.
        /// </summary>
        public void PrintIsoCitation()
        {
            Publication publication = publicationModel.GetPublicationById(publicationId);
            PublicationType publicationType = PublicationType.GetTypeByName(
                publicationTypes, publication.Type);
            publicationType.Dialog.PrintSpecificIsoCitation(publication);
        }

        /// <summary>
        /// Požádá o vygenerování BibTeX záznamu publikace a tento záznam vypíše.
        /// </summary>
        public void PrintBibtexEntry()
        {
            Publication publication = publicationModel.GetPublicationById(publicationId);
            PublicationType publicationType = PublicationType.GetTypeByName(
                publicationTypes, publication.Type);
            publicationType.Dialog.PrintSpecificBibtexEntry(publication);
        }

        /// <summary>
        /// Požádá o vytvoření HTML dokumentu pro umístění publikace na webové stránky.
        /// </summary>
        public void PrintHtmlDocument()
        {
            Publication publication = publicationModel.GetPublicationById(publicationId);
            PublicationType publicationType = PublicationType.GetTypeByName(
                publicationTypes, publication.Type);
            WriteLine("Zadejte cestu k souboru se šablonou pro vytvoření HTML dokumentu "
                + "nebo ponechte prázdný řádek pro použití výchozí šablony pro daný typ publikace:");
            string templatePath = ReadLine();
            WriteLine("Zadejte cestu k výstupnímu souboru exportovaného dokumentu ve formátu HTML "
                + "nebo ponechte prázdný řádek pro výpis HTML dokumentu na obrazovku:");
            string htmlPath = ReadLine();
            string template = File.ReadAllText(string.IsNullOrWhiteSpace(templatePath) ? 
                publicationType.Template : templatePath);
            publicationType.Dialog.PrintSpecificHtmlDocument(
                publication, publicationType.Description, template, htmlPath);
        }

        /// <summary>
        /// Po potvrzení požádá o odstranění publikace.
        /// </summary>
        public void DeletePublication()
        {
            if (!ReadYesNoAnswer("Opravdu chcete publikaci s ID {0} odstranit?"))
            {
                return;
            }

            Publication publication = publicationModel.GetPublicationById(publicationId);
            PublicationType publicationType = PublicationType.GetTypeByName(
                publicationTypes, publication.Type);
            publicationType.Dialog.DeleteSpecificBibliography(publicationId);
            WriteLine("Publikace s ID {0} byla úspěšně odstraněna.", publicationId);
            ExitMenu();
        }
    }
}
