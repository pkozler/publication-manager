using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TechnicalReportManager
    {
        public TechnicalReport GetTechnicalReport(Publication publication)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                return context.TechnicalReport.Find(publication.TechnicalReport);
            }
        }

        public void AddTechnicalReport(Publication publication, TechnicalReport technicalReport)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                publication.TechnicalReport = technicalReport;
                technicalReport.Publication = publication;
                context.Publication.Add(publication);
                context.TechnicalReport.Add(technicalReport);
                context.SaveChanges();
            }
        }

        public void EditTechnicalReport(Publication publication, TechnicalReport technicalReport)
        {
            using (var context = new PublicationDatabaseEntities())
            {
                context.TechnicalReport.Remove(publication.TechnicalReport);
                context.Publication.Remove(publication);
                publication.TechnicalReport = technicalReport;
                technicalReport.Publication = publication;
                context.Publication.Add(publication);
                context.TechnicalReport.Add(technicalReport);
                context.SaveChanges();
            }
        }
    }
}
