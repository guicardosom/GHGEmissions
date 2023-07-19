/*
 * Program:     GHGEmissions.exe
 * Module:      DataPersistence.cs
 * Course:      INFO-3138
 * Coder:       Gui Miranda
 * Date:        July 19, 2023
 * Description: A static class responsible for persisting data.
 */

using System.Xml;
using static GHGEmission.Program;

namespace GHGEmission
{
    public static class DataPersistence
    {
        public static void SaveDataToXml(string filePath, EmissionsReport reportConfig)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("EmissionsReport");
            doc.AppendChild(root);

            XmlElement startingYearElement = doc.CreateElement("StartingYear");
            startingYearElement.InnerText = report.StartingYear;
            root.AppendChild(startingYearElement);

            XmlElement endingYearElement = doc.CreateElement("EndingYear");
            endingYearElement.InnerText = report.EndingYear;
            root.AppendChild(endingYearElement);

            XmlElement regionElement = doc.CreateElement("Region");
            regionElement.InnerText = report.Region;
            root.AppendChild(regionElement);

            XmlElement sourceElement = doc.CreateElement("Source");
            sourceElement.InnerText = report.Source;
            root.AppendChild(sourceElement);

            doc.Save(filePath);
        }

        public static EmissionsReport LoadDataFromXml(string filePath)
        {
            EmissionsReport report = EmissionsReport.GetInstance();

            if (File.Exists(filePath))
            {
                try
                {
                    XmlDocument doc = new();
                    doc.Load(filePath);

                    XmlNode root = doc.DocumentElement!;
                    if (root != null)
                    {
                        XmlNode? startingYearNode = root.SelectSingleNode("StartingYear");
                        if (startingYearNode != null)
                            report.StartingYear = startingYearNode.InnerText;

                        XmlNode? endingYearNode = root.SelectSingleNode("EndingYear");
                        if (endingYearNode != null)
                            report.EndingYear = endingYearNode.InnerText;

                        XmlNode? regionNode = root.SelectSingleNode("Region");
                        if (regionNode != null)
                            report.Region = regionNode.InnerText;

                        XmlNode? sourceNode = root.SelectSingleNode("Source");
                        if (sourceNode != null)
                            report.Source = sourceNode.InnerText;
                    }
                }
                catch (Exception)
                {
                    //if any error occurs just return the default EmissionsReport object
                    return report;
                }
            }

            return report;
        }
    }
}