using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Log Summarizer");

        Console.Write("Enter the input file path: ");
        string inputFilePath = Console.ReadLine();

        Console.Write("Enter the output file name (without extension): ");
        string outputFileName = Console.ReadLine();

        // Get the directory of the input file
        string inputDirectory = Path.GetDirectoryName(inputFilePath);

        // Combine the input directory and the output filename to create the full output path
        string outputFilePath = Path.Combine(inputDirectory, outputFileName + ".txt");

        using (StreamReader reader = new StreamReader(inputFilePath))
        using (StreamWriter writer = new StreamWriter(outputFilePath, true)) // Pass 'true' to append to the file
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (IsTimestampedLine(line))
                {
                    string summarizedLine = SummarizeLogLine(line);
                    writer.WriteLine(summarizedLine);
                }
            }
        }

        Console.WriteLine("Log summarization completed. Output written to: " + outputFilePath);
    }

    static bool IsTimestampedLine(string line)
    {
        // Use a regex pattern to match lines with timestamps
        string pattern = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3} [-+]\d{2}:\d{2} \[[A-Z]+\] ";
        return Regex.IsMatch(line, pattern);
    }

    static string SummarizeLogLine(string line)
    {
        // Split the line into parts based on spaces
        string[] parts = line.Split(' ');

        // Create a summarized line with the desired format
        string summarizedLine = $"{parts[0]} | {parts[1]} | {parts[2]} | {parts[3]} | {parts[4]} | {parts[5]} | {string.Join(" ", parts, 6, parts.Length - 6)}";

        return summarizedLine;
    }
}