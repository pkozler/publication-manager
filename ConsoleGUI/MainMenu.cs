using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGUI
{
    public class MainMenu : AMenu
    {
        public bool IsRunning { get; private set; }

        public MainMenu()
        {
            IsRunning = true;
            MenuLabel = "Hlavní menu";
            MenuItems = new Dictionary<ConsoleKey, MenuItem>()
            {
                { ConsoleKey.L, new MenuItem() { Name = "List", Description = "Vypíše seznam publikací se zadanými filtry.", UIMethod = GetPublicationList } },
                { ConsoleKey.R, new MenuItem() { Name = "Read", Description = "Načte detail zadané publikace, kterou lze upravit nebo odstranit.", UIMethod = GetPublicationDetail } },
                { ConsoleKey.C, new MenuItem() { Name = "Create", Description = "Vytvoří novou publikaci.", UIMethod = CreateNewPublication } },
                { ConsoleKey.A, new MenuItem() { Name = "Authors", Description = "Vypíše seznam uložených autorů.", UIMethod = GetAuthorList } },
                { ConsoleKey.I, new MenuItem() { Name = "Info", Description = "Vypíše stručný popis programu.", UIMethod = PrintInfo } },
            };
            AddCommonItems();
        }

        public void GetPublicationList()
        {

        }

        public void GetPublicationDetail()
        {

        }

        public void CreateNewPublication()
        {

        }

        public void GetAuthorList()
        {

        }

        public void PrintInfo()
        {
            Console.WriteLine("\nO programu:");
            Console.WriteLine("\tAplikace na evidenci vědeckých textů - testovací konzolové rozhraní (Verze 1.0)");
            Console.WriteLine("\tSemestrální práce z předmětu \"Programování v prostředí .NET\" (KIV/NET)");
            Console.WriteLine("\tCopyright (c) Petr Kozler (A13B0359P), 2016\n");
        }
             
        public override void ExitMenu()
        {
            do
            {
                Console.WriteLine("\nOpravdu chcete ukončit program? (y/n)");
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    IsRunning = false;
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.N)
                {
                    PrintMenu();
                    break;
                }
            }
            while (true);
        }
    }
}
