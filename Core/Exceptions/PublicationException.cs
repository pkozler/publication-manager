using System;

namespace Core
{
    /// <summary>
    /// Třída představuje chybový stav při vytváření obecné publikace.
    /// </summary>
    public class PublicationException : Exception
    {
        /// <inheritDoc/>
        public PublicationException(string message) : base(message)
        {
            // inicializace v nadřazené třídě 
        }
    }
}
