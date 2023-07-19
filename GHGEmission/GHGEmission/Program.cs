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
using System.Xml.XPath;

namespace GHGEmissions
{
    internal class Program
    {
        const string XmlFile = @"../../../data/ghg-canada.xml";
        const string lineSeparator = "----------------------------------------------------------------------------------------------------------";

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
            Console.WriteLine("".PadLeft(30) + $"Greenhouse Gas Emissions in Canada ~ {title}");
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
                    List<string> regions = GetRegionList();

                    for (int i = 0; i < regions.Count; i++)
                        Console.WriteLine($"  ({i + 1}) {regions[i]}");

                    //Console.WriteLine("  (1) Alberta");
                    //Console.WriteLine("  (2) British Columbia");
                    //Console.WriteLine("  (3) Manitoba");
                    //Console.WriteLine("  (4) New Brunswick");
                    //Console.WriteLine("  (5) Newfoundland and Labrador");
                    //Console.WriteLine("  (6) Northwest Territories");
                    //Console.WriteLine("  (7) Northwest Territories and Nunavut");
                    //Console.WriteLine("  (8) Nova Scotia");
                    //Console.WriteLine("  (9) Nunavut");
                    //Console.WriteLine("  (10) Ontario");
                    //Console.WriteLine("  (11) Prince Edward Island");
                    //Console.WriteLine("  (12) Quebec");
                    //Console.WriteLine("  (13) Saskatchewan");
                    //Console.WriteLine("  (14) Yukon");
                    //Console.WriteLine("  (15) Canada");
                    break;

                case "Source Selection":
                    List<string> sources = GetSourceList();

                    for (int i = 0; i < sources.Count; i++)
                        Console.WriteLine($"  ({i + 1}) {sources[i]}");

                    //Console.WriteLine("  (1) Agriculture");
                    //Console.WriteLine("  (2) Buildings");
                    //Console.WriteLine("  (3) Heavy Industry");
                    //Console.WriteLine("  (4) Light Manufactoring, Construction and Forest Resources");
                    //Console.WriteLine("  (5) Oil and Gas");
                    //Console.WriteLine("  (6) Transport");
                    //Console.WriteLine("  (7) Waste");
                    //Console.WriteLine("  (8) Total");
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
                    Console.Clear();
                    Dictionary<string, List<string>> emissionsBySource = GenerateRegionReport();
                    DisplayReport("Region", emissionsBySource);
                    break;

                case "5":
                    Console.Clear();
                    Dictionary<string, List<string>> emissionsByRegion = GenerateSourceReport();
                    DisplayReport("Source", emissionsByRegion);
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

