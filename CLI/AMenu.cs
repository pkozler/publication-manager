﻿using System;
using System.Collections.Generic;
using System.Linq;

using static System.Console;

namespace CLI
{
    /// <summary>
    /// Třída představuje obecnou konzolovou nabídku příkazů, uchovávající společné znaky pro tyto nabídky
    /// (název, slovník příkazů a operací a metody pro výpis menu, ukončení menu a spuštění příkazu).
    /// </summary>
    public abstract class AMenu
    {
        /// <summary>
        /// Příznak, který slouží k určení toho, zda uživatel neopustil menu
        /// (pokud ano, přeruší se smyčka načítání příkazů aktuálního menu).
        /// </summary>
        public bool IsRunning { get; protected set; } = true;

        /// <summary>
        /// Slovník, který slouží pro asociaci dostupných příkazů s příslušnými operacemi
        /// v daném menu konzolového uživatelského rozhraní.
        /// </summary>
        protected Dictionary<ConsoleKey, MenuItem> MenuItems { get; private set; }

        /// <summary>
        /// Popisek aktuálního menu, který se používá při výpisu dostupných příkazů.
        /// </summary>
        protected string MenuLabel { get; set; }

        /// <summary>
        /// Inicializuje slovník položek pro aktuální menu a dodá položky společné pro všechna menu
        /// (nápověda příkazů a návrat do nadřazeného menu).
        /// </summary>
        /// <param name="menuItems">slovník položek</param>
        protected void InitializeMenuItems(Dictionary<ConsoleKey, MenuItem> menuItems)
        {
            MenuItems = new Dictionary<ConsoleKey, MenuItem>(menuItems);
            MenuItems.Add(ConsoleKey.H, new MenuItem() { Name = "Help", Description = "Vypíše znovu seznam příkazů aktuálního menu.", UIMethod = PrintMenu });
            MenuItems.Add(ConsoleKey.Q, new MenuItem() { Name = "Quit", Description = "Opustí aktuální menu a vrátí se do nadřazeného menu.", UIMethod = ExitMenu });
        }

        /// <summary>
        /// Spustí smyčku pro načítání příkazů uživatele
        /// (nutno zavolat pro spuštění menu po jeho inicializaci).
        /// </summary>
        public void Start()
        {
            // počáteční výpis dostupných příkazů
            PrintMenu();

            // čtení příkazů, dokud uživatel neopustí menu
            while (IsRunning)
            {
                RunMethod(ReadKey().Key);
            }
        }
        
        /// <summary>
        /// Zavolá metodu podle zadaného příkazu.
        /// </summary>
        /// <param name="command">příkaz</param>
        public virtual void RunMethod(ConsoleKey command)
        {
            // kontrola, zda je příkaz platný
            if (MenuItems.ContainsKey(command))
            {
                WriteLine();

                try
                {
                    MenuItems[command].UIMethod();
                }
                catch (Exception e)
                {
                    WriteLine("CHYBA:");
                    WriteLine(e.Message);
                }

                WriteLine();

                if (MenuItems[command].UIMethod != PrintMenu)
                {
                    PrintMenu();
                }
            }
        }

        /// <summary>
        /// Vypíše položky menu.
        /// </summary>
        public virtual void PrintMenu()
        {
            string itemFormat = "\t{0} - {1}: {2}";
            ConsoleKey[] commonItemKeys = new ConsoleKey[] { ConsoleKey.H, ConsoleKey.Q };
            WriteLine("----- {0} -----\nSeznam příkazů:", MenuLabel);

            // výpis specifických položek menu
            foreach (var item in MenuItems)
            {
                // přeskočení společných položek (vypíší se nakonec)
                if (commonItemKeys.Contains(item.Key))
                {
                    continue;
                }

                WriteLine(itemFormat, item.Key.ToString().ToLower(), item.Value.Name, item.Value.Description);
            }

            // dodatečný výpis společných položek menu
            foreach (var key in commonItemKeys)
            {
                WriteLine(itemFormat, key.ToString().ToLower(), MenuItems[key].Name, MenuItems[key].Description);
            }
        }

        /// <summary>
        /// Vyvolá opuštění menu.
        /// </summary>
        public virtual void ExitMenu()
        {
            IsRunning = false;
        }
    }
}
