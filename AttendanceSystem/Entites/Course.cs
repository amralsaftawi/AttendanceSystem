using AttendanceSystem.Data;

namespace AttendanceSystem.Entites
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int InstructorID { get; set; }
        public int Level { get; set; }


        public static void AddCourse(string Name, string Code, int InstructorID, int Level)
        {
            Course NewCourse = new Course
            {
                Name = Name,
                Code = Code,
                InstructorID = InstructorID,
                Level = Level
            };
            var context = new AppDbContext();
            context.Courses.Add(NewCourse);
            context.SaveChanges();
        } 

        public static void DeleteCourseByID(int ID)
        {
            var context = new AppDbContext();
            var coursetodelete = context.Courses.SingleOrDefault(x => x.ID == ID);
            if (coursetodelete != null)
            {
                context.Courses.Remove(coursetodelete);
                context.SaveChanges();
            }
        } 

        public static void UpdateCourse(int ID, string Name, string Code, int InstructorID, int Level)
        {
            var context = new AppDbContext();
            var coursetoupdate = context.Courses.SingleOrDefault(x => x.ID == ID);
            if (coursetoupdate != null)
            {
                coursetoupdate.Name = Name;
                coursetoupdate.Code = Code;
                coursetoupdate.InstructorID = InstructorID;
                coursetoupdate.Level = Level;
                context.SaveChanges();
            }
        } 

        
        public override string ToString()
        {
            return $"ID {ID}  Name : {Name}  Code : {Code}  InstructorID : {InstructorID}  Level {Level}";
        }
    }
}