using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class QualificationThesisModel
    {
        public QualificationThesis GetQualificationThesis(Publication publication)
        {
            using (var context = new DbPublicationEntities())
            {
                return context.QualificationThesis.Find(publication.QualificationThesis);
            }
        }

        public void AddQualificationThesis(Publication publication, QualificationThesis qualificationThesis)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.QualificationThesis = qualificationThesis;
                qualificationThesis.Publication = publication;
                context.Publication.Add(publication);
                context.QualificationThesis.Add(qualificationThesis);
                context.SaveChanges();
            }
        }

        public void EditQualificationThesis(Publication publication, QualificationThesis qualificationThesis)
        {
            using (var context = new DbPublicationEntities())
            {
                context.QualificationThesis.Remove(publication.QualificationThesis);
                context.Publication.Remove(publication);
                publication.QualificationThesis = qualificationThesis;
                qualificationThesis.Publication = publication;
                context.Publication.Add(publication);
                context.QualificationThesis.Add(qualificationThesis);
                context.SaveChanges();
            }
        }
    }
}
