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
    Console.WriteLine("-----File Cleaner - (TermVersions to .CSV) ----\r");
    Console.WriteLine("\to - open file");
    Console.WriteLine("\tc - clean");
    Console.WriteLine("\t1 - remove blanks");
    Console.WriteLine("\t2 - remove Headers");
    Console.WriteLine("\t3 - remove Headers");
    // Console.WriteLine("\th - Help");
    Console.Write("Your option? ");

    // Use a switch statement to do the math.
    switch (Console.ReadLine())
    {
        case "o":
            Console.WriteLine("\to - open file");
            FilePayload.OpenFile();
            break;

        case "c":
            Console.WriteLine("Processing file ");
            Console.WriteLine("removing blank lines ");
            Console.WriteLine("removing page breaks ");
            Console.WriteLine("removing page headers ");
            Console.WriteLine("creating columns ");
            Console.WriteLine("exporting CSV ");
            break;

        case "1":
            Console.WriteLine("\t1 - removing blank lines");
            FilePayload.RemoveBlanks();
            break;

        case "2":
            Console.WriteLine("\t1 - removing Header lines");
            FilePayload.RemoveHeaders();
            break;

                    case "3":
            Console.WriteLine("\tCleanup");
            FilePayload.CleanFormatting();
            break;
    }
}

