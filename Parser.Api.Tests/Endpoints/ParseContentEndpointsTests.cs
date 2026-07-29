using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Parser.Tests.Endpoints;

public class ParseContentEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ParseContentEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ParseContent_Csv_ReturnsParsedObjects()
    {
        var csv = """
            name,age,city
            Alice,30,London
            Bob,25,Paris
            """;

        var request = new
        {
            type = "CSV",
            content = Convert.ToBase64String(Encoding.UTF8.GetBytes(csv)),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();

        Assert.Contains("Alice", body);
        Assert.Contains("London", body);
        Assert.Contains("Bob", body);
        Assert.Contains("Paris", body);
    }

    [Fact]
    public async Task ParseContent_InternalJson_ReturnsParsedObjects()
    {
        var json = """
            [
              {
                "string": "Home",
                "number": 3,
                "decimal": 3.99,
                "boolean": true,
                "date": "2023-08-17T08:20:28.438Z"
              }
            ]
            """;

        var request = new
        {
            type = "INTERNAL_JSON",
            content = Convert.ToBase64String(Encoding.UTF8.GetBytes(json)),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();

        Assert.Contains("Home", body);
        Assert.Contains("3.99", body);
        Assert.Contains("true", body);
        Assert.Contains("2023-08-17", body);
    }

    [Fact]
    public async Task ParseContent_InvalidBase64_ReturnsBadRequest()
    {
        var request = new { type = "CSV", content = "invalid-base64" };

        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ParseContent_MissingContent_ReturnsBadRequest()
    {
        var request = new { type = "CSV" };

        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ParseContent_InvalidType_ReturnsBadRequest()
    {
        var request = new
        {
            type = "XML",
            content = Convert.ToBase64String(Encoding.UTF8.GetBytes("test")),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ParseContent_InternalJsonObjectInsteadOfArray_ReturnsBadRequest()
    {
        var json = """
            {
              "name": "Alice"
            }
            """;

        var request = new
        {
            type = "INTERNAL_JSON",
            content = Convert.ToBase64String(Encoding.UTF8.GetBytes(json)),
        };

        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
