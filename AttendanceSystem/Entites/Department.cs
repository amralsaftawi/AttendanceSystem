namespace AttendanceSystem.Entites
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return $"ID {ID}  Name : {Name}";
        }
    }
}
