# Data Generator Application

## Overview
The Data Generator is a dotnet 8 application that automates the creation of test data. 
This tool is designed to save time and effort when generating various types of test data files. 
The application generates as much test data as needed, adhering to a specific file standards.

At present the application is written to produce a plain text file for weather information in WIS format (Weather Information Service).

## WIS Format
Each record starts with a timestamp denoting the hour the weather data is for. The format is **YYYY-MM-DD HH:MMUTC**, e.g., **2024-06-01 00:00UTC**.

Following each timestamp is a tab-delimited line of weather information with the following details:

- **Longitude**: A floating-point number to 6 decimal places, e.g., -55.123456.
- **Latitude**: A floating-point number to 6 decimal places, e.g., 34.123456.
- **Temperature Value**: A floating-point number to 1 decimal place, e.g., 23.5.
- **Temperature Unit**: A string indicating the unit, either "C" for Celsius or "F" for Fahrenheit.
- **Wind Speed Value**: A floating-point number to 1 decimal place, e.g., 15.0.
- **Wind Speed Unit**: A string indicating the unit, which must be "km/h".
- **Wind Direction**: A string indicating the cardinal or ordinal direction, e.g., "NW".
- **Precipitation Chance Percentage**: An integer up to 100.

The generated weather data files are in plaintext format. The data format can be adjusted through the application settings to achieve different test data ranges. 
This can be used to produce positive, negative and different ranged test data.

This application utilises the [Bogus](https://github.com/bchavez/Bogus "Bogus") library to adjust rules on the data variables.

## Application Settings
The application settings are stored in a JSON file, allowing easy configuration of the output file path, file name, and weather data specifications.

| Setting Path                                            | Description                                                          | Example Value | Default Value                                |
|---------------------------------------------------------|----------------------------------------------------------------------|-----|----------------------------------------------|
| `FileConfig.FilePath`                                   | Path where the output files will be saved                            | `C:\\DataGenerator\\Output` | -                                            |
| `WeatherConfig.FileName`                                | Base name for the generated weather data file                        | `WeatherData` | `WeatherData`                                  |
| `WeatherConfig.StartTimestampUtc`                       | Starting timestamp for the weather data in UTC                       | `2024-06-27T13:00:00` | `Current DateTime UTC`                         |
| `WeatherConfig.NumberOfWeatherInformationRecords`       | Number of weather information records to generate                    | `2`   | `5`                                            |
| `WeatherConfig.WeatherTimestampQuantity`                | Number of timestamps to generate per file                            | `5`   | `10`                                           |
| `WeatherConfig.Information.LongitudeDecimalPlaces`      | Number of decimal places for longitude values                        | `6`   | `6`                                            |
| `WeatherConfig.Information.LatitudeDecimalPlaces`       | Number of decimal places for latitude values                         | `6`   | `6`                                            |
| `WeatherConfig.Information.TemperatureDecimalPlaces`    | Number of decimal places for temperature values                      | `1`   | `1`                                            |
| `WeatherConfig.Information.TemperatureUnit`             | List of possible temperature units                                   | `["C", "F"]` | `["C", "F"]`                                   |
| `WeatherConfig.Information.WindSpeedDecimalPlaces`      | Number of decimal places for wind speed values                       | `1   | 1                                            |
| `WeatherConfig.Information.WindSpeedUnit`               | List of possible wind speed units                                    | `["km/h"]` | `["km/h"]`                                     |
| `WeatherConfig.Information.WindDirection`               | List of possible wind directions                                     | `["N", "NE", "E", "SE", "S", "SW", "W", "NW"]` | `["N", "NE", "E", "SE", "S", "SW", "W", "NW"]` |
| `WeatherConfig.Information.PrecipitationChancePercentageUpperLimit` | Upper limit for precipitation chance percentage | `100` | `100`                                          |
| `Serilog.MinimumLevel.Default`                          | Default logging level                                                | `Information` | -                                            |
| `Serilog.MinimumLevel.Override.Microsoft`               | Logging level override for Microsoft components                      | `Information` | -                                            |
| `Serilog.MinimumLevel.Override.System`                  | Logging level override for System components                         | `Warning` | -                                            |

## Pre Requirements
.NET Core SDK 8
[Dotnet 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0 "Dotnet 8")

## Usage

1. Ensure the app settings file is properly set up with the desired settings.
2. Run the application.
3. The generated weather data files will be saved to the specified output path.

Ensure you have navigated to the `dataGenerator` directory containing `dataGenerator.csproj`
```console
dotnet build
dotnet run
```
These will install any needed dependencies, build the project, and run the project respectively.

**Alternatively** you can build and run via an IDE

## Future Development
At present the application produces weather information, but it can be easily extended to produce other data types.
The IDataGenerator interface can be implemented by a new Data Class and injected via Dependency Injection in the Program.cs file.

Current the application writes to a plain text file, but this can be easily extended to write to other file formats e.g. JSON.
The IFileWriter interface can be implemented by a new FileWriter class and injected via Dependency Injection in the Program.cs file.