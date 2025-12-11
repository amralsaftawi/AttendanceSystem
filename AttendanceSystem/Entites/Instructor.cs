namespace AttendanceSystem.Entites
{
    public class Instructor
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gmail { get; set; }


        public override string ToString()
        {
            return $"ID {ID} ,FName : {FirstName} , LName : {LastName}  , Gmail: {Gmail}";
        }
    }
}
