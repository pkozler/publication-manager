using System.Collections.Generic;
using System.Text;
using Antlr3.ST;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "kvalifikační práce".
    /// </summary>
    public class QualificationThesisModel : APublicationModel
    {
        /// <summary>
        /// Uchovává název typu pro použití v databázi.
        /// </summary>
        public const string NAME = "QualificationThesis";

        /// <summary>
        /// Uchovává název označení typu 'diplomová práce' pro použití v databázi.
        /// </summary>
        public const string TYPE_MASTER_THESIS = "MastersThesis";

        /// <summary>
        /// Uchovává název označení typu 'disertační práce' pro použití v databázi.
        /// </summary>
        public const string TYPE_PHD_THESIS = "PhdThesis";
        
        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        /*public QualificationThesis GetPublication(int id)
        {
            Publication publication = GetPublication(context, id);
            return publication.QualificationThesis;
        }*/

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="qualificationThesis">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, Author author, QualificationThesis qualificationThesis)
        {
            if (author == null)
            {
                throw new PublicationException("Kvalifikační práce musí mít právě jednoho autora.");
            }

            publication.QualificationThesis = qualificationThesis;
            qualificationThesis.Publication = publication;
            CreatePublication(publication, author == null ? null : new List<Author> { author });
            context.QualificationThesis.Add(qualificationThesis);
                
            context.SaveChanges();
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="qualificationThesis">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, Author author, QualificationThesis qualificationThesis)
        {
            Publication oldPublication = GetPublication(id);
            UpdatePublication(oldPublication, publication, author == null ? null : new List<Author> { author });
            QualificationThesis oldQualificationThesis = oldPublication.QualificationThesis;
            oldQualificationThesis.Address = qualificationThesis.Address;
            oldQualificationThesis.School = qualificationThesis.School;
            oldQualificationThesis.ThesisType = qualificationThesis.ThesisType;
            context.SaveChanges();
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            Publication oldPublication = GetPublication(id);
            QualificationThesis oldQualificationThesis = oldPublication.QualificationThesis;
            context.QualificationThesis.Remove(oldQualificationThesis);
            DeletePublication(oldPublication);
            context.SaveChanges();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationIsoCitation(Publication publication)
        {
            QualificationThesis qualificationThesis = publication.QualificationThesis;

            return new StringBuilder(GenerateAuthorCitationString(publication))
                .Append($"{publication.Title}. ")
                .Append($"{qualificationThesis.Address}, ")
                .Append($"{publication.Year}. ")
                .Append($"{qualificationThesis.ThesisType}. ")
                .Append($"{qualificationThesis.School}. ").ToString();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationBibtexEntry(Publication publication)
        {
            QualificationThesis qualificationThesis = publication.QualificationThesis;

            string thesisType = TYPE_PHD_THESIS.Equals(qualificationThesis.ThesisType) ?
                TYPE_PHD_THESIS : TYPE_MASTER_THESIS;

            return new StringBuilder($"@{thesisType}{{{publication.Entry},")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"title={{{publication.Title}}},")
                .Append($"address={{{qualificationThesis.Address}}},")
                .Append($"year={{{publication.Year}}},")
                .Append($"type={{{qualificationThesis.ThesisType}}},")
                .Append($"school={{{qualificationThesis.School}}}}}").ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string publicationType, string template)
        {
            StringTemplate stringTemplate = PrepareHtmlTemplate(publication, publicationType, template);
            QualificationThesis qualificationThesis = publication.QualificationThesis;
            stringTemplate.SetAttribute("address", qualificationThesis.Address);
            stringTemplate.SetAttribute("school", qualificationThesis.School);
            stringTemplate.SetAttribute("type", qualificationThesis.ThesisType == TYPE_MASTER_THESIS ? 
                "diplomová práce" : "disertační práce");

            return stringTemplate.ToString();
        }
    }
}
