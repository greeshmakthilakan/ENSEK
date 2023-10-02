using ENSEK.Controllers;
using ENSEK.Services.CSVServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK.Tests.Controllers
{
    [TestClass]
    public class ENSEKControllerTests
    {
        private readonly ICSVService _csvService;

        public ENSEKControllerTests()
        {
        }
        [Fact]
        public async Task ENSEKController_AddMeterReading_ReturnsValidType()
        {
            //Arrange
            
            var content = "Some content";
            var fileName = "mock.csv";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFileCollection files = new FormFileCollection();
            IFormFile file = new FormFile(stream, 0, stream.Length, "formFiles", fileName);
            files.Append(file);
            ENSEKController sut = new ENSEKController(_csvService);

            //Act
            var result = await sut.AddMeterReading(files);

            //Assert

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(IActionResult));

        }
    }
}
