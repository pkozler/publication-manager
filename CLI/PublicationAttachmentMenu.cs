using System;
using System.Collections.Generic;
using Core;

using static System.Console;
using static CLI.ConsoleExtension;

namespace CLI
{
    /// <summary>
    /// Třída představuje menu pro správu seznamu příloh jednotlivých publikací.
    /// </summary>
    class PublicationAttachmentMenu : AMenu
    {
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů publikací.
        /// </summary>
        private PublicationModel publicationModel;
        
        /// <summary>
        /// Objekt datové vrstvy, který slouží jako správce záznamů příloh.
        /// </summary>
        private AttachmentModel attachmentModel;

        /// <summary>
        /// Číslo, které představuje ID aktuálně spravované publikace.
        /// </summary>
        private int publicationId;

        /// <summary>
        /// Inicializuje menu správy seznamu příloh publikace.
        /// </summary>
        /// <param name="publicationModel">správce publikací</param>
        /// <param name="attachmentModel">správce příloh</param>
        public PublicationAttachmentMenu(PublicationModel publicationModel, AttachmentModel attachmentModel, int publicationId)
        {
            this.publicationModel = publicationModel;
            this.attachmentModel = attachmentModel;
            this.publicationId = publicationId;

            MenuLabel = "Menu správy příloh publikace č. " + publicationId;
            InitializeMenuItems(new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.C, new MenuItem() { Name = "Create", Description = "Přidá nový soubor se zadanou cestou do seznamu příloh publikace.", UIMethod = CreateAttachment } },
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odebere existující soubor se zadaným ID ze seznamu příloh publikace.", UIMethod = DeleteAttachment } },
            });

            GetAttachmentList();
        }

        /// <summary>
        /// Načte od uživatele údaje o nové příloze a požádá o její přidání do seznamu.
        /// </summary>
        public void CreateAttachment()
        {
            WriteLine("Zadejte cestu k souboru přílohy:");
            string path = ReadNonEmptyString("Cesta nesmí být prázdná.");
            Attachment attachment = new Attachment();
            attachment.Path = path;
            attachmentModel.AddAttachmentToPublication(
                publicationModel.GetPublicationById(publicationId), attachment);
        }

        /// <summary>
        /// Načte od uživatele ID přílohy a požádá o její odebrání ze seznamu.
        /// </summary>
        public void DeleteAttachment()
        {
            WriteLine("Zadejte ID přílohy k odstranění:");
            int id = ReadValidNumber("Zadejte celé číslo představující ID přílohy aktuální publikace.");
            
            if (ReadYesNoAnswer("Opravdu chcete přílohu odstranit?"))
            {
                attachmentModel.RemoveAttachmentFromPublication(
                    publicationModel.GetPublicationById(publicationId), id);
            }

            GetAttachmentList();
        }

        /// <summary>
        /// Zobrazí seznam příloh dané publikace.
        /// </summary>
        public void GetAttachmentList()
        {
            WriteLine("Seznam příloh:");
            WriteLine("ID\tCesta k souboru");
            List<Attachment> attachments = new List<Attachment>();

            foreach (Attachment attachment in attachments)
            {
                WriteLine($"{attachment.Id}\t{attachment.Path}");
            }
        }
    }
}
