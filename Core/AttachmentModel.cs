using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Attachment> GetAttachmentsByPublication(int publicationId)
        {
            using (var context = new DbPublicationEntities())
            {
                var attachments = from a in context.Attachment
                                  where a.PublicationId == publicationId
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
        public void AddAttachmentToPublication(int publicationId, Attachment attachment)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Odebere z publikace existující přílohu.
        /// </summary>
        /// <param name="publicationId">ID publikace</param>
        /// <param name="id">ID přílohy</param>
        public void RemoveAttachmentFromPublication(int publicationId, int id)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
    }
}
