using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.BL
{
    public interface IFileWriteService
    {
        Task<bool> WriteHtmlFile(string htmllData,string fileName);
    }
}
