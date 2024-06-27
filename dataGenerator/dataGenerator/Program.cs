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
        //C# Objects we pass references, we pass a reference to that instance
        //Any work done on builder within BuildConfig is modified for everyone
        BuildConfig(builder);

        Log.Logger = new LoggerConfiguration()
            //Execute the builder, set it up give it connection to appsettings.json file
            //Read the configuration from the resulting build 
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger.Information("Application Starting");

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<IFileWriter, PlainTextFileWriter>();
                services.AddTransient<IDataGenerator<WeatherModel>, WeatherGenerator>();
                services.AddTransient<IDataFactory, WeatherFactory>();
            })
            .UseSerilog()
            .Build();

        var svc = ActivatorUtilities.CreateInstance<WeatherFactory>(host.Services);
        svc.Generate();
    }

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsetings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}).json",
                optional: true)
            .AddEnvironmentVariables();
    }
}