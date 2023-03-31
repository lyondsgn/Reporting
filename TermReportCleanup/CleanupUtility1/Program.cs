// See https://aka.ms/new-console-template for more information
//https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.getlogicaldrives?view=net-6.0
//https://kodify.net/csharp/computer-drive/get-drive-info/
using System;
using System.Collections.Generic;
using CleanupUtility1;
// Console.SetWindowSize(70, 20);
while (true)
{
    Menu();
}

async void Menu()
{

    // Console.Clear();
    Console.WriteLine(" ");
    Console.WriteLine("-----File Preprocessor----\r");
    Console.WriteLine("\tc - Pre-Process r_term_version");
    // Console.WriteLine("\th - Help");
    Console.Write("Your option? ");

    // Use a switch statement to do the math.
    switch (Console.ReadLine())
    {
        case "c":
            FilePayload.r_Term_Version_Convert();

            // Console.WriteLine("exporting CSV ");
            break;

        case "1":
            FilePayload.Combinefiles(@"C:\Users\Ed\Documents\PowerBI\Reports");
            break;

    }
}

