namespace EmployeeManagement2.Services
{
    public interface ISmsSender
    {
        public Task<bool> SendSmsAsync(string number, string message);
    }
}
