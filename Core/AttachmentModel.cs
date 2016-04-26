using System.Collections.Generic;
using System.Linq;

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
        /// Vytvoří instanci správce.
        /// </summary>
        /// <param name="context">databázový kontext</param>
        public AttachmentModel(DbPublicationEntities context)
        {
            this.context = context;
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
                                orderby a.Path
                                select a;

            List<Attachment> attachmentList = attachments.ToList();
            attachmentList.Sort(new AttachmentComparer());

            return attachmentList;
        }
        
        /// <summary>
        /// Přidá k publikaci novou přílohu.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="attachment">údaje o příloze</param>
        public void AddAttachmentToPublication(Publication publication, Attachment attachment)
        {
            attachment.Publication = publication;
            List<Attachment> attachmentList = GetAttachmentsByPublication(publication);
            // určení ID nové přílohy
            attachment.Id = attachmentList.Count > 0 ? attachmentList.Last().Id + 1 : 1;
            publication.Attachment.Add(attachment);
            context.Attachment.Add(attachment);
            context.SaveChanges();
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
                    "Příloha s id {0} u publikace s id {1} neexistuje.", id, publication.Id));
            }

            publication.Attachment.Remove(attachment);
            context.Attachment.Remove(attachment);
            context.SaveChanges();
        }
    }
}
