/*
 * Program:     GHGEmissions.exe
 * Module:      Program.cs
 * Course:      INFO-3138
 * Date:        July 11, 2023
 * Description: todo -> A sample solution to the DOM coding exercise involving driving directions.
 *              This program uses the DOM to complete a set of driving directions contained in the XML file
 *              "directions.xml". You will find this file nested within the project's bin\Debug folder.
 */

using System;
using System.Xml;   // XmlDocument class and DOM interfaces
using System.IO;    // IOException class
using GHGEmission;

namespace GHGEmissions
{
    internal class Program
    {
        const string XmlFile = @"../../../data/ghg-canada.xml";
        const string lineSeparator = "---------------------------------------------------------------------------";

        private static EmissionsReport report = EmissionsReport.GetInstance();

        static void Main()
        {
            while (true)
            {
                DisplayMenu("Main Menu");
                MainMenuListener();
                ClearConsole();
            }
        }

        private static void DisplayMenu(string title)
        {
            Console.WriteLine(lineSeparator);
            Console.WriteLine("".PadLeft(10) + $"Greenhouse Gas Emissions in Canada ~ {title}");
            Console.WriteLine(lineSeparator);

            switch(title)
            {
                case "Main Menu":
                    Console.WriteLine($"  (1) To adjust the range of years (currently {report.StartingYear} to {report.EndingYear})");
                    Console.WriteLine($"  (2) To select a different region (currently {report.Region})");
                    Console.WriteLine($"  (3) To select a different GHG source (currently {report.Source})");
                    Console.WriteLine($"  (4) To learn about {report.Region}'s emissions");
                    Console.WriteLine($"  (5) To learn about the {report.Source} sector's emissions");
                    Console.WriteLine("  (X) To quit");
                    break;

                case "Region Selection":
                    Console.WriteLine("  (1) Alberta");
                    Console.WriteLine("  (2) British Columbia");
                    Console.WriteLine("  (3) Manitoba");
                    Console.WriteLine("  (4) New Brunswick");
                    Console.WriteLine("  (5) Newfoundland and Labrador");
                    Console.WriteLine("  (6) Northwest Territories");
                    Console.WriteLine("  (7) Northwest Territories and Nunavut");
                    Console.WriteLine("  (8) Nova Scotia");
                    Console.WriteLine("  (9) Nunavut");
                    Console.WriteLine("  (10) Ontario");
                    Console.WriteLine("  (11) Prince Edward Island");
                    Console.WriteLine("  (12) Quebec");
                    Console.WriteLine("  (13) Saskatchewan");
                    Console.WriteLine("  (14) Yukon");
                    Console.WriteLine("  (15) Canada");
                    break;

                case "Source Selection":
                    Console.WriteLine("  (1) Agriculture");
                    Console.WriteLine("  (2) Buildings");
                    Console.WriteLine("  (3) Heavy Industry");
                    Console.WriteLine("  (4) Light Manufactoring, Construction and Forest Resources");
                    Console.WriteLine("  (5) Oil and Gas");
                    Console.WriteLine("  (6) Transport");
                    Console.WriteLine("  (7) Waste");
                    Console.WriteLine("  (8) Total");
                    break;

                default:
                    Console.WriteLine("Invalid Menu Title.");
                    break;
            }
            
            Console.WriteLine(lineSeparator + "\n\n");
        }

        private static void MainMenuListener()
        {
            Console.Write("Enter your selection: ");
            string input = Console.ReadLine() ?? "";

            switch (input.ToUpper())
            {
                case "1":
                    report.StartingYear = GetValidYearInput("\nStarting year (1990 to 2019): ", 1990, 2019);
                    report.EndingYear = GetValidYearInput($"Ending year ({report.StartingYear} to {int.Parse(report.StartingYear) + 4}): ", int.Parse(report.StartingYear), int.Parse(report.StartingYear) + 4);
                    break;

                case "2":
                    Console.Clear();
                    DisplayMenu("Region Selection");
                    RegionListener();
                    break;

                case "3":
                    Console.Clear();
                    DisplayMenu("Source Selection");
                    SourceListener();
                    break;

                case "4":
                    break;

                case "5":
                    break;

                case "X":
                    Environment.Exit(0);
                    break;
                    
                default:
                    Console.WriteLine("Invalid Main Menu Selection.");
                    break;
            }
        }

        private static void RegionListener()
        {
            Console.Write("Enter a region #: ");
            string input = Console.ReadLine() ?? "";

            switch (input)
            {
                case "1":
                    report.Region = "Alberta";
                    break;

                case "2":
                    report.Region = "British Columbia";
                    break;

                case "3":
                    report.Region = "Manitoba";
                    break;

                case "4":
                    report.Region = "New Brunswick";
                    break;

                case "5":
                    report.Region = "Newfoundland and Labrador";
                    break;

                case "6":
                    report.Region = "Northwest Territories";
                    break;

                case "7":
                    report.Region = "Northwest Territories and Nunavut";
                    break;

                case "8":
                    report.Region = "Nova Scotia";
                    break;

                case "9":
                    report.Region = "Nunavut";
                    break;

                case "10":
                    report.Region = "Ontario";
                    break;

                case "11":
                    report.Region = "Prince Edward Island";
                    break;

                case "12":
                    report.Region = "Quebec";
                    break;

                case "13":
                    report.Region = "Saskatchewan";
                    break;

                case "14":
                    report.Region = "Yukon";
                    break;

                case "15":
                    report.Region = "Canada";
                    break;

                default:
                    Console.WriteLine("Invalid Region Menu Selection.");
                    break;
            }
        }

        private static void SourceListener()
        {
            Console.Write("Enter a source #: ");
            string input = Console.ReadLine() ?? "";

            switch (input)
            {
                case "1":
                    report.Source = "Agriculture";
                    break;

                case "2":
                    report.Source = "Buildings";
                    break;

                case "3":
                    report.Source = "Heavy Industry";
                    break;

                case "4":
                    report.Source = "Light Manufactoring, Construction and Forest Resources";
                    break;

                case "5":
                    report.Source = "Oil and Gas";
                    break;

                case "6":
                    report.Source = "Transport";
                    break;

                case "7":
                    report.Source = "Waste";
                    break;

                case "8":
                    report.Source = "Total";
                    break;

                default:
                    Console.WriteLine("Invalid Source Menu Selection.");
                    break;
            }
        }

        private static string GetValidYearInput(string prompt, int minValue, int maxValue)
        {
            string? year = string.Empty;
            bool invalid = true;

            while (invalid)
            {
                Console.Write(prompt);
                year = Console.ReadLine();

                if (!string.IsNullOrEmpty(year) && int.TryParse(year, out int yearInt) && yearInt >= minValue && yearInt <= maxValue)
                {
                    invalid = false;
                }
                else
                {
                    Console.WriteLine($"ERROR: Input must be an integer between {minValue} and {maxValue}");
                }
            }

            return year!;
        }

        private static void ClearConsole()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            //just clearing the console doesn't clear everything if there's a scrollbar (hidden content)
            //the following line fixes that issue (for some reason)
            Console.WriteLine("\x1b[3J");
        }
    }
}