        private static List<string> GetRegionList()
        {
            List<string> regions = new();

            try
            {
                XmlDocument doc = new();
                doc.Load(XmlFile);

                XPathNavigator navigator = doc.CreateNavigator()!;
                string sourceXPath = "//region";
                XPathNodeIterator sourceIterator = navigator.Select(sourceXPath);

                while (sourceIterator.MoveNext())
                {
                    XPathNavigator sourceNode = sourceIterator.Current!;
                    string region = sourceNode.GetAttribute("name", "");
                    regions.Add(region);
                }
            }
            catch(XmlException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (XPathException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            return regions;
        }

        private static void SourceListener()
        {
            Console.Write("Enter a source #: ");
            string input = Console.ReadLine() ?? "";

            List<string> sources = GetSourceList();
            for (int i = 0; i < sources.Count; i++)
            {
                if (int.Parse(input) == i + 1)
                    report.Source = sources[i];
            }

            //switch (input)
            //{
            //    case "1":
            //        report.Source = "Agriculture";
            //        break;

            //    case "2":
            //        report.Source = "Buildings";
            //        break;

            //    case "3":
            //        report.Source = "Heavy Industry";
            //        break;

            //    case "4":
            //        report.Source = "Light Manufactoring, Construction and Forest Resources";
            //        break;

            //    case "5":
            //        report.Source = "Oil and Gas";
            //        break;

            //    case "6":
            //        report.Source = "Transport";
            //        break;

            //    case "7":
            //        report.Source = "Waste";
            //        break;

            //    case "8":
            //        report.Source = "Total";
            //        break;

            //    default:
            //        Console.WriteLine("Invalid Source Menu Selection.");
            //        break;
            //}
        }

        private static List<string> GetSourceList()
        {
            List<string> sources = new();

            try
            {
                XmlDocument doc = new();
                doc.Load(XmlFile);

                XPathNavigator navigator = doc.CreateNavigator()!;
                string sourceXPath = "//source";
                XPathNodeIterator sourceIterator = navigator.Select(sourceXPath);

                while (sourceIterator.MoveNext())
                {
                    XPathNavigator sourceNode = sourceIterator.Current!;
                    string source = sourceNode.GetAttribute("description", "");

                    if (!sources.Contains(source))
                        sources.Add(source);
                    else
                        break;
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (XPathException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            return sources;
        }

        private static Dictionary<string, List<string>> GenerateRegionReport()
        {
            Dictionary<string, List<string>> emissionsBySource = new();

            try
            {
                XmlDocument doc = new();
                doc.Load(XmlFile);

                //XPathNavigator navigator = doc.CreateNavigator()!;
                //string xpath = $"//region[@name='{report.Region}']/source/emissions[@year>={report.StartingYear} and @year<={report.EndingYear}]";

                //XPathNodeIterator iterator = navigator.Select(xpath);
                //while (iterator.MoveNext())
                //{
                //    XPathNavigator emissionNode = iterator.Current!;
                //    string key = emissionNode.SelectSingleNode("parent::source/@description")!.Value;
                //    string value = emissionNode.Value;

                //    if (emissionsBySource.ContainsKey(key))
                //        emissionsBySource[key].Add(value);
                //    else
                //        emissionsBySource.Add(key, new List<string> { value });
                //}

                XPathNavigator navigator = doc.CreateNavigator()!;
                string regionXPath = $"//region[@name='{report.Region}']";
                XPathNodeIterator regionIterator = navigator.Select(regionXPath);

                while (regionIterator.MoveNext())
                {
                    XPathNavigator regionNode = regionIterator.Current!;

                    XPathNodeIterator sourceIterator = regionNode.Select("source");
                    while (sourceIterator.MoveNext())
                    {
                        XPathNavigator sourceNode = sourceIterator.Current!;
                        string sourceDescription = sourceNode.GetAttribute("description", "");

                        List<string> emissions = new List<string>();

                        for (int year = int.Parse(report.StartingYear); year <= int.Parse(report.EndingYear); year++)
                        {
                            string emissionXPath = $".//emissions[@year='{year}']";
                            XPathNodeIterator emissionIterator = sourceNode.Select(emissionXPath);

                            string value = emissionIterator.MoveNext() ? emissionIterator.Current!.Value : "-";
                            emissions.Add(value);
                        }

                        emissionsBySource.Add(sourceDescription, emissions);
                    }
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (XPathException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            return emissionsBySource;
        }

        private static Dictionary<string, List<string>> GenerateSourceReport()
        {
            Dictionary<string, List<string>> emissionsByRegion = new();

            try
            {
                XmlDocument doc = new();
                doc.Load(XmlFile);

                //XPathNavigator navigator = doc.CreateNavigator()!;
                //string xpath = $"//region/source[@description='{report.Source}']/emissions[@year>={report.StartingYear} and @year<={report.EndingYear}]";

                //XPathNodeIterator iterator = navigator.Select(xpath);
                //while (iterator.MoveNext())
                //{
                //    XPathNavigator emissionNode = iterator.Current!;
                //    string key = emissionNode.SelectSingleNode("ancestor::region/@name")!.Value;
                //    string value = emissionNode.Value;

                //    if (emissionsByRegion.ContainsKey(key))
                //        emissionsByRegion[key].Add(value);
                //    else
                //        emissionsByRegion.Add(key, new List<string> { value });
                //}

                XPathNavigator navigator = doc.CreateNavigator()!;
                string sourceXPath = $"//source[@description='{report.Source}']";
                XPathNodeIterator sourceIterator = navigator.Select(sourceXPath);

                while (sourceIterator.MoveNext())
                {
                    XPathNavigator sourceNode = sourceIterator.Current!;

                    XPathNodeIterator regionIterator = sourceNode.Select("ancestor::region");
                    while (regionIterator.MoveNext())
                    {
                        XPathNavigator regionNode = regionIterator.Current!;
                        string regionName = regionNode.GetAttribute("name", "");

                        List<string> emissions = new List<string>();

                        for (int year = int.Parse(report.StartingYear); year <= int.Parse(report.EndingYear); year++)
                        {
                            string emissionXPath = $"//region[@name='{regionName}']/source[@description='{report.Source}']/emissions[@year='{year}']";
                            XPathNodeIterator emissionIterator = regionNode.Select(emissionXPath);

                            string value = emissionIterator.MoveNext() ? emissionIterator.Current!.Value : "-";
                            emissions.Add(value);
                        }

                        emissionsByRegion.Add(regionName, emissions);
                    }
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (XPathException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            return emissionsByRegion;
        }

        private static void DisplayReport(string type, Dictionary<string, List<string>> emissions)
        {
            DisplayReportHeader(type);

            foreach (KeyValuePair<string, List<string>> kvp in emissions)
            {
                Console.Write(kvp.Key.PadLeft(54));

                foreach (string value in kvp.Value)
                {
                    if (double.TryParse(value, out _))
                        Console.Write(double.Parse(value).ToString("0.00").PadLeft(10));
                    else
                        Console.Write(value.PadLeft(10));
                }

                Console.WriteLine();
            }

            Console.WriteLine(lineSeparator + "\n\n");
        }

        private static void DisplayReportHeader(string type)
        {
            Console.WriteLine(lineSeparator);
            Console.WriteLine("".PadLeft(30) + $"Greenhouse Gas Emissions (Megatonnes) ~ {(type == "Region" ? report.Region : report.Source)}");
            Console.WriteLine(lineSeparator);

            Console.Write(type == "Region" ? "Source".PadLeft(54) : "Region".PadLeft(54));

            for (int i = int.Parse(report.StartingYear); i <= int.Parse(report.EndingYear); i++)
                Console.Write(i.ToString().PadLeft(10));

            Console.WriteLine("\n" + "------".PadLeft(54) + "----".PadLeft(10) + "----".PadLeft(10) + "----".PadLeft(10) + "----".PadLeft(10) + "----".PadLeft(10));
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
                    invalid = false;
                else
                    Console.WriteLine($"ERROR: Input must be an integer between {minValue} and {maxValue}");
            }

            return year!;
        }

        private static void ClearConsole()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            //just clearing the console doesn't clear everything if there's a scrollbar (hidden content)
            //the following line fixes the scrollbar issue (for some reason)
            Console.WriteLine("\x1b[3J");
        }
    }
}