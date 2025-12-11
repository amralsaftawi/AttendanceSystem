using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class TeacherControlService
    {

        private static bool IsLectureExist(int lectureId, AppDbContext context)
        {
            return context.Lectures.Any(l => l.ID == lectureId);
        }

        private static bool IsLectureAlreadyOpen(int lectureId, AppDbContext context)
        {
            return context.LectureSessions.Any(s => s.LectureID == lectureId && s.IsOpen);
        }

        private static void CreateLectureSession(int lectureId, AppDbContext context)
        {
            var session = new LectureSession
            {
                LectureID = lectureId,
                IsOpen = true,
                StartedAt = DateTime.Now
            };

            context.LectureSessions.Add(session);
            context.SaveChanges();
        }

        public static bool OpenLecture(int lectureId)
        {
            using var context = new AppDbContext();

            if (!IsLectureExist(lectureId, context))
                return false;

            if (IsLectureAlreadyOpen(lectureId, context))
                return false;

            CreateLectureSession(lectureId, context);

            return true;
        }

        private static LectureSession? GetOpenLectureSession(int lectureId, AppDbContext context)
        {
            return context.LectureSessions
                .FirstOrDefault(s => s.LectureID == lectureId && s.IsOpen);
        }

        public static bool CloseLecture(int lectureId)
        {
            using var context = new AppDbContext();

            var session = GetOpenLectureSession(lectureId, context);

            if (session == null)
                return false;

            session.IsOpen = false;
            session.ClosedAt = DateTime.Now;

            context.SaveChanges();

            return true;
        }

        private static lecture? GetLectureById(int lectureId, AppDbContext context)
        {
            return context.Lectures.FirstOrDefault(l => l.ID == lectureId);
        }

        public static bool ChangeLectureRoom(int lectureId, string newRoom)
        {
            using var context = new AppDbContext();

            if (string.IsNullOrWhiteSpace(newRoom))
                return false;

            var lecture = GetLectureById(lectureId, context);

            if (lecture == null)
                return false;

            lecture.Room = newRoom;

            context.SaveChanges();

            return true;
        }

        private static bool IsAttendanceAlreadyRecorded(int studentId, int lectureId, AppDbContext context)
        {
            return context.Attendance.Any(a => a.StudentID == studentId && a.LectureID == lectureId);
        }

        private static bool IsStudentExist(int studentId, AppDbContext context)
        {
            return context.Students.Any(s => s.ID == studentId);
        }

        private static bool IsStudentBlocked(int studentId, int lectureId, AppDbContext context)
        {
            return context.BlockedStudents.Any
                (b => b.StudentID == studentId && b.LectureID == lectureId);
        }

        private static void CreateAttendanceRecord(int studentId, int lectureId, DateTime time, AppDbContext context)
        {
            var attendance = new Attendance
            {
                StudentID = studentId,
                LectureID = lectureId,
                ScanTime = time,
                Status = "Present"
            };

            context.Attendance.Add(attendance);
            context.SaveChanges();
        }

        public static string ForceRegisterAttendance(int studentId, int lectureId)
        {
            using var context = new AppDbContext();
            var now = DateTime.Now;

            if (!IsStudentExist(studentId, context))
                return "Student not found.";

            if (!IsLectureExist(lectureId, context))
                return "Lecture not found.";

            if (IsStudentBlocked(studentId, lectureId, context))
                return "Student is blocked from this lecture.";

            if (IsAttendanceAlreadyRecorded(studentId, lectureId, context))
                return "Attendance already exists.";

            CreateAttendanceRecord(studentId, lectureId, now, context);

            return "Attendance forced successfully.";
        }

        private static Attendance? GetStudentAttendanceRecord(int studentId, int lectureId, AppDbContext context)
        {
            return context.Attendance
                .SingleOrDefault(a => a.StudentID == studentId && a.LectureID == lectureId);
        }

        public static string MarkStudentAbsent(int studentId, int lectureId)
        {
            using var context = new AppDbContext();

            var attendance = GetStudentAttendanceRecord(studentId, lectureId, context);

            if (attendance == null)
                return "Student is already absent.";

            context.Attendance.Remove(attendance);
            context.SaveChanges();

            return "Student marked as absent.";
        }

        public static string RemoveAttendance(int studentId, int lectureId)
        {
            using var context = new AppDbContext();

            var record = GetStudentAttendanceRecord(studentId, lectureId, context);

            if (record == null)
                return "No attendance record found.";

            context.Attendance.Remove(record);
            context.SaveChanges();

            return "Attendance record removed.";
        }

        public static string MarkStudentLate(int studentId, int lectureId)
        {
            using var context = new AppDbContext();

            var attendance = GetStudentAttendanceRecord(studentId, lectureId, context);

            if (attendance == null)
                return "Student is not registered as present.";

            attendance.Status = "Late";
            context.SaveChanges();

            return "Student marked late.";
        }

        public static List<Student> GetPresentStudents(int lectureId)
        {
            using var context = new AppDbContext();

            if (!IsLectureExist(lectureId, context))
                return new List<Student>();

            return context.Attendance
                  .Where(a => a.LectureID == lectureId)
                  .Join(context.Students,
                        a => a.StudentID,
                        s => s.ID,
                        (a, s) => s)
                  .ToList();
        }

        public static List<Student> GetAbsentStudents(int lectureId)
        {
            using var context = new AppDbContext();

            var lecture = context.Lectures.FirstOrDefault(l => l.ID == lectureId);

            if (lecture == null)
                return new List<Student>();

            var allStudentsInCourse = context.Enrolments
                .Where(e => e.CourseID == lecture.CourseID)
                .Join(context.Students,
                      e => e.StudentID,
                      s => s.ID,
                      (e, s) => s)
                .ToList();

            var presentStudentIds = context.Attendance
                .Where(a => a.LectureID == lectureId)
                .Select(a => a.StudentID)
                .ToList();

            return allStudentsInCourse
                .Where(s => !presentStudentIds.Contains(s.ID))
                .ToList();
        }

        public static string BlockStudentFromLecture(int studentId, int lectureId)
        {
            using var context = new AppDbContext();

            if (!IsStudentExist(studentId, context))
                return "Student not found.";

            if (!IsLectureExist(lectureId, context))
                return "Lecture not found.";

            if (IsStudentBlocked(studentId, lectureId, context))
                return "Student already blocked.";

            var BlockedStudent = new BlockedStudent
            {
                StudentID = studentId,
                LectureID = lectureId,
                BlockedAt = DateTime.Now
            };

            context.BlockedStudents.Add(BlockedStudent);
            context.SaveChanges();

            return "Student blocked from lecture.";
        }
    }
}
