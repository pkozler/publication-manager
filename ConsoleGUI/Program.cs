using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Core;

namespace ConsoleGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.PrintMenu();

            while (mainMenu.IsRunning)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                mainMenu.RunMethod(keyInfo.Key);
            }
        }
    }
}