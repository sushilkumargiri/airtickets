using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AirTicket.BL;
using AirTickets.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AirTickets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutputDataController : ControllerBase
    {
        private readonly ILogger<OutputDataController> _logger;
        private readonly IFileTransformService _fileTransformService;
        public OutputDataController(ILogger<OutputDataController> logger
            , IFileTransformService fileTransformService)
        {
            _logger = logger;
            _fileTransformService = fileTransformService;
        }

        [HttpGet]
        public async Task<IEnumerable<OutputFileModel>> Get()
        {
            return await _fileTransformService.ProcessXml().ConfigureAwait(false);
        }

    }
}
