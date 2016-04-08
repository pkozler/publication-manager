namespace Core
{
    /// <summary>
    /// Třída představuje obecného správce specifických
    /// bibliografických údajů pro konkrétní typ publikace.
    /// </summary>
    public abstract class APublicationModel
    {
        /// <summary>
        /// Načte základní údaje o publikaci se zadaným ID.
        /// </summary>
        /// <param name="context">aktuální kontext</param>
        /// <param name="id">ID publikace</param>
        /// <returns>publikace</returns>
        protected Publication GetPublication(DbPublicationEntities context, int id)
        {
            return context.Publication.Find(id);
        }

        /// <summary>
        /// Vytvoří novou publikaci se zadanými údaji.
        /// </summary>
        /// <param name="context">aktuální kontext</param>
        /// <param name="publication">údaje publikace</param>
        protected void CreatePublication(DbPublicationEntities context, Publication publication)
        {
            context.Publication.Add(publication);
        }

        /// <summary>
        /// Upraví zadanou publikaci zadanými údaji.
        /// </summary>
        /// <param name="context">aktuální kontext</param>
        /// <param name="oldPublication">existující publikace</param>
        /// <param name="publication">nové údaje</param>
        protected void UpdatePublication(DbPublicationEntities context, Publication oldPublication, Publication publication)
        {
            oldPublication.Entry = publication.Entry;
            oldPublication.Title = publication.Title;
            oldPublication.Year = publication.Year;
            oldPublication.Type = publication.Type;
        }

        /// <summary>
        /// Odstraní zadanou publikaci.
        /// </summary>
        /// <param name="context">aktuální kontext</param>
        /// <param name="oldPublication">existující publikace</param>
        protected void DeletePublication(DbPublicationEntities context, Publication oldPublication)
        {
            context.Publication.Remove(oldPublication);
        }
    }
}
