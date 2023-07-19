using System.Xml;
using System.Xml.XPath;
using static GHGEmission.Program;

namespace GHGEmission
{
	public static class ReportDirector
	{
        public static Dictionary<string, List<string>> GenerateRegionReport()
        {
            Dictionary<string, List<string>> emissionsBySource = new();

            try
            {
                XmlDocument doc = new();
                doc.Load(XmlFile);

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

        public static Dictionary<string, List<string>> GenerateSourceReport()
        {
            Dictionary<string, List<string>> emissionsByRegion = new();

            try
            {
                XmlDocument doc = new();
                doc.Load(XmlFile);

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

        public static void DisplayReport(string type, Dictionary<string, List<string>> emissions)
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

            Console.Write("\n" + "------".PadLeft(54));

            for (int i = int.Parse(report.StartingYear); i <= int.Parse(report.EndingYear); i++)
                Console.Write("----".PadLeft(10));

            Console.WriteLine();
        }
    }
}

