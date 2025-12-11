namespace AttendanceSystem.Entites
{
    public class LectureSession
    {
        public int ID{get;set;}
        public int LectureID { get;set;} 
        public DateTime StartedAt { get;set;}
        public DateTime? ClosedAt { get; set; }
        public bool IsOpen { get; set; } 




    }
}
