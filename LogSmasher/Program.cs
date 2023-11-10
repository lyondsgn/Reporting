// 20231110|ELS|initial build of log smasher... clean uip reports to analyze

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogFileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the directory containing log files: ");
            string directoryPath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory not found.");
                return;
            }

            Console.Write("Enter a Unique ID: ");
            string uniqueId = Console.ReadLine();

            string[] inputFiles = Directory.GetFiles(directoryPath, "*.log");

            if (inputFiles.Length == 0)
            {
                Console.WriteLine("No log files found in the directory.");
                return;
            }

            string outputFileName = Path.GetFileName(directoryPath) + ".reformatted.log";
            string outputFilePath = Path.Combine(directoryPath, outputFileName);

            using (StreamWriter combinedWriter = new StreamWriter(outputFilePath))
            {
                // Add a header line to the output file
                combinedWriter.WriteLine("Date | Time | Message Type | First Two Words | Elapsed Time | Unique ID");

                foreach (string inputFile in inputFiles)
                {
                    using (StreamReader reader = new StreamReader(inputFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (IsValidLogEntry(line))
                            {
                                line = ReformatLogEntry(line, uniqueId);
                                combinedWriter.WriteLine(line);
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Reformatted log files combined and saved as {outputFileName}");
        }

        static bool IsValidLogEntry(string line)
        {
            // Use regular expression to check if the line starts with a timestamp and message type
            // Example format: "2023-09-29 14:51:41.681 -05:00 [DBG]"
            string pattern = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} [-+]\d{2}:\d{2} \[[A-Z]+\]";

            // Check if the line contains an elapsed time in one of the specified formats
            string elapsedTimePattern = @"\b\d{2}:\d{2}:\d{2}\.\d{7}\b";
            pattern += ".*" + elapsedTimePattern;

            return Regex.IsMatch(line, pattern);
        }

        static string ReformatLogEntry(string line, string uniqueId)
        {
            // Extract the message type and elapsed time from the line
            string messageTypePattern = @"\[[A-Z]+\]";
            string elapsedTimePattern = @"\b\d{2}:\d{2}:\d{2}\.\d{7}\b";

            string messageType = Regex.Match(line, messageTypePattern).Value;
            string elapsedTime = Regex.Match(line, elapsedTimePattern).Value;

            // Extract the timestamp and message type from the line
            string timestamp = line.Substring(0, line.IndexOf(messageType));

            // Extract the remaining text after the message type
            string remainingText = line.Substring(line.IndexOf(messageType) + messageType.Length).TrimStart();

            // Split the remaining text into words and keep only the first two
            string[] words = remainingText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string firstTwoWords = string.Join(" ", words.Take(2));

            // Create the reformatted line with Unique ID and | between date and time
            string[] dateTimeParts = timestamp.Split(' ');
            string reformattedTimestamp = $"{dateTimeParts[0]} | {dateTimeParts[1]}";

            return $"{reformattedTimestamp} | {messageType} | {firstTwoWords} | {elapsedTime} | {uniqueId}";
        }
    }
}
