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

namespace GHGEmissions
{
    internal class Program
    {
        const string XmlFile = @"../../../data/ghg-canada.xml";
        const string lineSeparator = "---------------------------------------------------------------------------";

        static void Main()
        {
            DisplayMenu("Main Menu");
            DisplayMenu("Region Selection");
            DisplayMenu("Source Selection");
        }

        private static void DisplayMenu(string title)
        {
            Console.WriteLine(lineSeparator);
            Console.WriteLine("".PadLeft(10) + $"Greenhouse Gas Emissions in Canada ~ {title}");
            Console.WriteLine(lineSeparator);

            switch(title)
            {
                case "Main Menu":
                    Console.WriteLine("  (1) To adjust the range of years (currently 2015 to 2019)");
                    Console.WriteLine("  (2) To select a different region (currently Canada)");
                    Console.WriteLine("  (3) To select a different HG source (currently Oil and Gas)");
                    Console.WriteLine("  (4) To learn about Canada's emissions");
                    Console.WriteLine("  (5) To learn about the Oil and Gas sector's emissions");
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
    }
}