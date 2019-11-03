using AirTicket;
using AirTicket.BL;
using AirTickets.BL;
using AirTickets.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace AirTicket.Tests.Controllers
{
    public class HomeControllerTest
    {
        private readonly IFileReadService _fileReadService;
        private readonly IAirTicketsSettingsService _airTicketsSettings;

        public HomeControllerTest()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAirTicketsSettingsService, AirTicketsSettings>();
            services.AddTransient<IFileReadService, FileReadService>();

            var serviceProvider = services.BuildServiceProvider();
            _airTicketsSettings = serviceProvider.GetService<IAirTicketsSettingsService>();
        }
        [Fact]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(_fileReadService, _airTicketsSettings);

            // Act
            var result = controller.Test() as string;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("success", result);
        }
    }
}
