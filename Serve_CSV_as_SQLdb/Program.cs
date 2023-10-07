using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string csvFilePath = @"C:\Users\Ed\Documents\PowerBI\PrismTerminal\20231002_SAPAutumn Meet\csv\RTA_amt_230930.csv";
        string databasePath = @"C:\Users\Ed\Documents\PowerBI\PrismTerminal\20231002_SAPAutumn Meet\RTA_amt_230930.db";
        int batchSize = 1000; // Adjust batch size as needed

        // Modify the CSV header line to change "Transaction" to "TransactionID"
        ModifyCsvHeader(csvFilePath);

        CreateSQLiteDatabase(databasePath);

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
        {
            connection.Open();

            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string headerLine = reader.ReadLine();
                string[] headers = headerLine.Split(',');

                // Create a new table with modified headers
                CreateTable(connection, "YourTable", headers);

                while (!reader.EndOfStream)
                {
                    var batch = Enumerable.Range(0, batchSize)
                        .Select(_ => reader.ReadLine())
                        .Where(line => line != null)
                        .Select(line => line.Split(','));

                    // Insert the batch of rows into the newly created table
                    InsertBatch(connection, "YourTable", headers, batch);
                }
            }
        }

        Console.WriteLine("CSV data with modified headers has been imported into the SQLite database.");
    }

    static void CreateSQLiteDatabase(string databasePath)
    {
        SQLiteConnection.CreateFile(databasePath);
    }

    static void CreateTable(SQLiteConnection connection, string tableName, string[] headers)
    {
        using (SQLiteCommand createTableCommand = new SQLiteCommand(connection))
        {
            // Ensure column names are unique by appending a unique identifier to duplicates
            var uniqueHeaders = MakeColumnNamesUnique(headers);
            
            // Create a new table with the specified name and double-quoted column names
            string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", uniqueHeaders.Select(header => $"\"{header}\" TEXT"))});";
            createTableCommand.CommandText = createTableQuery;
            createTableCommand.ExecuteNonQuery();
        }
    }

    static void InsertBatch(SQLiteConnection connection, string tableName, string[] headers, IEnumerable<string[]> batch)
    {
        using (var transaction = connection.BeginTransaction())
        {
            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = $"INSERT INTO {tableName} ({string.Join(", ", headers)}) VALUES ({string.Join(", ", headers.Select(_ => "?"))})";

                foreach (var row in batch)
                {
                    insertCommand.Parameters.Clear();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        insertCommand.Parameters.AddWithValue($"@p{i}", row[i]);
                    }
                    insertCommand.ExecuteNonQuery();
                }
            }
            transaction.Commit();
        }
    }

    static void ModifyCsvHeader(string csvFilePath)
    {
        string headerText;
        using (StreamReader reader = new StreamReader(csvFilePath))
        {
            headerText = reader.ReadLine();
        }

        // Modify the header text to change "Transaction" to "TransactionID"
        headerText = headerText.Replace("Transaction", "TransactionID");

        // Write the modified header back to the CSV file
        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            writer.WriteLine(headerText);
        }
    }

    static string[] MakeColumnNamesUnique(string[] headers)
    {
        // Add a unique identifier to duplicate column names
        var uniqueHeaders = new string[headers.Length];
        var counts = new Dictionary<string, int>();

        for (int i = 0; i < headers.Length; i++)
        {
            string header = headers[i];
            if (counts.ContainsKey(header))
            {
                counts[header]++;
                uniqueHeaders[i] = $"{header}_{counts[header]}";
            }
            else
            {
                counts[header] = 0;
                uniqueHeaders[i] = header;
            }
        }

        return uniqueHeaders;
    }
}
