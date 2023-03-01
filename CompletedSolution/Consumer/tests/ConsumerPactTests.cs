using Pactify;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class ConsumerPactTests
    {
        private readonly PactDefinitionOptions _options;

        public ConsumerPactTests()
        {
            _options = new PactDefinitionOptions
            {
                IgnoreCasing = true,
                IgnoreContractValues = true
            };
        }

        [Fact]
        public async Task ItHandlesInvalidDateParam()
        {
            // Arrange
            var invalidRequestMessage = "validDateTime is not a date or time";
            await PactMaker
                .Create(_options)
                .Between("consumer", "provider")
                .WithHttpInteraction(b => b
                    .Given("There is data")
                    .UponReceiving("An invalid GET request for Date Validation with invalid date parameter")
                    .With(req => req
                        .WithMethod(HttpMethod.Get)
                        .WithPath("/api/provider?validDateTime=lolz"))
                    .WillRespondWith(resp => resp
                        .WithStatusCode(HttpStatusCode.BadRequest)
                        .WithHeader("Content-Type", "application/json; charset=utf-8")
                        .WithBody(invalidRequestMessage)))
                    .PublishedViaHttp("https://simcorpdm.pactflow.io/", HttpMethod.Post, "f3ZXi7mzYmbJXIks3dJnzw")
                    .MakeAsync();


            //// Act
            //var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("lolz", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            //var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            //// Assert
            //Assert.Contains(invalidRequestMessage, resultBodyText);
        }

        //[Fact]
        //public void ItHandlesEmptyDateParam()
        //{
        //    // Arrange
        //    var invalidRequestMessage = "validDateTime is required";
        //    _mockProviderService.Given("There is data")
        //                        .UponReceiving("A invalid GET request for Date Validation with empty string date parameter")
        //                        .With(new ProviderServiceRequest
        //                        {
        //                            Method = HttpVerb.Get,
        //                            Path = "/api/provider",
        //                            Query = "validDateTime="
        //                        })
        //                        .WillRespondWith(new ProviderServiceResponse
        //                        {
        //                            Status = 400,
        //                            Headers = new Dictionary<string, object>
        //                            {
        //                                { "Content-Type", "application/json; charset=utf-8" }
        //                            },
        //                            Body = new
        //                            {
        //                                message = invalidRequestMessage
        //                            }
        //                        });

        //    // Act
        //    var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi(String.Empty, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
        //    var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        //    // Assert
        //    Assert.Contains(invalidRequestMessage, resultBodyText);
        //}

        //[Fact]
        //public void ItHandlesNoData()
        //{
        //    // Arrange
        //    _mockProviderService.Given("There is no data")
        //                        .UponReceiving("A valid GET request for Date Validation")
        //                        .With(new ProviderServiceRequest
        //                        {
        //                            Method = HttpVerb.Get,
        //                            Path = "/api/provider",
        //                            Query = "validDateTime=04/04/2018"
        //                        })
        //                        .WillRespondWith(new ProviderServiceResponse
        //                        {
        //                            Status = 404
        //                        });

        //    // Act
        //    var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("04/04/2018", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
        //    var resultStatus = (int)result.StatusCode;

        //    // Assert
        //    Assert.Equal(404, resultStatus);
        //}

        //[Fact]
        //public void ItParsesADateCorrectly()
        //{
        //    var expectedDateString = "04/05/2018";
        //    var expectedDateParsed = DateTime.Parse(expectedDateString).ToString("dd-MM-yyyy HH:mm:ss");

        //    // Arrange
        //    _mockProviderService.Given("There is data")
        //                        .UponReceiving("A valid GET request for Date Validation")
        //                        .With(new ProviderServiceRequest
        //                        {
        //                            Method = HttpVerb.Get,
        //                            Path = "/api/provider",
        //                            Query = $"validDateTime={expectedDateString}"
        //                        })
        //                        .WillRespondWith(new ProviderServiceResponse
        //                        {
        //                            Status = 200,
        //                            Headers = new Dictionary<string, object>
        //                            {
        //                                { "Content-Type", "application/json; charset=utf-8" }
        //                            },
        //                            Body = new
        //                            {
        //                                test = "NO",
        //                                validDateTime = expectedDateParsed
        //                            }
        //                        });

        //    // Act
        //    var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi(expectedDateString, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
        //    var resultBody = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        //    // Assert
        //    Assert.Contains(expectedDateParsed, resultBody);
        //}
    }
}
