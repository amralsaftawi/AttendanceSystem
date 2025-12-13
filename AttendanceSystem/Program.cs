using AttendanceSystem.Data;
using AttendanceSystem.Entites;
using AttendanceSystem.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace AttendanceSystem
{
    public class program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("DB Path: " + Path.GetFullPath("attendance.db"));
           
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            object value = builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // Ensure folder exists
            var dbPath = Path.GetFullPath("attendance.db");
            var dir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            // Create DB and tables if missing
            if (!File.Exists(dbPath))
            {
                Console.WriteLine($"Creating SQLite DB at {dbPath}");
                using var conn = new SqliteConnection("DataSource=/app/data/attendance.db");
                conn.Open();

                // Read schema.sql
                var schemaSql = File.ReadAllText("schema.sql");

                // Execute each command separately
                foreach (var cmdText in schemaSql.Split(';'))
                {
                    var trimmed = cmdText.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = trimmed;
                        cmd.ExecuteNonQuery();
                    }
                }
            }";
            var dir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            // Create DB and tables if missing
            if (!File.Exists(dbPath))
            {
                Console.WriteLine($"Creating SQLite DB at {dbPath}");
                using var conn = new SqliteConnection(connString);
                conn.Open();

                // Read schema.sql
                var schemaSql = File.ReadAllText("schema.sql");

                // Execute each command separately
                foreach (var cmdText in schemaSql.Split(';'))
                {
                    var trimmed = cmdText.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = trimmed;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            if (!File.Exists(Path.GetFullPath("attendance.db")))
            {
                Console.WriteLine($"Creating SQLite database at {Path.GetFullPath("attendance.db")}");
                
                var schemaSql = File.ReadAllText("schema.sql");

                using var conn = new SqliteConnection("DataSource=/app/data/attendance.db;");
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = schemaSql;
                cmd.ExecuteNonQuery();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }

}