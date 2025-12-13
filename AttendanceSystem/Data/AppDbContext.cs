using AttendanceSystem.Entites;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Data
{
    public class AppDbContext:DbContext
    {

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<lecture> Lectures { get; set; }
        public DbSet<LectureSession> LectureSessions {  get; set; } 
        public DbSet<BlockedStudent> BlockedStudents { get ; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var Configration=new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            
            var envname = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"); 

            var ConnectionString = Configration.GetSection(envname=="Development"?"Const Dev" : "Const Prod");

            optionsBuilder.UseSqlite(ConnectionString.Value);

          

        }
    }
}
