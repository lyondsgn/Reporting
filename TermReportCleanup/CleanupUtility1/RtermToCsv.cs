using System;
using System.IO;

using System.Collections.Generic;
using System.Management;
using CsvHelper;
using System.Globalization;


namespace CleanupUtility1;

public static class FilePayload
{

    class Selections
    {
        public string PayloadDir { get; set; } = default!;
        public string OutputDir { get; set; } = default!;
    }

    public static void OpenFile()
    {
        // Using the GetFiles() method
        string[] filedata = Directory.GetFiles(@"C:\Users\Ed\Documents\PowerBI\Reports");

        // Displaying the file name one by one
        foreach (string i in filedata)
        {

            Console.WriteLine(i);
        }
        // System.Diagnostics.Process.Start("explorer.exe", @"C:\Users\Ed\Documents\PowerBI\Reports ");
    }

    public static void RemoveBlanks()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var streamReader = new StreamReader(@"C:\Users\Ed\Documents\PowerBI\Reports\GGFT_term_versions.txt"))
            using (var streamWriter = new StreamWriter(tempFileName))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        streamWriter.WriteLine(line);
                    }
                }
            }
            File.Copy(tempFileName, @"C:\Users\Ed\Documents\PowerBI\Reports\GGFT_term_versions1.txt", true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
    public static void RemoveHeaders()
    {
        int row = 0;
        int HeaderSize = 4;
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var streamReader = new StreamReader(@"C:\Users\Ed\Documents\PowerBI\Reports\GGFT_term_versions1.txt"))
            using (var streamWriter = new StreamWriter(tempFileName))
            {
                // string headerLine = "CMTY,Division,WIND";
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    row++;
                    // Remove Top
                    if (row == 1 || row == 2 || row == 3 || row == 4)
                    {
                        continue;
                    }

                    // Remove 
                    if (line.Contains("\f"))
                    {
                        row = 0;
                        continue;
                    }
                    // If letter 8 is blank. remove line 
                    if (Char.IsLetter(line[8]))
                    {
                        streamWriter.WriteLine(line);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            File.Copy(tempFileName, @"C:\Users\Ed\Documents\PowerBI\Reports\GGFT_term_versions1.txt", true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    public static void CleanFormatting()
    {
        int row = 0;
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var streamReader = new StreamReader(@"C:\Users\Ed\Documents\PowerBI\Reports\GGFT_term_versions1.txt"))
            using (var streamWriter = new StreamWriter(tempFileName))
            {
                // string headerLine = "CMTY,Division,WIND";
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    row++;
                    //Reduce Spaces

                    string newline = string.Join(" ", line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    string spaceline = newline.Replace("| ","|").Replace(" |","|").Replace(" ","|");

                    streamWriter.WriteLine(spaceline);
                }
            }
            File.Copy(tempFileName, @"C:\Users\Ed\Documents\PowerBI\Reports\GGFT_term_versions2.txt", true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }



}