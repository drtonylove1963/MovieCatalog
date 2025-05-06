using System;
using Npgsql;

class Program
{
    static void Main()
    {
        Console.WriteLine("Testing PostgreSQL connection...");
        
        // Try different connection string formats
        string[] connectionStrings = new[]
        {
            "Host=localhost;Port=5432;Database=movie_catalog;Username=postgres;Password=Password123!;Include Error Detail=true;",
            "Host=127.0.0.1;Port=5432;Database=movie_catalog;Username=postgres;Password=Password123!;Include Error Detail=true;",
            "Server=localhost;Port=5432;Database=movie_catalog;User Id=postgres;Password=Password123!;",
            "Server=127.0.0.1;Port=5432;Database=movie_catalog;User Id=postgres;Password=Password123!;"
        };
        
        foreach (var connectionString in connectionStrings)
        {
            try
            {
                Console.WriteLine($"\nTrying connection string: {connectionString}");
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connection successful!");
                
                // Test a simple query
                using var cmd = new NpgsqlCommand("SELECT version()", connection);
                var version = cmd.ExecuteScalar()?.ToString();
                Console.WriteLine($"PostgreSQL version: {version}");
                
                connection.Close();
                Console.WriteLine("Connection closed successfully.");
                return; // Exit if successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                if (ex is NpgsqlException npgEx)
                {
                    foreach (var entry in npgEx.Data.Keys)
                    {
                        Console.WriteLine($"{entry}: {npgEx.Data[entry]}");
                    }
                }
            }
        }
        
        Console.WriteLine("\nAll connection attempts failed.");
    }
}
