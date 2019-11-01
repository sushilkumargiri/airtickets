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
    public class FileReadService : IFileReadService
    {
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IAirTicketsSettingsService _airTicketsSettings;
        public FileReadService(IFileReadRepository fileReadRepository, IAirTicketsSettingsService airTicketsSettings)
        {
            _fileReadRepository = fileReadRepository;
            _airTicketsSettings = airTicketsSettings;
        }

        public List<OutputFileModel> GetFiles(FileInfo[] files, string path)
        {
            List<OutputFileModel> objFiles = new List<OutputFileModel>();
            int i = 0;
            foreach (FileInfo file in files)
            {
                objFiles.Add(new OutputFileModel()
                {
                    ID = i++,
                    FileName = file.Name,
                    FilePath = Path.Combine(path, file.Name)
                });
            }
            return objFiles;
        }

        public FileInfo[] GetXmlFile(string folder, string ext)
        {
            DirectoryInfo d = new DirectoryInfo(folder);
            return d.GetFiles(ext);
        }

        public async Task<List<string>> ReadXmlFile()
        {
            var xmlDataStr = await _fileReadRepository.ReadXmlFile(_airTicketsSettings.InputXmlPath);
            return xmlDataStr;
        }

        public async Task<string> ReadXsltFile()
        {
            var xsltDataStr = await _fileReadRepository.ReadXsltFile(Path.Combine(_airTicketsSettings.ProjectPath, "Resources/Computer.xslt"));
            return xsltDataStr;
        }
    }
}
