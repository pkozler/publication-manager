using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class PublicationManager
    {
        public List<Publication> ListPublications(Author author, int? year, PublicationType type)
        {
            string t = type.ToString();

            using (var context = new PublicationDatabaseEntities())
            {
                var publications = from p in context.Publication
                                   where p.Author == author && p.Year == year && p.Type == t
                                   orderby p.Title
                                   select p;

                return publications.ToList();
            }
        }
        
        /// <summary>
        /// Nalezne publikaci podle zadaného ID.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace podle ID</returns>
        public Publication GetPublication(int id)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                return context.Publication.Find(id);
            }            
        }

        /// <summary>
        /// Odstraní publikaci podle zadaného ID.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void RemovePublication(int id)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                context.Publication.Remove(context.Publication.Find(id));
                context.SaveChanges();
            }
        }

        public string GeneratePublicationCitation(PublicationType type, int id)
        {
            throw new NotImplementedException();
        }

        public string GeneratePublicationBibtexEntry(PublicationType type, int id)
        {
            throw new NotImplementedException();
        }

        public string ExportPublicationToHtml(PublicationType type, int id)
        {
            throw new NotImplementedException();
        }
    }
}
