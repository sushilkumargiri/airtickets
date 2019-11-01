using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.BL
{
    public interface IFileReadService
    {
        FileInfo[] GetXmlFile(string folder, string ext);
        Task<List<string>> ReadXmlFile();
        Task<string> ReadXsltFile();
        List<OutputFileModel> GetFiles(FileInfo[] files, string path);
    }
}
