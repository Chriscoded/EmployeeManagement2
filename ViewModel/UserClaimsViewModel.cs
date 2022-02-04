namespace EmployeeManagement2.ViewModel
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string? UserId { get; set; }
        //public string ClaimType { get; set; }
        //public bool IsSelected { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
