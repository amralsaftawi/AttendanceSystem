using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class StudentService
    {
        // ========================= Get Helpers =========================

        private static bool IsDepartmentExist(int departmentId, AppDbContext context)
        {
            return context.Departments.Any(d => d.ID == departmentId);
        }

        private static bool IsNfcExist(string nfc, AppDbContext context)
        {
            return context.Students.Any(s => s.NFC_Tag == nfc);
        }

        private static bool IsStudentExist(int id, AppDbContext context)
        {
            return context.Students.Any(s => s.ID == id);
        }

        private static bool HasRelations(int studentId, AppDbContext context)
        {
            return context.Attendance.Any(a => a.StudentID == studentId)
                || context.Enrolments.Any(e => e.StudentID == studentId);
        }

        // ========================= Add Student =========================

        public static string AddStudent(string FirstName, string LastName, string Gmail, int DepartmentID, string NFC_Tag, int Level)
        {
            using var context = new AppDbContext();

            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Gmail) ||
                string.IsNullOrWhiteSpace(NFC_Tag))
            {
                return "Invalid input data.";
            }

            if (!IsDepartmentExist(DepartmentID, context))
                return "Department does not exist.";

            if (IsNfcExist(NFC_Tag, context))
                return "This NFC already belongs to another student.";

            var student = new Student
            {
                FirstName = FirstName,
                LastName = LastName,
                Gmail = Gmail,
                DepartmentID = DepartmentID,
                NFC_Tag = NFC_Tag,
                Level = Level
            };

            context.Students.Add(student);
            context.SaveChanges();

            return "Student added successfully.";
        }

        // ========================= Delete Student =========================

        public static string DeleteStudentByID(int ID)
        {
            using var context = new AppDbContext();

            if (!IsStudentExist(ID, context))
                return "Student not found.";

            if (HasRelations(ID, context))
                return "Cannot delete student. Student has Attendance or Enrolments.";

            var student = context.Students.FirstOrDefault(x => x.ID == ID);

            if (student == null)
                return "Student not found.";

            context.Students.Remove(student);
            context.SaveChanges();

            return "Student deleted successfully.";
        }

        // ========================= Update Student =========================

        public static string UpdateStudent(int ID, string FirstName, string LastName, string Gmail, int DepartmentID, string NFC_Tag, int Level)
        {
            using var context = new AppDbContext();

            var student = context.Students.FirstOrDefault(x => x.ID == ID);

            if (student == null)
                return "Student not found.";

            if (!IsDepartmentExist(DepartmentID, context))
                return "Department does not exist.";

            if (student.NFC_Tag != NFC_Tag && IsNfcExist(NFC_Tag, context))
                return "This NFC already belongs to another student.";

            student.FirstName = FirstName;
            student.LastName = LastName;
            student.Gmail = Gmail;
            student.DepartmentID = DepartmentID;
            student.NFC_Tag = NFC_Tag;
            student.Level = Level;

            context.SaveChanges();

            return "Student updated successfully.";
        }

        // ========================= Get All Students =========================

        public static List<Student> GetAllStudents()
        {
            using var context = new AppDbContext();
            return context.Students.ToList();
        }

        // ========================= Get By Level =========================

        public static List<Student> GetAllStudentsByLevel(int Level)
        {
            using var context = new AppDbContext();
            return context.Students
                          .Where(s => s.Level == Level)
                          .ToList();
        }

        // ========================= Get By Department =========================

        public static List<Student> GetAllStudentsByDepartment(int DepartmentID)
        {
            using var context = new AppDbContext();

            if (!IsDepartmentExist(DepartmentID, context))
                return new List<Student>();

            return context.Students
                          .Where(s => s.DepartmentID == DepartmentID)
                          .ToList();
        }

        // ========================= Get By ID =========================

        public static Student? GetStudentByID(int ID)
        {
            using var context = new AppDbContext();

            return context.Students
                          .FirstOrDefault(x => x.ID == ID);
        }
    }
}
