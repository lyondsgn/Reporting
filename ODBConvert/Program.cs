using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Ask the user to enter the location of the input file
        Console.Write("Enter the location of the input file: ");
        string inputFileLocation = Console.ReadLine();

        // Create a new file with .csv extension
        string outputFileName = Path.GetFileNameWithoutExtension(inputFileLocation) + ".csv";
        string outputFileLocation = Path.Combine(Path.GetDirectoryName(inputFileLocation), outputFileName);

        try
        {
            using (StreamReader reader = new StreamReader(inputFileLocation))
            using (StreamWriter writer = new StreamWriter(outputFileLocation))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Replace "|" with ","
                    string modifiedLine = line.Replace("|", ",");
                    writer.WriteLine(modifiedLine);
                }
            }

            Console.WriteLine($"File converted and saved as {outputFileLocation}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
