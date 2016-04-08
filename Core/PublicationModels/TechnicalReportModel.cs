using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "technická zpráva".
    /// </summary>
    public class TechnicalReportModel : APublicationModel
    {
        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        public TechnicalReport GetPublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = GetPublication(context, id);
                return publication.TechnicalReport;
            }
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="technicalReport">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, TechnicalReport technicalReport)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.TechnicalReport = technicalReport;
                technicalReport.Publication = publication;
                CreatePublication(context, publication);
                context.TechnicalReport.Add(technicalReport);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="technicalReport">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, TechnicalReport technicalReport)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                UpdatePublication(context, oldPublication, publication);
                TechnicalReport oldTechnicalReport = oldPublication.TechnicalReport;
                oldTechnicalReport.Address = technicalReport.Address;
                oldTechnicalReport.Institution = technicalReport.Institution;
                oldTechnicalReport.Number = technicalReport.Number;
                oldTechnicalReport.ReportType = technicalReport.ReportType;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                TechnicalReport oldTechnicalReport = oldPublication.TechnicalReport;
                context.TechnicalReport.Remove(oldTechnicalReport);
                DeletePublication(context, oldPublication);
                context.SaveChanges();
            }
        }
    }
}
