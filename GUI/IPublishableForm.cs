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
        void ViewPublication(Publication publication);
        
        void InsertPublication(Publication publication, List<Author> authors);
        
        void EditPublication(int publicationId, Publication publication, List<Author> authors);
        
        void DeletePublication(int publicationId);
    }
}
