using AttendanceSystem.Data;
using AttendanceSystem.Entites;
using AttendanceSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AttendanceSystem
{
    public class program
    {
        public static void Main(string[] args)
        {

            //  Console.WriteLine("DB Path: " + Path.GetFullPath("attendance.db"));

           

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=attendance.db"));

            var app = builder.Build(); // <-- This is 'app'

            // Run database migrations at startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate(); // Ensures tables are created / migrations applied
            }

            // Configure middleware
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();


            //var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddControllers();
            //builder.Services.AddEndpointsApiExplorer();
            //object value = builder.Services.AddSwaggerGen();

            //var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseAuthorization();
            //app.MapControllers();

            //app.Run();



        }
    }

}