using System;
using System.Collections.Generic;
using System.Linq;
using Core;

using static System.Console;

namespace CLI
{
    /// <summary>
    /// Třída rozšiřuje funkcionalitu třídy Console o metody pro obvyklé způsoby
    /// zadávání různých typů bibliografických údajů (čísla, jména, názvy)
    /// a údajů o autorech se základním ošetřením chybových vstupů 
    /// a o metody pro výpisy často vypisovaných údajů (seznam autorů a typů publikací).
    /// </summary>
    sealed class ConsoleExtension
    {
        /// <summary>
        /// Načte od uživatele seznam autorů.
        /// </summary>
        /// <param name="authorModel">správce záznamů o uložených autorech</param>
        /// <param name="nameMessage">výzva k zadání křestního jména</param>
        /// <param name="surnameMessage">výzva k zadání příjmení</param>
        /// <returns>seznam autorů</returns>
        public static List<Author> ReadAuthorList(AuthorModel authorModel, string nameMessage, string surnameMessage)
        {
            List <Author> authors = new List<Author>();
            
            while (true)
            {
                Author author = new Author();
                string name = ReadLine().Trim();

                if (string.IsNullOrEmpty(name))
                {
                    // byl zadán prázdný řádek a seznam není prázdný
                    if (authors.Count > 0)
                    {
                        break;
                    }
                    // byl zadán prázdný řádek a seznam je prázdný
                    else
                    {
                        continue;
                    }
                }

                // bylo zadáno číslo (považuje se za ID uloženého autora)
                int authorId;
                if (int.TryParse(name, out authorId))
                {
                    authorModel.GetAuthorById(authorId);
                }
                // byl zadán řetězec (považuje se za jméno nového autora)
                else
                {
                    author.Name = name;
                    WriteLine(surnameMessage);
                    author.Surname = ReadNonEmptyString(surnameMessage);
                }

                authors.Add(author);
                WriteLine(nameMessage);
            }

            return authors;
        }

        /// <summary>
        /// Načte od uživatele neprázdný řetězec.
        /// </summary>
        /// <param name="errorMessage">výzva při vložení neplatného vstupu</param>
        /// <returns>neprázdný řetězec</returns>
        public static string ReadNonEmptyString(string errorMessage)
        {
            string str = ReadLine().Trim();

            while (string.IsNullOrEmpty(str))
            {
                WriteLine("Neplatný vstup. " + errorMessage);
                str = ReadLine().Trim();
            }

            return str;
        }

        /// <summary>
        /// Načte od uživatele platné celé číslo.
        /// </summary>
        /// <param name="errorMessage">výzva při vložení neplatného vstupu</param>
        /// <returns>celé číslo</returns>
        public static int ReadValidNumber(string errorMessage)
        {
            int id;
            while (!int.TryParse(ReadLine(), out id))
            {
                WriteLine("Neplatný vstup. " + errorMessage);
            }

            return id;
        }

        /// <summary>
        /// Načte od uživatele odpověď ano/ne.
        /// </summary>
        /// <param name="question">otázka k potvrzení</param>
        /// <returns>true při potvrzení, jinak false</returns>
        public static bool ReadYesNoAnswer(string question)
        {
            do
            {
                WriteLine("\n{0} (y/n)", question);
                ConsoleKeyInfo keyInfo = ReadKey();

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (keyInfo.Key == ConsoleKey.N)
                {
                    return false;
                }
            }
            while (true);
        }

        /// <summary>
        /// Vypíše typy publikací s číselným označením.
        /// </summary>
        /// <param name="description">úvodní popis</param>
        /// <param name="publicationTypes">seznam typů</param>
        public static void WritePublicationTypes(string description, List<PublicationType> publicationTypes)
        {
            WriteLine(description);

            for (int i = 0; i < publicationTypes.Count; i++)
            {
                WriteLine($"\t{i} - {publicationTypes[i].Description}");
            }
        }

        /// <summary>
        /// Vypíše seznam autorů.
        /// </summary>
        /// <param name="authorList">kolekce autorů</param>
        public static void WriteAuthors(ICollection<Author> authorList)
        {
            Author[] authors = authorList.ToArray();
            Write($"{authors[0].Name} {authors[0].Surname}");

            for (int i = 1; i < authors.Length; i++)
            {
                Write($", {authors[i].Name} {authors[i].Surname}");
            }

            WriteLine();
        }

        /// <summary>
        /// Vypíše seznam autorů s jejich ID.
        /// </summary>
        /// <param name="authorList">seznam autorů</param>
        public static void WriteAuthorsWithId(List<Author> authorList)
        {
            WriteLine("ID\tJméno\tPříjmení");

            foreach (Author author in authorList)
            {
                WriteLine($"{author.Id}\t{author.Name}\t{author.Surname}");
            }

            WriteLine();
        }
    }
}
