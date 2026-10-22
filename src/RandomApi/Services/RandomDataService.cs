using RandomApi.Models;

namespace RandomApi.Services;

public interface IRandomDataService
{
    IReadOnlyList<User> GetUsers(int count);
    User GetUser(int id);
    Quote GetQuote();
    WeatherReport GetWeather(string city);
}

public class RandomDataService(Random random) : IRandomDataService
{
    private static readonly string[] FirstNames = ["Asha", "Ravi", "Maria", "Chen", "Liam", "Fatima", "Noah", "Priya"];
    private static readonly string[] LastNames = ["Sharma", "Garcia", "Wang", "Smith", "Khan", "Patel", "Brown", "Rao"];
    private static readonly string[] Summaries = ["Sunny", "Cloudy", "Rainy", "Windy", "Stormy", "Foggy", "Snowy"];
    private static readonly Quote[] Quotes =
    [
        new("Simplicity is prerequisite for reliability.", "Edsger W. Dijkstra"),
        new("Make it work, make it right, make it fast.", "Kent Beck"),
        new("Programs must be written for people to read.", "Harold Abelson"),
        new("First, solve the problem. Then, write the code.", "John Johnson"),
    ];

    public IReadOnlyList<User> GetUsers(int count) =>
        Enumerable.Range(1, count).Select(_ => GetUser(random.Next(1, 10_000))).ToList();

    public User GetUser(int id)
    {
        var first = FirstNames[random.Next(FirstNames.Length)];
        var last = LastNames[random.Next(LastNames.Length)];
        return new User(
            id,
            $"{first} {last}",
            $"{first}.{last}{id}@example.com".ToLowerInvariant(),
            random.Next(18, 80),
            random.Next(2) == 1);
    }

    public Quote GetQuote() => Quotes[random.Next(Quotes.Length)];

    public WeatherReport GetWeather(string city) =>
        new(city, random.Next(-10, 45), Summaries[random.Next(Summaries.Length)], random.Next(10, 100), DateTimeOffset.UtcNow);
}
