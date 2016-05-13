using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

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

        private const string DATA_ROOT_FOLDER_NAME = "data";

        private readonly string DataRootFolderPath;

        /// <summary>
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        public AttachmentModel(DbPublicationEntities context)
        {
            this.context = context;

            DataRootFolderPath = $"{Path.GetFullPath(".")}{Path.DirectorySeparatorChar}{DATA_ROOT_FOLDER_NAME}{Path.DirectorySeparatorChar}";
        }

        private int createNewAttachmentId(Publication publication)
        {
            if (publication.Attachment.Count < 1)
            {
                return 1;
            }
            
            return (publication.Attachment.Max(a => a.Id) + 1);
        }

        private string getFullDataFolderPath(Publication publication, Attachment attachment)
        {
            return $"{DataRootFolderPath}{publication.Id}-{attachment.Id}{Path.DirectorySeparatorChar}";
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
        /// Přidá k publikaci novou přílohu.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="attachment">údaje o příloze</param>
        public void AddAttachmentToPublication(Publication publication, string srcFileName)
        {
            Attachment attachment = new Attachment();
            attachment.Path = Path.GetFileName(srcFileName);
            
            attachment.Publication = publication;
            attachment.Id = createNewAttachmentId(attachment.Publication);

            string fullDataPath = getFullDataFolderPath(publication, attachment);
            Directory.CreateDirectory(fullDataPath);
            File.Copy(srcFileName, getFullDataFolderPath(publication, attachment) + attachment.Path);

            publication.Attachment.Add(attachment);
            context.Attachment.Add(attachment);
            context.SaveChanges();
        }

        /// <summary>
        /// Zkopíruje existující přílohu publikace.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="id">ID přílohy</param>
        public void CopyAttachmentOfPublication(Publication publication, string destFileName, int id)
        {
            Attachment attachment = context.Attachment.Find(publication.Id, id);

            if (attachment == null)
            {
                throw new AttachmentException(string.Format(
                    "Příloha s ID {0} u publikace s ID {1} neexistuje.", id, publication.Id));
            }

            File.Copy(getFullDataFolderPath(publication, attachment) + attachment.Path, destFileName);
        }

        /// <summary>
        /// Odebere z publikace existující přílohu.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="id">ID přílohy</param>
        public void RemoveAttachmentFromPublication(Publication publication, int id)
        {
            Attachment attachment = context.Attachment.Find(publication.Id, id);
            
            if (attachment == null)
            {
                throw new AttachmentException(string.Format(
                    "Příloha s ID {0} u publikace s ID {1} neexistuje.", id, publication.Id));
            }

            string fullDataPath = getFullDataFolderPath(publication, attachment);
            File.Delete(getFullDataFolderPath(publication, attachment) + attachment.Path);
            Directory.Delete(fullDataPath);

            publication.Attachment.Remove(attachment);
            context.Attachment.Remove(attachment);
            context.SaveChanges();
        }
    }
}
