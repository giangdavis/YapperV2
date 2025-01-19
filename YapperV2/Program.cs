using Microsoft.Extensions.Configuration;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Base path is the app directory
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Constants for performance tuning
const int BUFFER_SIZE = 1024 * 1024; // 1MB buffer
const FileOptions FILE_OPTIONS = FileOptions.WriteThrough | FileOptions.Asynchronous;

string filePath = configuration["FileSettings:FilePath"];

// Create/truncate the log file with optimized settings 
using var fileStream = new FileStream(
   filePath,
   FileMode.Create,
   FileAccess.Write,
   FileShare.Read,  // Allow reading while we write
   BUFFER_SIZE,
   FILE_OPTIONS
);

using var bufferedStream = new BufferedStream(fileStream, BUFFER_SIZE);
using var writer = new StreamWriter(bufferedStream);

// Test writing some numbers
string[] testNumbers = {
   "123456789",
   "000000123",
   "999999999",
   "111111111",
   "444444444"
};

// Join all numbers into a single string with newline characters
string numbersToWrite = string.Join(Environment.NewLine, testNumbers);

// Write all numbers at once
await writer.WriteAsync(numbersToWrite);

await writer.FlushAsync();

Console.WriteLine("Done writing to file!");