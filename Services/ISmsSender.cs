namespace EmployeeManagement2.Services
{
    public interface ISmsSender
    {
        public Task SendSmsAsync(string number, string message);
    }
}
