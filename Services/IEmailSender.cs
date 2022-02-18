namespace EmployeeManagement2.Services
{
    public interface IEmailSender
    {
        public Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
