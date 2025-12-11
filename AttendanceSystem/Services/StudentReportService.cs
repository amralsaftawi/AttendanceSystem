using AttendanceSystem.Data;
using AttendanceSystem.DTOs;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class StudentReportService
    {
        private static Student? GetStudentById(int studentId, AppDbContext context)
        {
            return context.Students.FirstOrDefault(s => s.ID == studentId);
        }

        private static int GetTotalLectures(int studentId, AppDbContext context)
        {
            return context.Enrolments
                .Where(e => e.StudentID == studentId)
                .Join(context.Lectures,
                      e => e.CourseID,
                      l => l.CourseID,
                      (e, l) => l)
                .Count();
        }

        private static int GetAttendedLectures(int studentId, AppDbContext context)
        {
            return context.Attendance
                          .Count(a => a.StudentID == studentId);
        }

        private static double CalculateAttendancePercentage(int total, int attended)
        {
            if (total == 0)
                return 0;

            return (double)attended / total * 100;
        }

        //--------------------------------------

        public static StudentAttendanceReport? GetStudentBasicReport(int studentId)
        {
            using var context = new AppDbContext();

            var student = GetStudentById(studentId, context);

            if (student == null)
                return null;

            int totalLectures = GetTotalLectures(studentId, context);
            int attendedLectures = GetAttendedLectures(studentId, context);
            int absentLectures = totalLectures - attendedLectures;

            double attendancePercentage =
                CalculateAttendancePercentage(totalLectures, attendedLectures);

            string status =
                attendancePercentage >= 75 ? "Excellent" :
                attendancePercentage >= 50 ? "Warning" :
                                           "Danger";

            return new StudentAttendanceReport
            {
                StudentID = student.ID,
                StudentName = $"{student.FirstName} {student.LastName}",
                Level = student.Level,

                TotalLectures = totalLectures,
                AttendedLectures = attendedLectures,
                AbsentLectures = absentLectures,

                AttendancePercentage = Math.Round(attendancePercentage, 2),
                Status = status
            };
        }

        //--------------------------------------

        private static List<Course> GetStudentCourses(int studentId, AppDbContext context)
        {
            return context.Enrolments
                .Where(e => e.StudentID == studentId)
                .Join(context.Courses,
                      e => e.CourseID,
                      c => c.ID,
                      (e, c) => c)
                .Distinct()
                .ToList();
        }

        private static int GetTotalLecturesForCourse(int courseId, AppDbContext context)
        {
            return context.Lectures.Count(l => l.CourseID == courseId);
        }

        private static int GetAttendedLecturesInCourse(int studentId, int courseId, AppDbContext context)
        {
            return context.Attendance
                .Join(context.Lectures,
                      a => a.LectureID,
                      l => l.ID,
                      (a, l) => new { a.StudentID, l.CourseID })
                .Count(x => x.StudentID == studentId && x.CourseID == courseId);
        }

        //--------------------------------------

        public static List<StudentCourseAttendance> GetStudentAttendanceReportByCourse(int studentId)
        {
            using var context = new AppDbContext();

            var courses = GetStudentCourses(studentId, context);

            var result = new List<StudentCourseAttendance>();

            foreach (var course in courses)
            {
                int totalLectures = GetTotalLecturesForCourse(course.ID, context);
                int attendedLectures = GetAttendedLecturesInCourse(studentId, course.ID, context);

                int absent = totalLectures - attendedLectures;

                double percentage =
                    CalculateAttendancePercentage(totalLectures, attendedLectures);

                result.Add(new StudentCourseAttendance
                {
                    CourseID = course.ID,
                    CourseName = course.Name,

                    TotalLectures = totalLectures,
                    AttendedLectures = attendedLectures,
                    AbsentLectures = absent,

                    AttendancePercentage = Math.Round(percentage, 2)
                });
            }

            return result;
        }

        //--------------------------------------

        private static List<(int Year, int Month)> GetStudentLectureMonths(int studentId, AppDbContext context)
        {
            return context.Attendance
                .Where(a => a.StudentID == studentId)
                .Select(a => new { a.ScanTime.Year, a.ScanTime.Month })
                .Distinct()
                .AsEnumerable()
                .Select(x => (x.Year, x.Month))
                .ToList();
        }

        private static int GetTotalLecturesInMonth(int studentId, int year, int month, AppDbContext context)
        {
            var courseIds = context.Enrolments
                .Where(e => e.StudentID == studentId)
                .Select(e => e.CourseID)
                .ToList();

            return context.Lectures
                .Where(l => courseIds.Contains(l.CourseID) &&
                            l.StartTime.Year == year &&
                            l.StartTime.Month == month)
                .Count();
        }

        private static int GetAttendedLecturesInMonth(int studentId, int year, int month, AppDbContext context)
        {
            return context.Attendance
                .Count(a => a.StudentID == studentId &&
                            a.ScanTime.Year == year &&
                            a.ScanTime.Month == month);
        }

        private static string GetMonthName(int month)
        {
            return new DateTime(2025, month, 1).ToString("MMMM");
        }

        //--------------------------------------

        public static List<StudentMonthlyAttendance> GetStudentAttendanceByMonth(int studentId)
        {
            using var context = new AppDbContext();

            var months = GetStudentLectureMonths(studentId, context);

            var result = new List<StudentMonthlyAttendance>();

            foreach (var m in months)
            {
                int totalLectures = GetTotalLecturesInMonth(studentId, m.Year, m.Month, context);
                int attendedLectures = GetAttendedLecturesInMonth(studentId, m.Year, m.Month, context);

                int absent = totalLectures - attendedLectures;

                double percentage =
                    CalculateAttendancePercentage(totalLectures, attendedLectures);

                result.Add(new StudentMonthlyAttendance
                {
                    Year = m.Year,
                    Month = m.Month,
                    MonthName = GetMonthName(m.Month),

                    TotalLectures = totalLectures,
                    AttendedLectures = attendedLectures,
                    AbsentLectures = absent,

                    AttendancePercentage = Math.Round(percentage, 2)
                });
            }

            return result
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();
        }
    }
}
