/*
 * Program:     GHGEmissions.exe
 * Module:      Program.cs
 * Course:      INFO-3138
 * Coder:       Gui Miranda
 * Date:        July 11, 2023
 * Description: A C# console (.NET Core) application that uses an XML file for data storage and generates parameterized 
 * reports based on based on a selected region (province or territory) or based on a selected source of GHGs. 
 */

namespace GHGEmission
{
    internal class Program
    {
        public const string XmlFile = @"../../../data/ghg-canada.xml";
        public const string ConfigFile = @"../../../data/report-config.xml";
        public const string lineSeparator = "----------------------------------------------------------------------------------------------------------";

        public static EmissionsReport report = DataPersistence.LoadDataFromXml(ConfigFile);

        static void Main()
        {
            while (true)
            {
                MenuDirector.DisplayMenu("Main Menu");
                MenuDirector.MainMenuListener();
                MenuDirector.ClearConsole();
            }
        }
    }
}