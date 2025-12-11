namespace AttendanceSystem.DTOs
{
    public class StudentMonthlyAttendance
    {

        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }

        public int TotalLectures { get; set; }
        public int AttendedLectures { get; set; }
        public int AbsentLectures { get; set; }

        public double AttendancePercentage { get; set; }


        public override string ToString()
        {
            return $"Year: {Year} Month: {MonthName} " +
                $"Total: {TotalLectures}  Attendec: {AttendedLectures}" +
                $"Absent: {AbsentLectures}  Percentage: {AttendancePercentage}% "; 
        }
    }
}
