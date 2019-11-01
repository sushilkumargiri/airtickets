using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.DL
{
    public class FileReadRepository : IFileReadRepository
    {
        public async Task<List<string>> ReadXmlFile(string path)
        {
            var xmlDataStr = new List<string>();
            foreach (string file in Directory.EnumerateFiles(path, "*.xml"))
            {
                //xmlDataStr.Add(File.ReadAllText(file));
                using (var reader = new StreamReader(file))
                {
                    xmlDataStr.Add(await reader.ReadToEndAsync());
                }
            }
            return xmlDataStr;
        }
        public async  Task<string> ReadXsltFile(string path)
        {
            var xsltDataStr = string.Empty;
            using (var reader = new StreamReader(path))
            {
                xsltDataStr = await reader.ReadToEndAsync();
            }
            //var xsltDataStr = File.ReadAllText(Path.Combine(projectPath, "Resources/Computer.xslt"));

            return xsltDataStr;
        }
    }
}
