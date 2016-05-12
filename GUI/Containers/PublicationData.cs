using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace GUI
{
    class PublicationData
    {
        public readonly Publication Publication;
        public readonly List<Author> Authors;
        public readonly ASpecificPublication SpecificPublication;

        public PublicationData(Publication publication, List<Author> authors, ASpecificPublication specificPublication)
        {
            Publication = publication;
            Authors = authors;
            SpecificPublication = specificPublication;
        }
    }
}
