using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CLI
{
    /// <summary>
    /// Třída představuje vstupní menu pro správu jednotlivých publikací.
    /// </summary>
    class MainPublicationMenu : AMenu
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
        /// Číslo, které představuje ID aktuálně spravované publikace.
        /// </summary>
        private int publicationId;

        /// <summary>
        /// Inicializuje menu správy publikace.
        /// </summary>
        public MainPublicationMenu(PublicationModel publicationModel, AuthorModel authorModel, AttachmentModel attachmentModel, int publicationId) : base()
        {
            this.publicationModel = publicationModel;
            this.authorModel = authorModel;
            this.attachmentModel = attachmentModel;
            this.publicationId = publicationId;

            MenuLabel = "Menu správy publikace č. " + publicationId;
            MenuItems = new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.R, new MenuItem() { Name = "Read", Description = "Znovu vypíše bibliografické údaje zobrazené publikace.", UIMethod = GetBibliography } },
                { ConsoleKey.U, new MenuItem() { Name = "Update", Description = "Spustí průvodce úpravou bibliografických údajů zobrazené publikace.", UIMethod = UpdateBibliography } },
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odstraní zobrazenou publikaci (vyžaduje potvrzení).", UIMethod = UpdateContentText } },
                { ConsoleKey.T, new MenuItem() { Name = "Text", Description = "Vypíše obsah (hlavní text) zobrazené publikace a umožní jeho editaci.", UIMethod = UpdateAuthors } },
                { ConsoleKey.A, new MenuItem() { Name = "Authors", Description = "Přepne do menu správy autorů (umožňuje přidávat a odebírat autory publikace).", UIMethod = UpdateAttachments } },
                { ConsoleKey.F, new MenuItem() { Name = "Files", Description = "Přepne do menu správy příloh (umožňuje přidávat a odebírat přílohy publikace).", UIMethod = GenerateIsoCitation } },
                { ConsoleKey.I, new MenuItem() { Name = "ISO", Description = "Vygeneruje citaci zobrazené publikace podle normy ČSN ISO 690.", UIMethod = GenerateBibtexRecord } },
                { ConsoleKey.B, new MenuItem() { Name = "BibTeX", Description = "Vygeneruje BibTeX záznam zobrazené publikace odpovídající citaci podle normy.", UIMethod = ExportToHtml } },
                { ConsoleKey.E, new MenuItem() { Name = "Export", Description = "Exportuje zobrazenou publikaci do souboru ve formátu HTML.", UIMethod = DeletePublication } },
            };
        }

        /// <summary>
        /// Přijme a vypíše bibliografické údaje o publikaci.
        /// </summary>
        public void GetBibliography()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Načte od uživatele nové bibliografické údaje o publikaci a zažádá o úpravu údajů v záznamu.
        /// </summary>
        public void UpdateBibliography()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zobrazí menu úpravy hlavního textu publikace.
        /// </summary>
        public void UpdateContentText()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zobrazí menu správy seznamu autorů publikace.
        /// </summary>
        public void UpdateAuthors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zobrazí menu správy seznamu příloh publikace.
        /// </summary>
        public void UpdateAttachments()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Požádá o vygenerování citace publikace podle ISO normy a tuto citaci vypíše.
        /// </summary>
        public void GenerateIsoCitation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Požádá o vygenerování BibTeX záznamu publikace a tento záznam vypíše.
        /// </summary>
        public void GenerateBibtexRecord()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Požádá o vytvoření HTML dokumentu pro umístění publikace na webové stránky.
        /// </summary>
        public void ExportToHtml()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Po potvrzení požádá o odstranění publikace.
        /// </summary>
        public void DeletePublication()
        {
            throw new NotImplementedException();
        }
    }
}
