using AirTickets.BL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace AirTicket.BL
{
    public class FileTransformService: IFileTransformService
    {
        public ConcurrentQueue<string> HtmlOutputs { get; set; }

        private readonly IAirTicketsSettingsService _airTicketsSettings;
        private readonly IFileReadService _fileReadService;
        private readonly IFileWriteService _fileWriteService;
        public FileTransformService(IAirTicketsSettingsService airTicketsSettings
            , IFileReadService fileReadService, IFileWriteService fileWriteService)
        {
            _airTicketsSettings = airTicketsSettings;
            _fileReadService = fileReadService;
            _fileWriteService = fileWriteService;
        }
        public async Task<List<OutputFileModel>> ProcessXml()
        {
            HtmlOutputs = new ConcurrentQueue<string>();

            //create output directory if it does not exist
            Directory.CreateDirectory(_airTicketsSettings.OutputHtmlPath);

            //Read xml and write to html in 2 separate async tasks
            await EnqueueHtmls().ConfigureAwait(false);
            await DequeueAndWriteHtmls().ConfigureAwait(false);

            //return converted html files
            var htmlFiles = _fileReadService.GetXmlFile(_airTicketsSettings.OutputHtmlPath, "*.html");
            return _fileReadService.GetFiles(htmlFiles, _airTicketsSettings.OutputHtmlPath);
        }

        public string TransformXMLToHTML(string inputXml, string xsltString)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            using (XmlReader reader = XmlReader.Create(new StringReader(xsltString), settings))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }

        private async Task DequeueAndWriteHtmls()
        {
            int i = 1;
            string htmlString;
            while (HtmlOutputs.TryDequeue(out htmlString))
            {
                _ = await _fileWriteService.WriteHtmlFile(htmlString, i++.ToString() + ".html").ConfigureAwait(false);
            }
        }

        private async Task EnqueueHtmls()
        {
            var xmlData = await _fileReadService.ReadXmlFile().ConfigureAwait(false);
            var xsltData = await _fileReadService.ReadXsltFile().ConfigureAwait(false);
            foreach (var strData in xmlData)
            {
                var htmlData = TransformXMLToHTML(strData, xsltData);
                HtmlOutputs.Enqueue(htmlData);
            }
        }
    }
}
