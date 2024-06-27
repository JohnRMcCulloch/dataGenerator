using Bogus;
using dataGenerator.Data.WeatherData;

namespace dataGenerator.Data;

public class WeatherGenerator : IDataGenerator<WeatherModel>
{
    private readonly Faker<WeatherModel> _weatherModelFaker;

    public WeatherGenerator()
    {
        //Look into Randomizer and Seed value
        Randomizer.Seed = new Random(123);
        _weatherModelFaker = new Faker<WeatherModel>()
            .RuleFor(w => w.Timestamp, f => f.Date.RecentOffset(30).UtcDateTime)
            .RuleFor(w => w.Longitude, f => Math.Round(f.Address.Longitude(), 6))
            .RuleFor(w => w.Latitude, f => Math.Round(f.Address.Latitude(), 6))
            .RuleFor(w => w.TemperatureUnit, f => f.PickRandom("C", "F"))
            .RuleFor(w => w.TemperatureValue, (f, w) =>
                w.TemperatureUnit == "C"
                    ? Math.Round(f.Random.Double(-30, 50), 1) // Celsius range
                    : Math.Round(f.Random.Double(-22, 122), 1)) // Fahrenheit range
            .RuleFor(w => w.WindSpeedValue, f => Math.Round(f.Random.Double(0, 150), 1))
            .RuleFor(w => w.WindSpeedUnit, f => "km/h")
            .RuleFor(w => w.WindDirection, f =>
                f.PickRandom("N", "NE", "E", "SE", "S", "SW", "W", "NW"))
            .RuleFor(w => w.PrecipitationChance, f => f.Random.Int(0, 100));
    }

    public WeatherModel GenerateData()
    {
        return _weatherModelFaker.Generate();
    }
}