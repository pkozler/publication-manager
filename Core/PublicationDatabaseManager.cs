using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class PublicationDatabaseManager
    {
        PublicationDatabaseEntities db;

        public PublicationDatabaseManager()
        {
            db = new PublicationDatabaseEntities();
        }
    }
}
