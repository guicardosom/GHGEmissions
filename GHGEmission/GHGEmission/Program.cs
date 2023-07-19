/*
 * Program:     GHGEmissions.exe
 * Module:      Program.cs
 * Course:      INFO-3138
 * Date:        July 11, 2023
 * Description: todo -> A sample solution to the DOM coding exercise involving driving directions.
 *              This program uses the DOM to complete a set of driving directions contained in the XML file
 *              "directions.xml". You will find this file nested within the project's bin\Debug folder.
 */

using GHGEmission;

namespace GHGEmissions
{
    internal class Program
    {
        public const string XmlFile = @"../../../data/ghg-canada.xml";
        public const string lineSeparator = "----------------------------------------------------------------------------------------------------------";

        public static EmissionsReport report = EmissionsReport.GetInstance();

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