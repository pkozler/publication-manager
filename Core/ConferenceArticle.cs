//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConferenceArticle : ASpecificPublication
    {
        public int PublicationId { get; set; }
        public string BookTitle { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public string Address { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }
    
        public virtual Publication Publication { get; set; }
    }
}
