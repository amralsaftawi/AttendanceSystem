using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class InstructorService
    {
        // ========================= Helpers =========================

        private static bool IsInstructorExist(int id, AppDbContext context)
        {
            return context.Instructors.Any(i => i.ID == id);
        }

        private static bool IsGmailExist(string gmail, AppDbContext context)
        {
            return context.Instructors.Any(i => i.Gmail == gmail);
        }

     

        // ========================= Add =========================

        public static string AddInstructor(string FirstName, string LastName, string Gmail)
        {
            using var context = new AppDbContext();

            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Gmail))
            {
                return "Invalid data.";
            }

            if (IsGmailExist(Gmail, context))
                return "This Gmail is already used by another instructor.";

            var instructor = new Instructor
            {
                FirstName = FirstName,
                LastName = LastName,
                Gmail = Gmail
            };

            context.Instructors.Add(instructor);
            context.SaveChanges();

            return "Instructor added successfully.";
        }

        // ========================= Delete =========================

        public static string DeleteInstructorByID(int ID)
        {
            using var context = new AppDbContext();

            if (!IsInstructorExist(ID, context))
                return "Instructor not found.";

    

            var instructor = context.Instructors
                                    .FirstOrDefault(x => x.ID == ID);

            if (instructor == null)
                return "Instructor not found.";

            context.Instructors.Remove(instructor);
            context.SaveChanges();

            return "Instructor deleted successfully.";
        }

        // ========================= Update =========================

        public static string UpdateInstructor(int ID, string FirstName, string LastName, string Gmail)
        {
            using var context = new AppDbContext();

            var instructor = context.Instructors
                                    .FirstOrDefault(x => x.ID == ID);

            if (instructor == null)
                return "Instructor not found.";

            if (instructor.Gmail != Gmail && IsGmailExist(Gmail, context))
                return "This Gmail is already used by another instructor.";

            instructor.FirstName = FirstName;
            instructor.LastName = LastName;
            instructor.Gmail = Gmail;

            context.SaveChanges();

            return "Instructor updated successfully.";
        }

        // ========================= Get All =========================

        public static List<Instructor> GetAllInstructors()
        {
            using var context = new AppDbContext();

            return context.Instructors.ToList();
        }

        // ========================= Get By ID =========================

        public static Instructor? GetInstructorByID(int ID)
        {
            using var context = new AppDbContext();

            return context.Instructors
                          .FirstOrDefault(x => x.ID == ID);
        }
    }
}
