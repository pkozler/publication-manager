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
        /// Uchovává název souboru výchozí šablony.
        /// </summary>
        private const string TEMPLATE = "qualification-thesis";

        /// <summary>
        /// Uchovává název typu 'diplomová práce' pro použití v databázi.
        /// </summary>
        public const string TYPE_MASTER_THESIS = "MastersThesis";

        /// <summary>
        /// Uchovává název typu 'disertační práce' pro použití v databázi.
        /// </summary>
        public const string TYPE_PHD_THESIS = "PhdThesis";

        /// <summary>
        /// Uchovává popis typu 'diplomová práce' pro použití v uživatelském rozhraní.
        /// </summary>
        public string MastersThesisDescription { get; set; }

        /// <summary>
        /// Uchovává popis typu 'disertační práce' pro použití v uživatelském rozhraní.
        /// </summary>
        public string PhdThesisDescription { get; set; }

        /// <inheritDoc/>
        public QualificationThesisModel(DbPublicationEntities context, string typeDescription,
            string masterThesisDescription, string phdThesisDescription)
            : base(context, typeDescription)
        {
            DefaultTemplateFile = TEMPLATE;
            // uložení popisu typu diplomové práce
            MastersThesisDescription = masterThesisDescription;
            // uložení popisu typu disertační práce
            PhdThesisDescription = phdThesisDescription;
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="author">autor publikace</param>
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
            Context.QualificationThesis.Add(qualificationThesis);

            Context.SaveChanges();
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="author">autor publikace</param>
        /// <param name="qualificationThesis">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, Author author, QualificationThesis qualificationThesis)
        {
            Publication oldPublication = GetPublication(id);
            UpdatePublication(oldPublication, publication, author == null ? null : new List<Author> { author });
            QualificationThesis oldQualificationThesis = oldPublication.QualificationThesis;

            if (qualificationThesis.Address != null)
            {
                oldQualificationThesis.Address = qualificationThesis.Address;
            }
            
            if (qualificationThesis.School != null)
            {
                oldQualificationThesis.School = qualificationThesis.School;
            }

            if (qualificationThesis.ThesisType != null)
            {
                oldQualificationThesis.ThesisType = qualificationThesis.ThesisType;
            }
            
            Context.SaveChanges();
        }

        /// <summary>
        /// Odstraní publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        public void DeletePublication(int id)
        {
            Publication oldPublication = GetPublication(id);
            QualificationThesis oldQualificationThesis = oldPublication.QualificationThesis;
            Context.QualificationThesis.Remove(oldQualificationThesis);
            DeletePublication(oldPublication);
            Context.SaveChanges();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationIsoCitation(Publication publication)
        {
            QualificationThesis qualificationThesis = publication.QualificationThesis;

            return new StringBuilder($"{AddTrailingDot(GenerateAuthorCitationString(publication))} ")
                .Append($"{AddTrailingDot(publication.Title)} ")
                .Append($"{qualificationThesis.Address}, ")
                .Append($"{publication.Year}. ")
                .Append($"{AddTrailingDot(qualificationThesis.ThesisType)} ")
                .Append($"{AddTrailingDot(qualificationThesis.School)} ").ToString();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationBibtexEntry(Publication publication)
        {
            QualificationThesis qualificationThesis = publication.QualificationThesis;

            string thesisType = TYPE_PHD_THESIS.Equals(qualificationThesis.ThesisType) ?
                TYPE_PHD_THESIS : TYPE_MASTER_THESIS;

            return new StringBuilder($"@{thesisType}{{{publication.Entry},\n")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"\ttitle={{{publication.Title}}},\n")
                .Append($"\taddress={{{qualificationThesis.Address}}},\n")
                .Append($"\tyear={{{publication.Year}}},\n")
                .Append($"\ttype={{{qualificationThesis.ThesisType}}},\n")
                .Append($"\tschool={{{qualificationThesis.School}}}\n}}\n").ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string templatePath, string htmlPath)
        {
            StringTemplate stringTemplate = LoadHtmlTemplate(publication, templatePath);

            QualificationThesis qualificationThesis = publication.QualificationThesis;
            stringTemplate.SetAttribute("address", qualificationThesis.Address);
            stringTemplate.SetAttribute("school", qualificationThesis.School);
            stringTemplate.SetAttribute("type", qualificationThesis.ThesisType == TYPE_MASTER_THESIS ? 
                MastersThesisDescription : PhdThesisDescription);

            return SaveHtmlDocument(stringTemplate, htmlPath);
        }
    }
}
