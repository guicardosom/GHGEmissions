/*
 * Program:     GHGEmissions.exe
 * Module:      EmissionsReport.cs
 * Course:      INFO-3138
 * Coder:       Gui Miranda
 * Date:        July 19, 2023
 * Description: A singleton responsible for holding the report's configuration.
 */

namespace GHGEmission
{
	public class EmissionsReport
	{
        public static EmissionsReport? _instance;

        public string StartingYear { get; set; }
        public string EndingYear { get; set; }
        public string Region { get; set; }
        public string Source { get; set; }

        private EmissionsReport()
        {
            StartingYear = "2015";
            EndingYear = "2019";
            Region = "Canada";
            Source = "Oil and Gas";
        }

        public static EmissionsReport GetInstance()
        {
            if (_instance == null)
                _instance = new EmissionsReport();

            return _instance;
        }
    }
}

