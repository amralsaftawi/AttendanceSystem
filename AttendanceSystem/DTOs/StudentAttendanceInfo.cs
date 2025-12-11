using AttendanceSystem.Data;

namespace AttendanceSystem.DTOs
{
    public class StudentAttendanceInfo
        {
            public int LectureID { get; set; }
            public string CourseName { get; set; }
            public DateTime LectureStartTime { get; set; }
            public DateTime ScanTime { get; set; }
            public string Room { get; set; }



        public static List<StudentAttendanceInfo> GetStudentAttendance(int studentId)
        {
            using var context = new AppDbContext();

            var student = context.Students.FirstOrDefault(s => s.ID == studentId);

            if (student == null)
                return new List<StudentAttendanceInfo>();

            var attendanceData = context.Attendance
                .Where(a => a.StudentID == studentId)
                .Join(context.Lectures,
                      a => a.LectureID,
                      l => l.ID,
                      (a, l) => new { a, l })
                .Join(context.Courses,
                      al => al.l.CourseID,
                      c => c.ID,
                      (al, c) => new StudentAttendanceInfo
                      {
                          LectureID = al.l.ID,
                          CourseName = c.Name,
                          LectureStartTime = al.l.StartTime,
                          ScanTime = al.a.ScanTime,
                          Room = al.l.Room
                      })
                .OrderByDescending(x => x.ScanTime)
                .ToList();

            return attendanceData;
        }

    }

}
