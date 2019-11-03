using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirTicket.BL;
using AirTickets.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AirTickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IFileReadService _fileReadService;
        private readonly IAirTicketsSettingsService _airTicketsSettings;


        public HomeController(IFileReadService fileReadService, IAirTicketsSettingsService airTicketsSettings)
        {
            _fileReadService = fileReadService;
            _airTicketsSettings = airTicketsSettings;
        }

        [HttpGet]
        public IEnumerable<OutputFileModel> Get()
        {
            var xmlFiles = _fileReadService.GetFile(Path.Combine(_airTicketsSettings.ProjectPath, "Data/Computers"), "*.xml");
            return _fileReadService.GetFiles(xmlFiles, Path.Combine(_airTicketsSettings.ProjectPath, "Data\\Computers\\"));
        }

        [HttpPost]
        public string Test()
        {
            return "success";
        }

    }
}
