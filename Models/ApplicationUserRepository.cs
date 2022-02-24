namespace EmployeeManagement2.Models
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly AppDbContext context;

        public ApplicationUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return context.applicationUsers;
        }

        public Employee GetUser(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
