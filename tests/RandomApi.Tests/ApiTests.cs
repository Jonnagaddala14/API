using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RandomApi.Models;

namespace RandomApi.Tests;

public class ApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Users_DefaultsToFive()
    {
        var users = await _client.GetFromJsonAsync<List<User>>("/api/users");
        Assert.NotNull(users);
        Assert.Equal(5, users.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task Users_OutOfRangeCount_ReturnsBadRequest(int count)
    {
        var response = await _client.GetAsync($"/api/users?count={count}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UserById_ReturnsRequestedId()
    {
        var user = await _client.GetFromJsonAsync<User>("/api/users/42");
        Assert.Equal(42, user!.Id);
    }

    [Fact]
    public async Task Weather_EchoesCity()
    {
        var report = await _client.GetFromJsonAsync<WeatherReport>("/api/weather/Hyderabad");
        Assert.Equal("Hyderabad", report!.City);
    }

    [Fact]
    public async Task Products_ReturnsRequestedPage()
    {
        var result = await _client.GetFromJsonAsync<PagedResult<Product>>("/api/products?page=2&pageSize=5");
        Assert.NotNull(result);
        Assert.Equal(2, result.Page);
        Assert.Equal(5, result.Items.Count);
        Assert.Equal(6, result.Items[0].Id);
    }

    [Fact]
    public async Task Products_PastLastPage_ReturnsEmptyItems()
    {
        var result = await _client.GetFromJsonAsync<PagedResult<Product>>("/api/products?page=100&pageSize=10");
        Assert.Empty(result!.Items);
    }

    [Theory]
    [InlineData("page=0")]
    [InlineData("pageSize=51")]
    public async Task Products_InvalidPaging_ReturnsBadRequest(string query)
    {
        var response = await _client.GetAsync($"/api/products?{query}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ProductById_ReturnsRequestedId()
    {
        var product = await _client.GetFromJsonAsync<Product>("/api/products/12");
        Assert.Equal(12, product!.Id);
    }

    [Fact]
    public async Task Echo_ReturnsMessageAndLength()
    {
        var response = await _client.PostAsJsonAsync("/api/echo", new EchoRequest("hello"));
        var body = await response.Content.ReadFromJsonAsync<EchoResponse>();
        Assert.Equal("hello", body!.Message);
        Assert.Equal(5, body.Length);
    }

    [Fact]
    public async Task Echo_EmptyMessage_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/echo", new EchoRequest(""));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
