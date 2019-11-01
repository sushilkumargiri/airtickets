using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.DL
{
    public interface IFileWriteRepository
    {
        Task<bool> WriteHtmlFile(string htmlStrings,string filePath, string fileName);
    }
}
