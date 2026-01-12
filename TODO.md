Add the xunit web dev project
add the dependencies
FluentAssertions 6.8.0
Microsoft.AspNetCore.Mvc.Testing --version 8.0.0;

using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using JakeTest.Api;
using System.Net.Http;
using System.Net;

namespace JakeTest.Integration.Tests;

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
    public void Compile()
    {
        1.Should().Be(1);
    }

    [Fact]
    public async Task HealthCheck_Get_Success()
    {
        //Arrange
        var url = "/healthcheck";

        //Act
        var result = await _client.GetAsync(url);

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}


app.MapHealthChecks("/healthcheck");
builder.Services.AddHealthChecks();
