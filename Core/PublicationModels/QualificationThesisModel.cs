using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "kvalifikační práce".
    /// </summary>
    public class QualificationThesisModel : APublicationModel
    {
        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        public QualificationThesis GetPublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = GetPublication(context, id);
                return publication.QualificationThesis;
            }
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="qualificationThesis">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, QualificationThesis qualificationThesis)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.QualificationThesis = qualificationThesis;
                qualificationThesis.Publication = publication;
                CreatePublication(context, publication);
                context.QualificationThesis.Add(qualificationThesis);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="qualificationThesis">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, QualificationThesis qualificationThesis)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                UpdatePublication(context, oldPublication, publication);
                QualificationThesis oldQualificationThesis = oldPublication.QualificationThesis;
                oldQualificationThesis.Address = qualificationThesis.Address;
                oldQualificationThesis.School = qualificationThesis.School;
                oldQualificationThesis.ThesisType = qualificationThesis.ThesisType;
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
                QualificationThesis oldQualificationThesis = oldPublication.QualificationThesis;
                context.QualificationThesis.Remove(oldQualificationThesis);
                DeletePublication(context, oldPublication);
                context.SaveChanges();
            }
        }
    }
}
