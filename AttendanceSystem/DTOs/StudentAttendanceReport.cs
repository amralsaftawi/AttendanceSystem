using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.DTOs
{
    public class StudentAttendanceReport
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public int Level { get; set; }

        public int TotalLectures { get; set; }
        public int AttendedLectures { get; set; }
        public int AbsentLectures { get; set; }

        public double AttendancePercentage { get; set; }
        public string Status { get; set; }



        public override string ToString()
        {
            return $"Id: {StudentID} Name:{StudentName} Level {Level} " +
                $"Total: {TotalLectures}  Attendec: {AttendedLectures}" +
                $"Absent: {AbsentLectures}  Percentage: {AttendancePercentage}% " + $"Status: {Status}";   

        }
    }

}
