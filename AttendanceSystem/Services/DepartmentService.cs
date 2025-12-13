using AttendanceSystem.Data;
using AttendanceSystem.Entites;

namespace AttendanceSystem.Services
{
    public class DepartmentService
    {

        public void AddDepartment(string name,AppDbContext context)
        {
           context.Departments.Add(new Entites.Department { Name = name }); 
              context.SaveChanges(); 
        }

        public void DeleteDepartment(int id, AppDbContext context)
        {
            var department = context.Departments. FirstOrDefault(d => d.ID == id); 
       
            if (department != null)
            {
                context.Departments.Remove(department);
                context.SaveChanges(); 
            } 
        }

        public List<Department>GetAllDepartments (AppDbContext context)
        {
            return context.Departments.ToList();
        } 

        public Department? GetDepartmentById(int id, AppDbContext context)
        {
            return context.Departments.FirstOrDefault(d => d.ID == id);
        } 
        public List<Department> GetDepartmentByName(string name, AppDbContext context)
        {
            return context.Departments.Where(x=>x.Name.Contains(name)).ToList();
        } 


    }
}
