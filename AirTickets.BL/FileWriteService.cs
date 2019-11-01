using AirTicket.DL;
using AirTickets.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.BL
{
    public class FileWriteService : IFileWriteService
    {
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IAirTicketsSettingsService _airTicketsSettings;
        public FileWriteService(IFileWriteRepository fileWriteRepository, IAirTicketsSettingsService airTicketsSettings)
        {
            _fileWriteRepository = fileWriteRepository;
            _airTicketsSettings = airTicketsSettings;
        }
        public async Task<bool> WriteHtmlFile(string htmlData, string fileName)
        {
            return await _fileWriteRepository.WriteHtmlFile(htmlData, _airTicketsSettings.OutputHtmlPath, fileName);
        }
    }
}
