using System.Text;
using dataGenerator.Config;
using dataGenerator.Data;
using dataGenerator.Data.WeatherData;
using dataGenerator.FileWriter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataGenerator.Factory;

/// <summary>
/// Factory class responsible for generating weather data and writing it to a file.
/// </summary>
public class WeatherFactory : IDataFactory
{
    private readonly ILogger<WeatherFactory> _log;
    private readonly WeatherConfig _weatherConfig;
    private readonly IDataGenerator<WeatherModel> _dataGenerator;
    private readonly IFileWriter _fileWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherFactory"/> class.
    /// </summary>
    /// <param name="log">The logger instance for logging information.</param>
    /// <param name="weatherConfig">The weather configuration options.</param>
    /// <param name="dataGenerator">The data generator for generating weather data.</param>
    /// <param name="fileWriter">The file writer for writing data to a file.</param>
    public WeatherFactory(ILogger<WeatherFactory> log, IOptions<WeatherConfig> weatherConfig,
        IDataGenerator<WeatherModel> dataGenerator, IFileWriter fileWriter)
    {
        _log = log;
        _weatherConfig = weatherConfig.Value;
        _dataGenerator = dataGenerator;
        _fileWriter = fileWriter;
    }

    /// <summary>
    /// Generates weather data and writes it to a file.
    /// </summary>
    public void Generate()
    {
        _log.LogInformation("Generating Weather Data for File");
        var content = GenerateWeatherBatchData(
            startTimestamp: _weatherConfig.StartTimestampUtc,
            lengthOfHours: _weatherConfig.WeatherTimestampQuantity,
            numberOfWeatherInformationRecords: _weatherConfig.NumberOfWeatherInformationRecords
        );
        _fileWriter.WriteToFile(content, GetFileName());
    }

    /// <summary>
    /// Generates a batch of weather data for a specified period.
    /// </summary>
    /// <param name="startTimestamp">The start timestamp for the weather data.</param>
    /// <param name="lengthOfHours">The length of time in hours for which to generate data.</param>
    /// <param name="numberOfWeatherInformationRecords">The number of weather information records per hour.</param>
    /// <returns>A formatted string containing the generated weather data.</returns>
    private string GenerateWeatherBatchData(DateTime startTimestamp, int lengthOfHours,
        int numberOfWeatherInformationRecords)
    {
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < lengthOfHours; i++)
        {
            stringBuilder.AppendLine(startTimestamp.AddHours(i).ToString("yyyy-MM-dd HH:00UTC"));
            for (var j = 0; j < numberOfWeatherInformationRecords; j++)
            {
                var weatherData = _dataGenerator.GenerateData();
                stringBuilder.AppendLine(
                    $"{weatherData.Longitude}" +
                    $"\t{weatherData.Latitude}" +
                    $"\t{weatherData.TemperatureValue}" +
                    $"\t{weatherData.TemperatureUnit}" +
                    $"\t{weatherData.WindSpeedValue}" +
                    $"\t{weatherData.WindSpeedUnit}" +
                    $"\t{weatherData.WindDirection}" +
                    $"\t{weatherData.PrecipitationChance}");
            }
        }

        return stringBuilder.ToString();
    }

    private string GetFileName()
    {
        var fileName = _weatherConfig.FileName;
        return string.IsNullOrEmpty(fileName) ? "WeatherData" : fileName;
    }
}