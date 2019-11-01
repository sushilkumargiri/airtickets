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
        private readonly ILogger<OutputDataController> _logger;
        private readonly IFileReadService _fileReadService;
        private readonly IFileTransformService _fileTransformService;
        private readonly IFileWriteService _fileWriteService;
        private readonly IAirTicketsSettingsService _airTicketsSettings;

        public HomeController()
        {
        }

        public HomeController(ILogger<OutputDataController> logger
            , IFileReadService fileReadService
            , IFileTransformService fileTransformService
            , IFileWriteService fileWriteService, IAirTicketsSettingsService airTicketsSettings)
        {
            _logger = logger;
            _fileReadService = fileReadService;
            _fileWriteService = fileWriteService;
            _fileTransformService = fileTransformService;
            _airTicketsSettings = airTicketsSettings;
        }

        [HttpGet]
        public IEnumerable<OutputFileModel> Get()
        {
            var xmlFiles = _fileReadService.GetXmlFile(Path.Combine(_airTicketsSettings.ProjectPath, "Data/Computers"), "*.xml");
            return _fileReadService.GetFiles(xmlFiles, Path.Combine(_airTicketsSettings.ProjectPath, "Data\\Computers\\"));
        }

        [HttpGet]
        public string Test()
        {
            return "success";
        }

    }
}
