namespace AttendanceSystem.DTOs
{

    public class StudentCourseAttendance
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        public int TotalLectures { get; set; }
        public int AttendedLectures { get; set; }
        public int AbsentLectures { get; set; }

        public double AttendancePercentage { get; set; }

        public override string ToString()
        {
            return $"CourseId: {CourseID} CourseName: {CourseName} " +
                $"Total: {TotalLectures}  Attendec: {AttendedLectures}" +
                $"Absent: {AbsentLectures}  Percentage: {AttendancePercentage}% "; 
        }
    }
}
