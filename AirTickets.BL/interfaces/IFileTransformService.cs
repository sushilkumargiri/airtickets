using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicket.BL
{
    public interface IFileTransformService
    {
        Task<List<OutputFileModel>> ProcessXml();
        string TransformXMLToHTML(string inputXml, string xsltString);
    }
}
