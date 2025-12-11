using AttendanceSystem.Data;

namespace AttendanceSystem.Entites
{
    public class lecture
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Room { get; set; }


        public static void AddLecture(int courseID, DateTime startTime, DateTime endTime, string room)
        {
            lecture NewLectur = new lecture()
            {
                CourseID = courseID,
                StartTime = startTime,
                EndTime = endTime,
                Room = room
            };

            using (var Context = new AppDbContext())
            {
                Context.Lectures.Add(NewLectur);
                Context.SaveChanges();
            }
        }

       public static void DeleteLectureByID(int ID)
        {
            using (var Context = new AppDbContext())
            {
                var lecturetodelete = Context.Lectures.SingleOrDefault(x => x.ID == ID);
                if (lecturetodelete != null)
                {
                    Context.Lectures.Remove(lecturetodelete);
                    Context.SaveChanges();
                }
            }
        } 


        public override string ToString()
        {
            return $"ID {ID}  CourseID : {CourseID}  StartTime : {StartTime}  EndTime : {EndTime}  Room : {Room}";
        }
    }
}