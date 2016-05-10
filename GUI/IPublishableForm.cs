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
        /// <summary>
        /// Vrátí objekt datové vrstvy pro správu příslušného typu publikací.
        /// </summary>
        /// <returns>objekt pro správu</returns>
        APublicationModel GetModel();
        
        /// <summary>
        /// Vypíše uložené specifické údaje aktuální zobrazené publikace konkrétního typu.
        /// </summary>
        /// <param name="publication">aktuální publikace</param>
        void GetSpecificBibliography(Publication publication);
    }
}
