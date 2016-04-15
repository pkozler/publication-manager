﻿using System.Collections.Generic;
using System.Text;

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

        /// <summary>
        /// Vrátí specifické údaje o publikaci příslušného typu.
        /// </summary>
        /// <param name="id">ID publikace</param>
        /// <returns>specifické údaje o publikaci s uvedeným ID</returns>
        /*public TechnicalReport GetPublication(int id)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication publication = GetPublication(context, id);
                return publication.TechnicalReport;
            }
        }*/

        /// <summary>
        /// Uloží novou publikaci příslušného typu a propojí záznam základních a specifických údajů.
        /// </summary>
        /// <param name="publication">základní údaje o publikaci</param>
        /// <param name="technicalReport">specifické údaje o publikaci</param>
        public void CreatePublication(Publication publication, List<Author> authors, TechnicalReport technicalReport)
        {
            using (var context = new DbPublicationEntities())
            {
                publication.TechnicalReport = technicalReport;
                technicalReport.Publication = publication;
                CreatePublication(context, publication, authors);
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
        public void UpdatePublication(int id, Publication publication, List<Author> authors, TechnicalReport technicalReport)
        {
            using (var context = new DbPublicationEntities())
            {
                Publication oldPublication = GetPublication(context, id);
                UpdatePublication(context, oldPublication, publication, authors);
                TechnicalReport oldTechnicalReport = oldPublication.TechnicalReport;
                oldTechnicalReport.Address = technicalReport.Address;
                oldTechnicalReport.Institution = technicalReport.Institution;
                oldTechnicalReport.Number = technicalReport.Number;
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
        public override string ExportPublicationToHtmlDocument(Publication publication)
        {
            return new StringBuilder($"<p>{GeneratePublicationIsoCitation(publication)}</p>")
                    .Append($"<p>{publication.Text}</p>").ToString();
        }
    }
}
