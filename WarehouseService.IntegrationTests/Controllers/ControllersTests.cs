using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace WarehouseService.IntegrationTests.Controllers
{
    public class ControllersTests : IClassFixture<TestWebApplicationFactory<API.Program>>
    {
        private readonly HttpClient client;
        private readonly TestWebApplicationFactory<API.Program> factory;

        public ControllersTests(TestWebApplicationFactory<API.Program> factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/api/product/all")]
        [InlineData("/api/category/all")]
        [InlineData("/api/order/all")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            Assert.Equal(MediaTypeNames.Application.Json, response.Content.Headers.ContentType.MediaType);
        }
    }
}
