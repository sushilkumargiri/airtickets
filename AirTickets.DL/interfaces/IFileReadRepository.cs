using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.DL
{
    public interface IFileReadRepository
    {
        Task<List<string>> ReadXmlFile(string path);
        Task<string> ReadXsltFile(string path);
    }
}
