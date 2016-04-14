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
        /// Vrátí seznam příloh publikace se zadaným ID.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <returns>seznam příloh</returns>
        public List<Attachment> GetAttachmentsByPublication(Publication publication)
        {
            using (var context = new DbPublicationEntities())
            {
                var attachments = from a in context.Attachment
                                  where a.PublicationId == publication.Id
                                  orderby a.Path
                                  select a;

                return attachments.ToList();
            }
        }
        
        /// <summary>
        /// Přidá k publikaci novou přílohu.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="attachment">údaje o příloze</param>
        public void AddAttachmentToPublication(Publication publication, Attachment attachment)
        {
            using (var context = new DbPublicationEntities())
            {
                attachment.Publication = publication;
                publication.Attachment.Add(attachment);
                context.Attachment.Add(attachment);
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Odebere z publikace existující přílohu.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="id">ID přílohy</param>
        public void RemoveAttachmentFromPublication(Publication publication, int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Attachment attachment = context.Attachment.Find(publication.Id, id);
                publication.Attachment.Remove(attachment);
                context.Attachment.Remove(attachment);
                context.SaveChanges();
            }
        }
    }
}
