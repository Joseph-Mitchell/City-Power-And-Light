using CityPowerAndLight.Models;
using CityPowerAndLight.Services;
using DotNetEnv;
using Moq;
using Moq.Protected;
using System.Text.Json.Serialization;

namespace CityPowerAndLight.Tests.Services
{
    public class ServiceTests
    {
        //Test model for JsonSerializer to serialize response into
        private class TestModel : Model
        {
            [JsonPropertyName("property1")]
            public string Property1 { get; set; }
            [JsonPropertyName("property2")]
            public string Property2 { get; set; }

            public string GetPayload()
            {
                throw new NotImplementedException();
            }
        }

        private readonly Mock<HttpMessageHandler> mockHandler;
        private readonly HttpClient testClient;
        private readonly HttpResponseMessage testResponse;
        private readonly Service<TestModel> mockService;

        public ServiceTests()
        {
            Env.Load();

            //HttpMessageHandlers SendAsync method is mocked
            //as HttpClients PostAsync method cannot be
            mockHandler = new Mock<HttpMessageHandler>();

            testResponse = new HttpResponseMessage();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(testResponse);
            testClient = new HttpClient(mockHandler.Object);

            mockService = new Service<TestModel>(testClient, "testSuffix", "testQuery");
        }

        [Fact]
        public async Task Create_NormalCircumstances_ReturnExpectedTestModel()
        {
            //Arrange
            string testProperty1 = "test1";
            string testProperty2 = "test2";

            testResponse.Content = new StringContent(
                    $"{{" +
                    $"  \"property1\": \"{testProperty1}\"," +
                    $"  \"property2\": \"{testProperty2}\"" +
                    $"}}"
            );
            testResponse.StatusCode = System.Net.HttpStatusCode.Created;

            //Act
            TestModel result = await mockService.Create(new StringContent(""));

            //Assert
            Assert.Equal(testProperty1, result.Property1);
            Assert.Equal(testProperty2, result.Property2);
        }

        [Fact]
        public async Task Create_RespondsFailedStatus_ThrowException()
        {
            //Arrange
            testResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;

            //Assert
            await Assert.ThrowsAsync<Exception>(() => mockService.Create(new StringContent("")));
        }
    }
}
