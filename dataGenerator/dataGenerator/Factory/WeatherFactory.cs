using System.Text;
using dataGenerator.Data;
using dataGenerator.Data.WeatherData;
using dataGenerator.FileWriter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dataGenerator.Factory;

public class WeatherFactory : IDataFactory
{
    private readonly ILogger<WeatherFactory> _log;
    private readonly IConfiguration _config;
    private readonly IDataGenerator<WeatherModel> _dataGenerator;
    private readonly IFileWriter _fileWriter;

    public WeatherFactory(ILogger<WeatherFactory> log, IConfiguration config,
        IDataGenerator<WeatherModel> dataGenerator, IFileWriter fileWriter)
    {
        _log = log;
        _config = config;
        _dataGenerator = dataGenerator;
        _fileWriter = fileWriter;
    }

    public void Generate()
    {
        var startTimestamp = _config.GetValue<DateTime>("Weather:StartTimestampUTC");
        var lengthOfHours = _config.GetValue<int>("Weather:WeatherTimestampQuantity");
        var numberOfRecords = _config.GetValue<int>("Weather:NumberOfWeatherInformationRecords");

        var content = GenerateWeatherBatchData(startTimestamp, lengthOfHours, numberOfRecords);
        _fileWriter.WriteToFile(content, GetFileName());
    }

    private string GenerateWeatherBatchData(DateTime startTimestamp, int lengthOfHours,
        int numberOfWeatherInformationRecords)
    {
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < lengthOfHours; i++)
        {
            var hourData = new List<WeatherModel>();

            for (var j = 0; j < numberOfWeatherInformationRecords; j++)
            {
                var weatherData = _dataGenerator.GenerateData();
                // Adjust the timestamp to the specific hour we are iterating over
                weatherData.Timestamp = startTimestamp.AddHours(i);
                hourData.Add(weatherData);
            }

            var timestamp = hourData.First().Timestamp.ToString("yyyy-MM-dd HH:00UTC");
            stringBuilder.AppendLine(timestamp);

            foreach (var weatherData in hourData)
            {
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
        var fileName = _config.GetValue<string>("Weather:fileName");
        return string.IsNullOrEmpty(fileName) ? "WeatherData" : fileName;
    }
}