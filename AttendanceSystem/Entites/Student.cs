using AttendanceSystem.Data;

namespace AttendanceSystem.Entites
{
    public class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gmail { get; set; }
        public int DepartmentID {get;set;}
        public int Level { get; set; }
        public string NFC_Tag { get; set; }


        public static void AddStudent(string FirstName,string LastName,string Gmail,int DepartmentID,int Level,string NFC_Tag)
        {
            Student NewStudent = new Student
            {
                FirstName = FirstName,
                LastName= LastName, 
                Gmail = Gmail,
                DepartmentID = DepartmentID,
                 Level = Level, 
                 NFC_Tag = NFC_Tag 
            };

            var context = new AppDbContext();
            context.Students.Add(NewStudent);
            context.SaveChanges();   
        }

        public static void DeleteStudentByID(int ID)
        {
            var context = new AppDbContext();
            var studenttodelete = context.Students.SingleOrDefault(x => x.ID == ID);

            if (studenttodelete != null)
            {
                context.Students.Remove(studenttodelete);
                context.SaveChanges();
            } 
        }

        public static void UpdateStudent(int ID,string FirstName, string LastName, string Gmail, int DepartmentID, int Level, string NFC_Tag)
        {
            var context = new AppDbContext();
            var studenttoupdate = context.Students.SingleOrDefault(x => x.ID == ID);
            if (studenttoupdate != null)
            {
                studenttoupdate.FirstName = FirstName;
                studenttoupdate.LastName = LastName;
                studenttoupdate.Gmail = Gmail;
                studenttoupdate.DepartmentID = DepartmentID;
                studenttoupdate.Level = Level;
                studenttoupdate.NFC_Tag = NFC_Tag;
                context.SaveChanges();
            }
        } 

        public static Student GetStudentByID(int ID)
        {
            var context = new AppDbContext(); 
            var student = context.Students.SingleOrDefault(x => x.ID == ID); 
            return student;                             
        }


        public override string ToString()
        {
            return $"FirstName : {FirstName}  LastName: {LastName}  Gmail : {Gmail}  DepID: {DepartmentID}" +
                $"Level : {Level}  NFC: {NFC_Tag}";
        }
    }
    

}
