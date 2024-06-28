using dataGenerator.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataGenerator.FileWriter;

public class PlainTextFileWriter : IFileWriter
{
    private readonly ILogger<PlainTextFileWriter> _log;
    private readonly FileConfig _weatherConfig;

    public PlainTextFileWriter(ILogger<PlainTextFileWriter> log, IOptions<FileConfig> weatherConfig)
    {
        _log = log;
        _weatherConfig = weatherConfig.Value;
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
        var directoryPath = _weatherConfig.FilePath;
        if (Directory.Exists(directoryPath)) return directoryPath;
        Directory.CreateDirectory(directoryPath!);
        _log.LogInformation("Directory created successfully {directory}", directoryPath);
        return directoryPath!;
    }
}