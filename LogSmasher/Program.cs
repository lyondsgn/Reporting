using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LogFileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path of the log file: ");
            string inputFilePath = Console.ReadLine();

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            Console.Write("Enter the name for the new file: ");
            string outputFileName = Console.ReadLine();
            string outputFilePath = Path.Combine(Environment.CurrentDirectory, outputFileName);


            Console.Write("Enter a Unique ID: ");
            string uniqueId = Console.ReadLine();

            using (StreamReader reader = new StreamReader(inputFilePath))
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (IsValidLogEntry(line))
                    {
                        line = ReformatLogEntry(line, uniqueId);
                        writer.WriteLine(line);
                    }
                }
            }

            Console.WriteLine($"Reformatted log file saved as {outputFileName}");
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

            // Create the reformatted line with Unique ID
            return $"{timestamp} | {messageType} | {firstTwoWords} | {elapsedTime} | {uniqueId}";
        }
    }
}
