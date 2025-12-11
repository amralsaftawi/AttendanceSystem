using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class CourseService
    {
        // helper checks
        private static bool InstructorExists(int instructorId, AppDbContext context)
        {
            return context.Instructors.Any(i => i.ID == instructorId);
        }

        private static bool CourseCodeExists(string code, AppDbContext context, int? excludeCourseId = null)
        {
            return context.Courses.Any(c => c.Code == code && (!excludeCourseId.HasValue || c.ID != excludeCourseId.Value));
        }

        // 1. AddCourse
        public static string AddCourse(string Name, string Code, int InstructorID, int Level)
        {
            using var context = new AppDbContext();

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Code))
                return "Name and Code are required.";

            if (!InstructorExists(InstructorID, context))
                return "Instructor not found.";

            if (CourseCodeExists(Code, context))
                return "Course code already exists.";

            var course = new Entites.Course
            {
                Name = Name,
                Code = Code,
                InstructorID = InstructorID,
                Level = Level
            };

            context.Courses.Add(course);
            context.SaveChanges();

            return "Course added successfully.";
        }

        // 2. DeleteCourseByID
        public static string DeleteCourseByID(int ID)
        {
            using var context = new AppDbContext();

            var courseToDelete = context.Courses.FirstOrDefault(x => x.ID == ID);
            if (courseToDelete == null)
                return "Course not found.";

            // check related lectures/enrolments
            var hasLectures = context.Lectures.Any(l => l.CourseID == ID);
            var hasEnrolments = context.Enrolments.Any(e => e.CourseID == ID);

            if (hasLectures || hasEnrolments)
                return "Cannot delete course. There are related lectures or enrolments.";

            context.Courses.Remove(courseToDelete);
            context.SaveChanges();

            return "Course deleted successfully.";
        }

        // 3. UpdateCourse (fixed name)
        public static string UpdateCourse(int ID, string Name, string Code, int InstructorID, int Level)
        {
            using var context = new AppDbContext();

            var course = context.Courses.FirstOrDefault(c => c.ID == ID);
            if (course == null)
                return "Course not found.";

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Code))
                return "Name and Code are required.";

            if (!InstructorExists(InstructorID, context))
                return "Instructor not found.";

            if (CourseCodeExists(Code, context, excludeCourseId: ID))
                return "Another course with same code exists.";

            // update fields (do NOT set ID)
            course.Name = Name;
            course.Code = Code;
            course.InstructorID = InstructorID;
            course.Level = Level;

            context.SaveChanges();

            return "Course updated successfully.";
        }

        // 4. GetAllCourses
        public static List<Course> GetAllCourse()
        {
            using var context = new AppDbContext();
            return context.Courses.ToList();
        }

        // 5. ChangeCourseInstructor (fixed name)
        public static string ChangeCourseInstructor(int CourseID, int NewInstructorID)
        {
            using var context = new AppDbContext();

            var course = context.Courses.FirstOrDefault(c => c.ID == CourseID);
            if (course == null)
                return "Course not found.";

            if (!InstructorExists(NewInstructorID, context))
                return "Instructor not found.";

            course.InstructorID = NewInstructorID;
            context.SaveChanges();

            return "Course instructor changed successfully.";
        }

        // 6. GetCourseByID
        public static Course? GetCourseByID(int ID)
        {
            using var context = new AppDbContext();
            return context.Courses.FirstOrDefault(c => c.ID == ID);
        }
    }
}
