using System;
using System.Collections.Generic;
using System.Text;

namespace AirTickets.BL
{
    public class AirTicketsSettings : IAirTicketsSettingsService
    {
        public string ProjectPath { get; set; }
        public string InputXmlPath { get; set; }
        public string OutputHtmlPath { get; set; }
    }
}
