using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dataGenerator.FileWriter;

public class PlainTextFileWriter : IFileWriter
{
    private readonly ILogger<PlainTextFileWriter> _log;
    private readonly IConfiguration _config;

    public PlainTextFileWriter(ILogger<PlainTextFileWriter> log, IConfiguration config)
    {
        _log = log;
        _config = config;
    }

    public void WriteToFile(string content, string fileName)
    {
        try
        {
            var filePath = Path.Combine(GetDirectoryPath(), $"{fileName}{DateTime.Now:MMddyyyy.hhmmss}.txt");

            File.WriteAllText(filePath, content);
            _log.LogInformation("Data successfully written to file: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _log.LogError("An error occurred while writing to file: {Exception}", ex);
        }
    }

    private string GetDirectoryPath()
    {
        var directoryPath = _config.GetValue<string>("FilePath");
        if (Directory.Exists(directoryPath)) return directoryPath;
        Directory.CreateDirectory(directoryPath!);
        _log.LogInformation("Directory created successfully {directory}", directoryPath);
        return directoryPath!;
    }
}