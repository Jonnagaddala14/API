using RandomApi.Models;
using RandomApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton(Random.Shared);
builder.Services.AddSingleton<IRandomDataService, RandomDataService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

var api = app.MapGroup("/api");

api.MapGet("/users", (IRandomDataService data, int? count) =>
{
    var n = count ?? 5;
    if (n is < 1 or > 100)
    {
        return Results.BadRequest(new { error = "count must be between 1 and 100." });
    }

    return Results.Ok(data.GetUsers(n));
});

api.MapGet("/users/{id:int}", (IRandomDataService data, int id) =>
    id < 1 ? Results.NotFound() : Results.Ok(data.GetUser(id)));

api.MapGet("/quote", (IRandomDataService data) => Results.Ok(data.GetQuote()));

api.MapGet("/weather/{city}", (IRandomDataService data, string city) =>
    Results.Ok(data.GetWeather(city)));

api.MapPost("/echo", (EchoRequest request) =>
    string.IsNullOrWhiteSpace(request.Message)
        ? Results.BadRequest(new { error = "message is required." })
        : Results.Ok(new EchoResponse(request.Message, request.Message.Length, Guid.NewGuid(), DateTimeOffset.UtcNow)));

app.Run();

public partial class Program;
