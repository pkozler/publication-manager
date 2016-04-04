using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public sealed class PublicationType
    {
        private readonly string name;
        private readonly int value;

        public static readonly PublicationType CONFERENCE_ARTICLE = new PublicationType("ConferenceArticle", 1);
        public static readonly PublicationType JOURNAL_ARTICLE = new PublicationType("JournalArticle", 2);
        public static readonly PublicationType TECHNICAL_REPORT = new PublicationType("TechnicalReport", 4);
        public static readonly PublicationType QUALIFICATION_THESIS = new PublicationType("QualificationThesis", 8);

        private PublicationType(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
