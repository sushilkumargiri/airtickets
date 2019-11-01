using AirTicket;
using AirTickets.Controllers;
using Xunit;

namespace AirTicket.Tests.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            var result = controller.Test() as string;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("success", result);
        }
    }
}
