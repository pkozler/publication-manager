using System.Collections.Generic;
using System.Text;
using Antlr3.ST;

namespace Core
{
    /// <summary>
    /// Třída představuje správce bibliografických údajů o publikaci typu
    /// "technická zpráva".
    /// </summary>
    public class TechnicalReportModel : APublicationModel
    {
        /// <summary>
        /// Uchovává název typu pro použití v databázi.
        /// </summary>
        public const string NAME = "TechnicalReport";
        
        /// <inheritDoc/>
        public TechnicalReportModel(DbPublicationEntities context, string typeDescription, string defaultTemplate)
            : base(context, typeDescription, defaultTemplate)
        {
            // inicializace v nadřazené třídě
        }

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="technicalReport">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, List<Author> authors, TechnicalReport technicalReport)
        {
            publication.TechnicalReport = technicalReport;
            technicalReport.Publication = publication;
            CreatePublication(publication, authors);
            Context.TechnicalReport.Add(technicalReport);
            Context.SaveChanges();
        }

        /// <summary>
        /// Aktualizuje údaje o existující publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="technicalReport">specifické údaje o publikaci</param>
        public void UpdatePublication(int id, Publication publication, List<Author> authors, TechnicalReport technicalReport)
        {
            Publication oldPublication = GetPublication(id);
            UpdatePublication(oldPublication, publication, authors);
            TechnicalReport oldTechnicalReport = oldPublication.TechnicalReport;

            if (technicalReport.Address != null)
            {
                oldTechnicalReport.Address = technicalReport.Address;
            }
            
            if (technicalReport.Institution != null)
            {
                oldTechnicalReport.Institution = technicalReport.Institution;
            }
            
            if (technicalReport.Number != null)
            {
                oldTechnicalReport.Number = technicalReport.Number;
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
            TechnicalReport oldTechnicalReport = oldPublication.TechnicalReport;
            Context.TechnicalReport.Remove(oldTechnicalReport);
            DeletePublication(oldPublication);
            Context.SaveChanges();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationIsoCitation(Publication publication)
        {
            TechnicalReport technicalReport = publication.TechnicalReport;

            return new StringBuilder(GenerateAuthorCitationString(publication))
                .Append($"{publication.Title}. ")
                .Append($"{technicalReport.Address}: ")
                .Append($"{technicalReport.Institution}, ")
                .Append($"{publication.Year}. ")
                .Append($"č. {technicalReport.Number}.").ToString();
        }

        /// <inheritDoc/>
        public override string GeneratePublicationBibtexEntry(Publication publication)
        {
            TechnicalReport technicalReport = publication.TechnicalReport;
                
            return new StringBuilder($"@TechReport{{{publication.Entry},")
                .Append(GenerateAuthorBibtexString(publication))
                .Append($"title={{{publication.Title}}},")
                .Append($"address={{{technicalReport.Address}}},")
                .Append($"institution={{{technicalReport.Institution}}},")
                .Append($"year={{{publication.Year}}},")
                .Append($"number={{{technicalReport.Number}}}}}").ToString();
        }

        /// <inheritDoc/>
        public override string ExportPublicationToHtmlDocument(Publication publication, string templatePath, string htmlPath)
        {
            StringTemplate stringTemplate = LoadHtmlTemplate(publication, templatePath);

            TechnicalReport technicalReport = publication.TechnicalReport;
            stringTemplate.SetAttribute("address", technicalReport.Address);
            stringTemplate.SetAttribute("institution", technicalReport.Institution);
            stringTemplate.SetAttribute("number", technicalReport.Number);

            return SaveHtmlDocument(stringTemplate, htmlPath);
        }
    }
}
