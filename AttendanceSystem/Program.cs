using AttendanceSystem.Data;
using AttendanceSystem.Entites;
using AttendanceSystem.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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