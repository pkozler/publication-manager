using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Core
{
    /// <summary>
    /// Třída představuje správce základních bibliografických údajů společných
    /// pro všechny typy publikací (zahrnují BibTeX klíč, název, rok a typ publikace).
    /// </summary>
    public class PublicationModel
    {
        public PublicationModel()
        {
            /*using (var context = new DbPublicationEntities())
            {
                Publication p = new Publication();
                p.Entry = "aaa";
                p.Title = "bbb";
                p.Year = 1111;
                p.Text = "ABCDEF";
                p.Type = QualificationThesisModel.NAME;

                QualificationThesis q = new QualificationThesis();
                q.Address = "ddd";
                q.School = "eee";
                q.ThesisType = QualificationThesisModel.TYPE_MASTER_THESIS;
                
                Debug.WriteLine("=== VÝPIS PUBLIKACE ===");
                Debug.WriteLine(p.Id);
                Debug.WriteLine(p.Entry);
                Debug.WriteLine(p.Title);
                Debug.WriteLine(p.Year);
                Debug.WriteLine(p.Text);
                Debug.WriteLine(p.Type);
                Debug.WriteLine(q.PublicationId);
                Debug.WriteLine(q.Address);
                Debug.WriteLine(q.School);
                Debug.WriteLine(q.ThesisType);

                p.QualificationThesis = q;
                q.Publication = p;

                context.Publication.Add(p);
                context.QualificationThesis.Add(q);
                context.SaveChanges();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication p = new Publication();
                p.Entry = "aaa";
                p.Title = "bbb";
                p.Year = 1111;
                p.Text = "ABCDEF";
                p.Type = TechnicalReportModel.NAME;

                TechnicalReport t = new TechnicalReport();
                t.Address = "ccc";
                t.Institution = "ddd";
                t.Number = "eee";
                t.ReportType = "?";

                Debug.WriteLine("=== VÝPIS PUBLIKACE ===");
                Debug.WriteLine(p.Id);
                Debug.WriteLine(p.Entry);
                Debug.WriteLine(p.Title);
                Debug.WriteLine(p.Year);
                Debug.WriteLine(p.Text);
                Debug.WriteLine(p.Type);
                Debug.WriteLine(t.PublicationId);
                Debug.WriteLine(t.Address);
                Debug.WriteLine(t.Institution);
                Debug.WriteLine(t.ReportType);

                p.TechnicalReport = t;
                t.Publication = p;

                context.Publication.Add(p);
                context.TechnicalReport.Add(t);
                context.SaveChanges();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication p = new Publication();
                p.Entry = "aaa";
                p.Title = "bbb";
                p.Year = 1111;
                p.Text = "ABCDEF";
                p.Type = JournalArticleModel.NAME;

                JournalArticle j = new JournalArticle();
                j.JournalTitle = "ccc";
                j.Number = "ddd";
                j.FromPage = 1;
                j.ToPage = 2;
                j.ISSN = "123456";

                Debug.WriteLine("=== VÝPIS PUBLIKACE ===");
                Debug.WriteLine(p.Id);
                Debug.WriteLine(p.Entry);
                Debug.WriteLine(p.Title);
                Debug.WriteLine(p.Year);
                Debug.WriteLine(p.Text);
                Debug.WriteLine(p.Type);
                Debug.WriteLine(j.PublicationId);
                Debug.WriteLine(j.JournalTitle);
                Debug.WriteLine(j.Number);
                Debug.WriteLine(j.FromPage);
                Debug.WriteLine(j.ToPage);
                Debug.WriteLine(j.ISSN);

                p.JournalArticle = j;
                j.Publication = p;

                context.Publication.Add(p);
                context.JournalArticle.Add(j);
                context.SaveChanges();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication p = new Publication();
                p.Entry = "aaa";
                p.Title = "bbb";
                p.Year = 1111;
                p.Text = "ABCDEF";
                p.Type = ConferenceArticleModel.NAME;

                ConferenceArticle c = new ConferenceArticle();
                c.BookTitle = "ccc";
                c.Address = "ddd";
                c.Publisher = "eee";
                c.FromPage = 1;
                c.ToPage = 2;
                //c.ISBN = "123456";
                c.ISSN = "789000";

                Debug.WriteLine("=== VÝPIS PUBLIKACE ===");
                Debug.WriteLine(p.Id);
                Debug.WriteLine(p.Entry);
                Debug.WriteLine(p.Title);
                Debug.WriteLine(p.Year);
                Debug.WriteLine(p.Text);
                Debug.WriteLine(p.Type);
                Debug.WriteLine(c.PublicationId);
                Debug.WriteLine(c.BookTitle);
                Debug.WriteLine(c.Address);
                Debug.WriteLine(c.Publisher);
                Debug.WriteLine(c.FromPage);
                Debug.WriteLine(c.ToPage);
                //Debug.WriteLine(c.ISBN);
                Debug.WriteLine(c.ISSN);

                p.ConferenceArticle = c;
                c.Publication = p;

                context.Publication.Add(p);
                context.ConferenceArticle.Add(c);
                context.SaveChanges();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication publication = context.Publication.Find(1);
                QualificationThesis thesis = publication.QualificationThesis;

                Debug.WriteLine("=== VÝPIS PO ULOŽENÍ ===");
                Debug.WriteLine(publication.Id);
                Debug.WriteLine(publication.Entry);
                Debug.WriteLine(publication.Title);
                Debug.WriteLine(publication.Year);
                Debug.WriteLine(publication.Text);
                Debug.WriteLine(publication.Type);
                Debug.WriteLine(thesis.PublicationId);
                Debug.WriteLine(thesis.Address);
                Debug.WriteLine(thesis.School);
                Debug.WriteLine(thesis.ThesisType);

                Debug.Flush();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication publication = context.Publication.Find(1);
                TechnicalReport report = publication.TechnicalReport;

                Debug.WriteLine("=== VÝPIS PO ULOŽENÍ ===");
                Debug.WriteLine(publication.Id);
                Debug.WriteLine(publication.Entry);
                Debug.WriteLine(publication.Title);
                Debug.WriteLine(publication.Year);
                Debug.WriteLine(publication.Text);
                Debug.WriteLine(publication.Type);
                Debug.WriteLine(report.PublicationId);
                Debug.WriteLine(report.Address);
                Debug.WriteLine(report.Institution);
                Debug.WriteLine(report.Number);
                Debug.WriteLine(report.ReportType);

                Debug.Flush();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication publication = context.Publication.Find(1);
                JournalArticle article = publication.JournalArticle;

                Debug.WriteLine("=== VÝPIS PO ULOŽENÍ ===");
                Debug.WriteLine(publication.Id);
                Debug.WriteLine(publication.Entry);
                Debug.WriteLine(publication.Title);
                Debug.WriteLine(publication.Year);
                Debug.WriteLine(publication.Text);
                Debug.WriteLine(publication.Type);
                Debug.WriteLine(article.PublicationId);
                Debug.WriteLine(article.JournalTitle);
                Debug.WriteLine(article.Number);
                Debug.WriteLine(article.FromPage);
                Debug.WriteLine(article.ToPage);
                Debug.WriteLine(article.ISSN);

                Debug.Flush();
            }*/

            /*using (var context = new DbPublicationEntities())
            {
                Publication publication = context.Publication.Find(1);
                ConferenceArticle conference = publication.ConferenceArticle;

                Debug.WriteLine("=== VÝPIS PO ULOŽENÍ ===");
                Debug.WriteLine(publication.Id);
                Debug.WriteLine(publication.Entry);
                Debug.WriteLine(publication.Title);
                Debug.WriteLine(publication.Year);
                Debug.WriteLine(publication.Text);
                Debug.WriteLine(publication.Type);
                Debug.WriteLine(conference.PublicationId);
                Debug.WriteLine(conference.BookTitle);
                Debug.WriteLine(conference.Address);
                Debug.WriteLine(conference.Publisher);
                Debug.WriteLine(conference.FromPage);
                Debug.WriteLine(conference.ToPage);
                Debug.WriteLine(conference.ISBN);
                Debug.WriteLine(conference.ISSN);

                Debug.Flush();
            }*/
        }

        /// <summary>
        /// Vrátí seznam evidovaných publikací filtrovaný podle předaných množin údajů
        /// (tedy vrátí pouze publikace s údaji, které jsou obsaženy v příslušných množinách).
        /// </summary>
        /// <param name="authorFilter">množina požadovaných autorů</param>
        /// <param name="yearFilter">množina požadovaných roků vydání</param>
        /// <param name="publicationTypeFilter">množina požadovaných typů publikací</param>
        /// <returns>filtrovaný seznam publikací</returns>
        public List<Publication> GetPublications(
            HashSet<int> authorFilter, HashSet<int> yearFilter, HashSet<string> publicationTypeFilter)
        {
            using (var context = new DbPublicationEntities())
            {
                // výběr autorů ze seznamu evidovaných podle ID autorů ze zadané množiny
                var authors = from a in context.Author
                              where authorFilter.Contains(a.Id)
                              select a;
                
                // výběr publikací s filtrováním podle množin (pokud je některá množina prázdná, příslušná položka se nefiltruje)
                var publications = from p in context.Publication
                                   where ((authors.Count() == 0) ? true : authors.Intersect(p.Author).Any())
                                   && ((yearFilter.Count() == 0) ? true : yearFilter.Contains(p.Year))
                                   && ((publicationTypeFilter.Count()) == 0 ? true : publicationTypeFilter.Contains(p.Type))
                                   orderby p.Title
                                   select p;

                return publications.ToList();
            }
        }
        
        /// <summary>
        /// Načte základní údaje uložené publikace se zadaným ID.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace podle ID</returns>
        public Publication GetPublicationById(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = context.Publication.Find(id);

                if (publication == null)
                {
                    throw new PublicationException(string.Format("Publikace s id {0} neexistuje.", id));
                }

                return publication;
            }
        }
    }
}
