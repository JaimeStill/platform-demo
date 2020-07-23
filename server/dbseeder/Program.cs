using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Core.Extensions;
using PlatformDemo.Data;
using PlatformDemo.Data.Extensions;

namespace dbseeder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("A connection string must be provided to seed the database.");
                Console.WriteLine();
                throw new Exception("No connection string provided");
            }

            var connection = args[0];

            while (string.IsNullOrEmpty(connection))
            {
                Console.WriteLine("Please provide a connection string:");
                connection = Console.ReadLine();
                Console.WriteLine();
            }

            try
            {
                Console.WriteLine($"Connection: {connection}");

                var builder = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(connection);

                using var db = new AppDbContext(builder.Options);


                var iterations = 0;
                var check = false;

                while (!check && iterations < 3)
                {
                    iterations++;
                    Console.WriteLine($"Verifying DB Connection. Attempt #: {iterations}");
                    check = await db.Database.CanConnectAsync();
                }

                if (!check) throw new Exception("Unable to connect to the database");

                Console.WriteLine("Connection Succeeded");
                Console.WriteLine();
                await db.Initialize();

                Console.WriteLine();
                Console.WriteLine("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while seeding the database:");
                Console.WriteLine(ex.GetExceptionChain());
            }
        }
    }
}
