namespace RandomApi.Models;

public record User(int Id, string Name, string Email, int Age, bool IsActive);

public record Quote(string Text, string Author);

public record WeatherReport(string City, int TemperatureC, string Summary, int HumidityPercent, DateTimeOffset ObservedAt);

public record EchoRequest(string? Message);

public record EchoResponse(string Message, int Length, Guid RequestId, DateTimeOffset ReceivedAt);
