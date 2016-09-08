using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Core
{
    /// <summary>
    /// Třída představuje správce připojených příloh publikací.
    /// </summary>
    public class AttachmentModel
    {
        /// <summary>
        /// Uchovává databázový kontext.
        /// </summary>
        protected DbPublicationEntities context;

        /// <summary>
        /// Představuje název složky pro ukládání souborů připojených jako přílohy publikací.
        /// </summary>
        private const string DATA_ROOT_FOLDER_NAME = "attachments";
        
        /// <summary>
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        public AttachmentModel(DbPublicationEntities context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Určí vlastní ID nové přílohy v seznamu příloh dané publikace.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <returns>ID nové přílohy</returns>
        private int createNewAttachmentId(Publication publication)
        {
            // pro prázdný seznam příloh zvoleno ID 1
            if (publication.Attachment.Count < 1)
            {
                return 1;
            }

            // ID zvoleno jako o 1 větší než nejvyšší ID v seznamu
            return (publication.Attachment.Max(a => a.Id) + 1);
        }

        /// <summary>
        /// Sestaví plnou cestu k zadanému souboru uloženému jako příloha publikace.
        /// Připojený soubor je umístěn do podsložky s názvem ve tvaru "<ID publikace>-<ID přílohy>",
        /// která se nachází v kořenovém adresáři pro ukládání příloh publikací.
        /// Toto opatření umožňuje i v rámci jedné publikace připojit libovolný počet souborů
        /// se stejným názvem (a tento název zachovat).
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <param name="attachment">příloha</param>
        /// <returns>cesta k souboru</returns>
        private string getFullDataFolderPath(Publication publication, Attachment attachment)
        {
            // sestavení cesty včetně podsložky pojmenované podle ID publikace a ID přílohy
            return $"{DATA_ROOT_FOLDER_NAME}{Path.DirectorySeparatorChar}{publication.Id}-{attachment.Id}{Path.DirectorySeparatorChar}";
        }
        
        /// <summary>
        /// Vrátí seznam příloh publikace se zadaným ID.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <returns>seznam příloh</returns>
        public List<Attachment> GetAttachmentsByPublication(Publication publication)
        {
            var attachments = from a in context.Attachment
                                where a.PublicationId == publication.Id
                                orderby a.Id
                                select a;

            return attachments.ToList();
        }
        
        /// <summary>
        /// Přidá zadaný soubor do seznamu příloh publikace a zkopíruje ho do adresáře pro ukládání příloh.
        /// </summary>
        /// <param name="publication">publikace</param>
        /// <param name="srcFileName">cesta k připojenému souboru</param>
        public void AddAttachmentToPublication(Publication publication, string srcFileName)
        {
            // vytvoření záznamu přílohy s ID a jménem souboru
            Attachment attachment = new Attachment();
            attachment.Path = Path.GetFileName(srcFileName);
            attachment.Publication = publication;
            attachment.Id = createNewAttachmentId(attachment.Publication);

            // zkopírování zadaného zdrojového souboru do datové složky aplikace
            string fullDataPath = getFullDataFolderPath(publication, attachment);
            Directory.CreateDirectory(fullDataPath);
            File.Copy(srcFileName, getFullDataFolderPath(publication, attachment) + attachment.Path, true);

            // propojení záznamu s publikací a uložení změn
            publication.Attachment.Add(attachment);
            context.Attachment.Add(attachment);
            context.SaveChanges();
        }

        /// <summary>
        /// "Stáhne" (zkopíruje) vybraný soubor, který je přílohou publikace, na zadanou cestu.
        /// </summary>
        /// <param name="publication">publikace s přílohou</param>
        /// <param name="destFileName">cesta pro zkopírování souboru</param>
        /// <param name="attachmentId">ID přílohy v seznamu příloh dané publikace</param>
        public void CopyAttachmentOfPublication(Publication publication, string destFileName, int attachmentId)
        {
            Attachment attachment = context.Attachment.Find(publication.Id, attachmentId);

            if (attachment == null)
            {
                throw new AttachmentException(string.Format(
                    "Příloha s ID {0} u publikace s ID {1} neexistuje.", attachmentId, publication.Id));
            }

            // zkopírování souboru přílohy v datové složce aplikace do zadaného cílového souboru
            File.Copy(getFullDataFolderPath(publication, attachment) + attachment.Path, destFileName, true);
        }

        /// <summary>
        /// Odebere vybraný soubor ze seznamu příloh publikace a odstraní ho z adresáře pro ukládání příloh.
        /// </summary>
        /// <param name="publication">publikace s přílohou</param>
        /// <param name="attachmentId">ID přílohy v seznamu příloh dané publikace</param>
        public void RemoveAttachmentFromPublication(Publication publication, int attachmentId)
        {
            Attachment attachment = context.Attachment.Find(publication.Id, attachmentId);
            
            if (attachment == null)
            {
                throw new AttachmentException(string.Format(
                    "Příloha s ID {0} u publikace s ID {1} neexistuje.", attachmentId, publication.Id));
            }

            // odstranění souboru přílohy z datové složky aplikace
            string fullDataPath = getFullDataFolderPath(publication, attachment);
            File.Delete(getFullDataFolderPath(publication, attachment) + attachment.Path);
            Directory.Delete(fullDataPath);

            // odstranění záznamu o příloze a uložení změn
            publication.Attachment.Remove(attachment);
            context.Attachment.Remove(attachment);
            context.SaveChanges();
        }
    }
}
