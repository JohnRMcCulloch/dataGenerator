namespace dataGenerator.Data.WeatherData;

public class WeatherModel
{
    public DateTime Timestamp { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double TemperatureValue { get; set; }
    public string? TemperatureUnit { get; set; }
    public double WindSpeedValue { get; set; }
    public string? WindSpeedUnit { get; set; }
    public string? WindDirection { get; set; }
    public int PrecipitationChance { get; set; }
}