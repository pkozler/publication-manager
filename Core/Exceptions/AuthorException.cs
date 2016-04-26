using System;

namespace Core
{
    /// <summary>
    /// Třída představuje chybový stav při vytváření záznamu o autorovi.
    /// </summary>
    public class AuthorException : Exception
    {
        /// <inheritDoc/>
        public AuthorException(string message) : base(message)
        {
            // inicializace v nadřazené třídě 
        }
    }
}
