using dataGenerator.Config;
using dataGenerator.Data;
using dataGenerator.Data.WeatherData;
using dataGenerator.Factory;
using dataGenerator.FileWriter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace dataGenerator;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.Configure<WeatherConfig>(context.Configuration.GetSection("WeatherConfig"));
                services.Configure<FileConfig>(context.Configuration.GetSection("FileConfig"));
                services.AddTransient<IFileWriter, PlainTextFileWriter>();
                services.AddTransient<IDataGenerator<WeatherModel>, WeatherGenerator>();
                services.AddTransient<IDataFactory, WeatherFactory>();
            })
            .UseSerilog()
            .Build();

        var svc = host.Services.GetService<IDataFactory>();
        svc?.Generate();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}