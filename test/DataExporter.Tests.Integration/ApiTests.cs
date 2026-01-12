using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using DataExporter;
using System.Net.Http;
using System.Net;

namespace DataExporter.Tests.Integration;

public class ApiTests
    : IClassFixture<WebApplicationFactory<App>>
{
    private readonly WebApplicationFactory<App> _factory;
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<App> factory)
    {
        _factory = factory;
        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task HealthCheck_Get_Success()
    {
        //Arrange
        var url = "/healthcheck";

        //Act
        var result = await _client.GetAsync(url);

        //Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}
