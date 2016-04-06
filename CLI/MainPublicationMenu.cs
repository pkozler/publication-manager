using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    class MainPublicationMenu : AMenu
    {
        public MainPublicationMenu()
        {
            MenuLabel = "Menu správy publikace";
            MenuItems = new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.U, new MenuItem() { Name = "Update", Description = "Spustí průvodce úpravou bibliografických údajů zobrazené publikace." } },
                { ConsoleKey.D, new MenuItem() { Name = "Delete", Description = "Odstraní zobrazenou publikaci (vyžaduje potvrzení)." } },
            };
            AddCommonItems();
        }

        public override void ExitMenu()
        {
            throw new NotImplementedException();
        }
    }
}
