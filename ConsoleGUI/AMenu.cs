using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGUI
{
    public abstract class AMenu
    {
        /// <summary>
        /// Popisek daného menu, který se používá při výpisu dostupných příkazů.
        /// </summary>
        protected string MenuLabel { get; set; }

        /// <summary>
        /// Slovník, který slouží pro asociaci dostupných příkazů s příslušnými operacemi
        /// v daném menu konzolového uživatelského rozhraní.
        /// </summary>
        protected Dictionary<ConsoleKey, MenuItem> MenuItems { get; set; } = new Dictionary<ConsoleKey, MenuItem>();

        /// <summary>
        /// Přidá do menu položky pro běžné příkazy (nápověda a návrat do předchozího menu).
        /// </summary>
        protected void AddCommonItems()
        {
            MenuItems.Add(ConsoleKey.H, new MenuItem() { Name = "Help", Description = "Vypíše znovu seznam příkazů.", UIMethod = PrintMenu });
            MenuItems.Add(ConsoleKey.Q, new MenuItem() { Name = "Quit", Description = "Ukončí program.", UIMethod = ExitMenu });
        }

        /// <summary>
        /// Zavolá metodu podle zadaného příkazu.
        /// </summary>
        /// <param name="command">příkaz</param>
        public virtual void RunMethod(ConsoleKey command)
        {
            if (MenuItems.ContainsKey(command))
            {
                MenuItems[command].UIMethod();
            }
        }

        /// <summary>
        /// Vypíše dostupné příkazy.
        /// </summary>
        public virtual void PrintMenu()
        {
            Console.WriteLine($"\n----- {MenuLabel} -----\nPříkazy:");
            foreach (var item in MenuItems)
            {
                Console.WriteLine($"\t{item.Key.ToString().ToLower()} - {item.Value.Name}: {item.Value.Description}");
            }
        }

        /// <summary>
        /// Opustí aktuální menu.
        /// </summary>
        public abstract void ExitMenu();
    }
}
