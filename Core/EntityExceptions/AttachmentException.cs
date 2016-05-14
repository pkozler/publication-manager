using System;

namespace Core
{
    /// <summary>
    /// Třída představuje chybový stav při vytváření záznamu o příloze.
    /// </summary>
    public class AttachmentException : Exception
    {
        /// <inheritDoc/>
        public AttachmentException(string message) : base(message)
        {
            // inicializace v nadřazené třídě 
        }
    }
}
