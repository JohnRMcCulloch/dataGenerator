namespace dataGenerator.Config;

public class Information
{
    public int LongitudeDecimalPlaces { get; set; } = 6;
    public int LatitudeDecimalPlaces { get; set; } = 6;
    public int TemperatureDecimalPlaces { get; set; } = 1;
    public string[] TemperatureUnit { get; set; } = ["C", "F"];
    public int WindSpeedDecimalPlaces { get; set; } = 1;
    public string[] WindSpeedUnit { get; set; } = ["km/h"];
    public string[] WindDirection { get; set; } = ["N", "NE", "E", "SE", "S", "SW", "W", "NW"];
    public int PrecipitationChancePercentageUpperLimit { get; set; } = 100;
}