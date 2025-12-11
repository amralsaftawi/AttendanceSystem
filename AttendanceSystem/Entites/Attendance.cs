using AttendanceSystem.Data;

namespace AttendanceSystem.Entites
{
    public class Attendance
    {

        public int ID { get; set; }
        public int LectureID { get; set; }
        public int StudentID { get; set; }
        public DateTime ScanTime { get; set; }
        public string? Status { get; set; } 




        public override string ToString()
        {
            return $"ID {ID}  LectureID : {LectureID}  StudentID : {StudentID}  ScanTime : {ScanTime} statuse {Status}";
        }


}
}