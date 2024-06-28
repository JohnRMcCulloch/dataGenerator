namespace dataGenerator.Config;

/// <summary>
/// Configuration class for specifying weather data generation settings.
/// </summary>
public class WeatherConfig
{
    public string FileName { get; set; } = "WeatherData";
    public DateTime StartTimestampUtc { get; set; } = DateTime.Now.ToUniversalTime();
    public int NumberOfWeatherInformationRecords { get; set; } = 5;
    public int WeatherTimestampQuantity { get; set; } = 10;
    public Information Information { get; set; } = new();
}