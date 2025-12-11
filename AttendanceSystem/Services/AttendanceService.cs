using AttendanceSystem.Data;
using AttendanceSystem.DTOs;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class AttendanceService
    {

        private static Student? GetStudentByNfc(string NFC_Tag, AppDbContext context)
        {
            return context.Students.FirstOrDefault(s => s.NFC_Tag == NFC_Tag);
        }


        private static List<lecture> GetOngoingLectures(DateTime currentTime, AppDbContext context)
        {
            return context.Lectures
                .Where(l => l.StartTime <= currentTime && l.EndTime >= currentTime)
                .ToList();
        }


        private static lecture? GetValidLectureForStudent(List<lecture> lectures, int studentLevel, AppDbContext context)
        {


            var validLecture = lectures
                .Join(context.Courses,
                      l => l.CourseID,
                      c => c.ID,
                      (l, c) => new { l, c })
                .FirstOrDefault(x => x.c.Level == studentLevel)?.l;

            return validLecture;
        }


        private static bool IsStudentEnrolled(int studentId, int courseId, AppDbContext context)
        {
            return context.Enrolments.Any(e => e.StudentID == studentId && e.CourseID == courseId);
        }


        private static bool IsAttendanceAlreadyRecorded(int studentId, int lectureId, AppDbContext context)
        {
            return context.Attendance.Any(a => a.StudentID == studentId && a.LectureID == lectureId);
        }

        private static bool IsLectureOpen(int lectureId, AppDbContext context)
        {
            return context.LectureSessions.Any(s =>
                s.LectureID == lectureId && s.IsOpen == true);
        }

        private static void SaveAttendance(int studentId, int lectureId, DateTime scanTime, AppDbContext context)
        {
            var attendance = new Attendance
            {
                StudentID = studentId,
                LectureID = lectureId,
                ScanTime = scanTime,
                Status="Present"
            };

            context.Attendance.Add(attendance);
            context.SaveChanges();
        }


        public static string AttendanceRegistration(string NFC_Tag)
        {
            using var context = new AppDbContext();
            var now = DateTime.Now;

            var student = GetStudentByNfc(NFC_Tag, context);
            if (student == null)
            {
                return "Invalid NFC Tag.";
                
            }

            var lectures = GetOngoingLectures(now, context);
            if (!lectures.Any())
            {
                return "No Ongoing lectures.";
            }

            var validLecture = GetValidLectureForStudent(lectures, student.Level, context);
            if (validLecture == null)
            {
                return"No matching lecture for level.";
            }

            if (!IsLectureOpen(validLecture.ID, context))
            {
                return "Lecture is closed.";

            }

            if (!IsStudentEnrolled(student.ID, validLecture.CourseID, context))
            {
                return "Student not enrolled.";
            }

            if (IsAttendanceAlreadyRecorded(student.ID, validLecture.ID, context))
            {
              return "Already registered.";
              
            }

            SaveAttendance(student.ID, validLecture.ID, now, context);

            return " Attendance saved successfully.";
        }



    }
}
