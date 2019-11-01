using AirTicket.BL;
using AirTicket.DL;
using AirTickets.BL;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AirTicket.Tests.Business
{
    public class FileReadWriteTest
    {
        private readonly IAirTicketsSettingsService _airTicketsSettings;
        public FileReadWriteTest(IAirTicketsSettingsService airTicketsSettings)
        {
            _airTicketsSettings = airTicketsSettings;
        }
        [Fact]
        public void Read_Xml_Files()
        {
            //init
            var xmlList = new List<string>() { "Xml Data 1", "Xml Data 2", "Xml Data 3" };

            //Setup
            var mockRepository = new Mock<IFileReadRepository>();
            mockRepository.Setup(m => m.ReadXmlFile(_airTicketsSettings.InputXmlPath)).Returns(Task.FromResult(xmlList));
            var fileReadService = new FileReadService(mockRepository.Object, _airTicketsSettings);

            //act
            var returnedXML = fileReadService.ReadXmlFile();

            //assert
            Assert.Equal(3, returnedXML.Result.Count());
        }
        [Fact]
        public void Read_Xslt_File()
        {
            //init
            var xsltStr = "Xslt string";
            var xsltPath = Path.Combine(_airTicketsSettings.ProjectPath, "Resources/Computer.xslt");

            //Setup
            var mockRepository = new Mock<IFileReadRepository>();
            mockRepository.Setup(m => m.ReadXsltFile(xsltPath)).Returns(Task.FromResult(xsltStr));
            var fileReadService = new FileReadService(mockRepository.Object, _airTicketsSettings);

            //act
            var returnedXML = fileReadService.ReadXsltFile();

            //assert
            Assert.Equal("Xslt string", xsltStr);
        }

        [Fact]
        public void Write_Html_File()
        {
            //init
            var xsltStr = "Html string";
            var fileName = "test.html";

            //Setup
            var mockRepository = new Mock<IFileWriteRepository>();
            mockRepository.Setup(m => m.WriteHtmlFile(xsltStr, _airTicketsSettings.OutputHtmlPath, fileName)).Returns(Task.FromResult(true));
            var fileWriteService = new FileWriteService(mockRepository.Object, _airTicketsSettings);

            //act
            var status = fileWriteService.WriteHtmlFile(xsltStr, fileName);

            //assert
            Assert.Equal(true, status.Result);
        }
        [Fact]
		public void Transform_Xml_and_Xslt_to_Html()
        {
			//init
            var xmlString = "<?xml version=\"1.0\" standalone=\"yes\"?><Overview><Assigned><AssignedCount>1</AssignedCount></Assigned></Overview>";
            var xsltString = "<?xml version=\"1.0\" encoding=\"utf - 8\" ?><!DOCTYPE xsl:stylesheet [<!ENTITY nbsp \" &#160;\">]><xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"><xsl:output method=\"html\"/><xsl:template match=\"/\"><HTML>"
            + "<xsl:if test=\"Overview / Assigned / AssignedCount != '0'\">Yes</xsl:if><xsl:if test=\"Overview / Assigned / AssignedCount = '0'\">No</xsl:if>"
				+ "</HTML></xsl:template></xsl:stylesheet>";
            var output = "<HTML>Yes</HTML>";

            //Setup
            var mock = new Mock<IFileTransformService>();
            mock.Setup(m => m.TransformXMLToHTML(xmlString, xsltString)).Returns(()=>It.IsAny<string>());
            var mockFileWriteService = new Mock<IFileWriteService>();
            var mockFileReadService = new Mock<IFileReadService>();

            //act
            var transformedStr = new FileTransformService(_airTicketsSettings, mockFileReadService.Object, mockFileWriteService.Object)
                .TransformXMLToHTML(xmlString, xsltString);

            //assert
			Assert.Equal(transformedStr, output);
        }
    }
}
