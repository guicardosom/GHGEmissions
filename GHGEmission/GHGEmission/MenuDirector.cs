using System.Xml;
using System.Xml.XPath;
using static GHGEmissions.Program;

namespace GHGEmission
{
	public static class MenuDirector
	{
        public static void DisplayMenu(string title)
        {
            Console.WriteLine(lineSeparator);
            Console.WriteLine("".PadLeft(30) + $"Greenhouse Gas Emissions in Canada ~ {title}");
            Console.WriteLine(lineSeparator);

            switch (title)
            {
                case "Main Menu":
                    Console.WriteLine($"  (1) To adjust the range of years (currently {report.StartingYear} to {report.EndingYear})");
                    Console.WriteLine($"  (2) To select a different region (currently {report.Region})");
                    Console.WriteLine($"  (3) To select a different GHG source (currently {report.Source})");
                    Console.WriteLine($"  (4) To learn about {report.Region}'s emissions");
                    Console.WriteLine($"  (5) To learn about the {report.Source} sector's emissions");
                    Console.WriteLine($"  (X) To quit");
                    break;

                case "Region Selection":
                    List<string> regions = GetRegionList();

                    for (int i = 0; i < regions.Count; i++)
                        Console.WriteLine($"  ({i + 1}) {regions[i]}");

                    break;

                case "Source Selection":
                    List<string> sources = GetSourceList();

                    for (int i = 0; i < sources.Count; i++)
                        Console.WriteLine($"  ({i + 1}) {sources[i]}");

                    break;

                default:
                    Console.WriteLine("Invalid Menu Title.");
                    break;
            }

            Console.WriteLine(lineSeparator + "\n\n");
        }

        public static void MainMenuListener()
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
                    Dictionary<string, List<string>> emissionsBySource = ReportDirector.GenerateRegionReport();
                    ReportDirector.DisplayReport("Region", emissionsBySource);
                    break;

                case "5":
                    Console.Clear();
                    Dictionary<string, List<string>> emissionsByRegion = ReportDirector.GenerateSourceReport();
                    ReportDirector.DisplayReport("Source", emissionsByRegion);
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

            List<string> regions = GetRegionList();
            if (int.Parse(input) >= 0 && int.Parse(input) < regions.Count)
            {
                for (int i = 0; i < regions.Count; i++)
                {
                    if (int.Parse(input) == i + 1)
                        report.Region = regions[i];
                }
            }
            else
                Console.WriteLine("Invalid Region Menu Selection.");
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

            return regions;
        }

        private static void SourceListener()
        {
            Console.Write("Enter a source #: ");
            string input = Console.ReadLine() ?? "";

            List<string> sources = GetSourceList();
            if (int.Parse(input) >= 0 && int.Parse(input) < sources.Count)
            {
                for (int i = 0; i < sources.Count; i++)
                {
                    if (int.Parse(input) == i + 1)
                        report.Source = sources[i];
                }
            }
            else
                Console.WriteLine("Invalid Source Menu Selection.");
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

        public static void ClearConsole()
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

