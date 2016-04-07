﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class AttachmentModel
    {
        public List<Attachment> ListAttachments(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                var attachments = from a in context.Attachment
                                  where a.PublicationId == id
                                  orderby a.Path
                                  select a;

                return attachments.ToList();
            }
        }
        
        public void AddAttachment(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
        
        public void RemoveAttachment(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                // TODO implementovat
                context.SaveChanges();
            }
        }
    }
}
