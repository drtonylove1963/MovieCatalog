using System;
using Npgsql;

class Program
{
    static void Main()
    {
        Console.WriteLine("Testing PostgreSQL connection...");
        
        // Try different connection string formats with more variations
        string[] connectionStrings = new[]
        {
            // Default format with localhost - exact password from Docker container
            "Host=localhost;Port=5432;Database=movie_catalog;Username=postgres;Password=Password123!;Include Error Detail=true;",
            
            // Try with different database name
            "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Password123!;Include Error Detail=true;",
            
            // Try with different port
            "Host=localhost;Port=5433;Database=movie_catalog;Username=postgres;Password=Password123!;Include Error Detail=true;",
            
            // Try with IP address
            "Host=127.0.0.1;Port=5432;Database=movie_catalog;Username=postgres;Password=Password123!;Include Error Detail=true;",
            
            // Try with different user
            "Host=localhost;Port=5432;Database=movie_catalog;Username=postgres;Password=postgres;Include Error Detail=true;",
        };
        
        foreach (var connectionString in connectionStrings)
        {
            try
            {
                Console.WriteLine($"\nTrying connection string: {connectionString}");
                using var connection = new NpgsqlConnection(connectionString);
                
                Console.WriteLine("Opening connection...");
                connection.Open();
                Console.WriteLine("Connection successful!");
                
                // Test a simple query
                using var cmd = new NpgsqlCommand("SELECT version()", connection);
                var version = cmd.ExecuteScalar()?.ToString();
                Console.WriteLine($"PostgreSQL version: {version}");
                
                // Test if the schema exists
                try
                {
                    using var schemaCmd = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'movie_catalog')", connection);
                    bool schemaExists = (bool)schemaCmd.ExecuteScalar();
                    Console.WriteLine($"Schema 'movie_catalog' exists: {schemaExists}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking schema: {ex.Message}");
                }
                
                connection.Close();
                Console.WriteLine("Connection closed successfully.");
                return; // Exit if successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                
                if (ex is NpgsqlException npgEx)
                {
                    Console.WriteLine("PostgreSQL exception details:");
                    foreach (var entry in npgEx.Data.Keys)
                    {
                        Console.WriteLine($"  {entry}: {npgEx.Data[entry]}");
                    }
                }
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner exception type: {ex.InnerException.GetType().Name}");
                }
            }
        }
        
        Console.WriteLine("\nAll connection attempts failed.");
    }
}
