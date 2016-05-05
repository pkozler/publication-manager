namespace Core
{
    /// <summary>
    /// Třída představuje obecnou entitu správce vědeckých textů
    /// (publikaci/autora/přílohu), jejíž záznamy jsou jednoznačně 
    /// identifikovány a mohou být řazeny a filtrovány pomocí číselného ID.
    /// </summary>
    public abstract class IdEntity
    {
        /// <summary>
        /// Představuje unikátní číselné ID entity.
        /// </summary>
        public int Id { get; set; }
    }
}
