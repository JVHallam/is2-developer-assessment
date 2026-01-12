using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using DataExporter;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Net.Mime;
using System.Text;

namespace DataExporter.Tests.Integration;

public class ApiTests
    : IClassFixture<WebApplicationFactory<App>>
{
    private readonly WebApplicationFactory<App> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiTests(WebApplicationFactory<App> factory)
    {
        _factory = factory;
        _client = factory.CreateDefaultClient();

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
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

    [Fact]
    public async Task GivenExistingPolicy_WhenGetPolicyCalled_ThenReturnsExpectedPolicy()
    {
        //Arrange
        var policyId = 1;
        var url = $"/Policies/{policyId}";

        //Act
        var result = await _client.GetAsync(url);

        //Assert
        var bodyString = await result.Content.ReadAsStringAsync();

        var bodyContent = JsonSerializer.Deserialize<ReadPolicy>(bodyString, _serializerOptions);

        //Turn that into the expected result body
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(bodyContent);

        Assert.Equal(policyId, bodyContent.Id);
        Assert.Equal("HSCX1001", bodyContent.PolicyNumber);
        Assert.Equal(200, bodyContent.Premium);
        Assert.Equal(DateTime.Parse("2024-04-01T00:00:00"), bodyContent.StartDate);
    }

    [Fact]
    public async Task GivenExistingPolicies_WhenGetPoliciesCalled_ThenReturnsExpectedCount()
    {
        //Arrange
        var url = "/Policies";

        //Act
        var result = await _client.GetAsync(url);

        //Assert
        var bodyString = await result.Content.ReadAsStringAsync();

        var bodyContent = JsonSerializer.Deserialize<List<ReadPolicy>>(bodyString, _serializerOptions);

        //Turn that into the expected result body
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(bodyContent);

        Assert.Equal(5, bodyContent.Count);
    }

    [Fact]
    public async Task GivenANewPolicy_WhenPostPoliciesCalled_ThenReturnsExpectedPolicy()
    {
        //Arrange
        var url = "/Policies";
        var request = new PostPolicy()
        {
            PolicyNumber = "Test123",
            Premium = 200,
            StartDate = "2024-04-01T00:00:00"
        };

        var requestAsJson = JsonSerializer.Serialize(request);

        var content = new StringContent(
            requestAsJson,
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        //Act
        var result = await _client.PostAsync(url, content);

        //Assert
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        //Assert the header
        var hasLocationHeader = result.Headers.TryGetValues("Location", out var locationHeaders);
        var locationHeader = locationHeaders.First();
        Assert.True(hasLocationHeader);
        Assert.Equal("/Policies/6",  locationHeader);

        //Assert the returned values
        var bodyString = await result.Content.ReadAsStringAsync();
        var bodyContent = JsonSerializer.Deserialize<ReadPolicy>(bodyString, _serializerOptions);

        Assert.Equal(6, bodyContent.Id);
        Assert.Equal(request.PolicyNumber, bodyContent.PolicyNumber);
        Assert.Equal(request.Premium, bodyContent.Premium);
        Assert.Equal(DateTime.Parse(request.StartDate), bodyContent.StartDate);
    }

    [Fact]
    public async Task GivenTwoDates_WhenExportDataCalled_ReturnsPoliciesAndNotes()
    {
        //Arrange
        var startDate = "2024-04-01";
        var endDate = "2024-04-05";

        var url = $"/policies/export?startDate={startDate}&endDate={endDate}";

        //Act
        var result = await _client.GetAsync(url);

        //Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        //Assert the returned values
        var bodyString = await result.Content.ReadAsStringAsync();
        var bodyContent = JsonSerializer.Deserialize<List<ExportDto>>(bodyString, _serializerOptions);

        Assert.Equal(3, bodyContent.Count);

        //1
        Assert.Equal("HSCX1001", bodyContent[0].PolicyNumber);
        Assert.Equal(200, bodyContent[0].Premium);
        Assert.Equal(new DateTime(2024, 4, 1), bodyContent[0].StartDate);
        Assert.Equal(2, bodyContent[0].Notes.Count);
        Assert.Equal("This is a test note", bodyContent[0].Notes[0]);
        Assert.Equal("This is a second note", bodyContent[0].Notes[1]);

        //2
        Assert.Equal("HSCX1002", bodyContent[1].PolicyNumber);
        Assert.Equal(153, bodyContent[1].Premium);
        Assert.Equal(new DateTime(2024, 4, 5), bodyContent[1].StartDate);
        Assert.Equal(2, bodyContent[1].Notes.Count);
        Assert.Equal("This is a test note", bodyContent[1].Notes[0]);
        Assert.Equal("This is a second note", bodyContent[1].Notes[1]);

        //5
        Assert.Equal("HSCX1005", bodyContent[2].PolicyNumber);
        Assert.Equal(100, bodyContent[2].Premium);
        Assert.Equal(new DateTime(2024, 4, 1), bodyContent[2].StartDate);
        Assert.Equal(2, bodyContent[2].Notes.Count);
        Assert.Equal("This is a test note", bodyContent[2].Notes[0]);
        Assert.Equal("This is a second note", bodyContent[2].Notes[1]);
    }
}
