using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace GUI
{
    public interface IPublishableForm
    {
        List<string> ValidatePublicationTypeSpecificBibliography(
            Publication publication, List<Author> authors, out ASpecificPublication specificPublication);

        void ViewPublication(Publication publication);
        
        void InsertPublication(Publication publication, List<Author> authors,
            ASpecificPublication specificPublication);
        
        void EditPublication(int publicationId, Publication publication, List<Author> authors,
            ASpecificPublication specificPublication);
        
        void DeletePublication(int publicationId);
    }
}
