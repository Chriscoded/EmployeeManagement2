namespace EmployeeManagement2.Models
{
    public interface IApplicationUserRepository
    {
        Employee GetUser(int Id);
        IEnumerable<ApplicationUser> GetAllUsers();
       
    }
}
