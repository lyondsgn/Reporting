using System;
using System.IO;

using System.Collections.Generic;
using System.Management;
using CsvHelper;
using System.Globalization;


namespace CleanupUtility1;

public static class FilePayload
{
    public static int[] DelimiterSpacing = { 10, 21, 27, 38,48 };
    public static string NL = Environment.NewLine; // shortcut
    public static string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
    public static string RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
    public static string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
    public static string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";
    public static string BLUE = Console.IsOutputRedirected ? "" : "\x1b[94m";

    public class UserSelect
    {
        public string PayloadDir { get; set; } = default!;
        public string OutputDir { get; set; } = default!;
    }


    public static void r_Term_Version_Convert()
    {
        UserSelect Selections = new UserSelect();

        Console.WriteLine($"{BLUE}---r_Term_Version_Convert----{NORMAL}");
        Console.WriteLine($"{YELLOW}---WARNING - This tool will attempt to format and overwrite all .txt files in the directory provided----{NORMAL}");
        Console.WriteLine("-Enter Source Directory");
        // Console.WriteLine("Spectrum    : line:{0} index:{1}", Timread.line, Timread.index);
        // Console.WriteLine("GWS         : Ch {0} : Model {1}", Machine.gwsChannel, Machine.Model);
        // Console.WriteLine("---------------------------");
        // Console.WriteLine("Write to {0}?  y or n", d.Name);
        Selections.PayloadDir = Console.ReadLine();
        Selections.OutputDir = Selections.PayloadDir;
        // Using the GetFiles() method
        string[] filedata = Directory.GetFiles(Selections.PayloadDir);
        Console.WriteLine("input: " + Selections.PayloadDir);
        // Displaying the file name one by one
        foreach (string i in filedata)
        {
            RemoveBlanks(i, Selections.OutputDir);
            RemoveHeaders(i, 4);
            CleanFormatting(i);
            Console.WriteLine("Processed File: " + i);
            // Console.WriteLine(i);
        }
        Combinefiles(Selections.PayloadDir, "CMTY|DIV|WIN|DATE|TIME|Version|A|Reader|B|C|Card|D|OS|E|F|G");
        Console.WriteLine("Files Combined");
        Console.WriteLine($"{GREEN}---r_Term_Version PreProcessing Complete----{NORMAL}");
        // System.Diagnostics.Process.Start("explorer.exe", @"C:\Users\Ed\Documents\PowerBI\Reports ");
    }

    public static void Combinefiles(string DirLoc, string header)
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var output = new StreamWriter(tempFileName))
            {
                output.WriteLine(header);
                foreach (var file in Directory.GetFiles(DirLoc, "*.*"))
                {
                    using (var input = new StreamReader(file))
                    {
                        output.WriteLine(input.ReadToEnd());
                    }
                }
            }
            File.Copy(tempFileName, DirLoc + "/combined.txt", true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }




    public static void RemoveBlanks(string fileLoc, string DirLoc)
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var streamReader = new StreamReader(fileLoc))
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
            File.Copy(tempFileName, fileLoc, true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }
    // Remove header information denoted by [FF] and row count
    public static void RemoveHeaders(string fileLoc, int NumHeaderRows)
    {
        int row = 0;
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var streamReader = new StreamReader(fileLoc))
            using (var streamWriter = new StreamWriter(tempFileName))
            {
                // string headerLine = "CMTY,Division,WIND";
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    row++;
                    // Remove Top
                    if (row <= NumHeaderRows)
                    {
                        continue;
                    }

                    // Remove 
                    if (line.Contains("\f"))
                    {
                        row = 0;
                        continue;
                    }
                    if (line.Contains("NO TERM VERSIONS") || line.Contains("error"))
                    {
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
            File.Copy(tempFileName, fileLoc, true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }


    // Cleanup remaining spaces and Format with Pipes
    public static void CleanFormatting(string fileLoc)
    {
        int row = 0;
        var tempFileName = Path.GetTempFileName();
        try
        {
            using (var streamReader = new StreamReader(fileLoc))
            using (var streamWriter = new StreamWriter(tempFileName))
            {
                // string headerLine = "CMTY,Division,WIND";
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    row++;
                    for (int j = 0; j < DelimiterSpacing.Length; j++)
                    {
                        line = line.Insert(DelimiterSpacing[j], "|");
                    }

                    //Reduce Spaces
                    // string newline = string.Join(" ", line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    // string spaceline = newline.Replace("| ", "|").Replace(" |", "|");//.Replace(" ", "|");
                    streamWriter.WriteLine(line);
                }
            }
            File.Copy(tempFileName, fileLoc, true);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

}