using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.DL
{
    public class FileWriteRepository : IFileWriteRepository
    {
        public async Task<bool> WriteHtmlFile(string htmlStrings, string filePath, string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(Path.Combine(filePath , fileName), FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        await w.WriteLineAsync(htmlStrings);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
