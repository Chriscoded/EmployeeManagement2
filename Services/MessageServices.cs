using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
namespace EmployeeManagement2.Services
{
    public class MessageServices : IEmailSender, ISmsSender
    {
        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                //From Address    
                string FromAddress = "amaemechris@gmail.com";
                string FromAdressTitle = "Employee Management";
                //To Address    
                string ToAddress = email;
                string ToAdressTitle = email;
                string Subject = subject;
                string BodyContent = message;

                //Smtp Server    
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number    
                int SmtpPortNumber = 465;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress
                                        (FromAdressTitle,
                                         FromAddress
                                         ));
                mimeMessage.To.Add(new MailboxAddress
                                         (ToAdressTitle,
                                         ToAddress
                                         ));
                mimeMessage.Subject = Subject; //Subject  
                mimeMessage.Body = new TextPart("html")
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, true);
                    client.Authenticate("amaemechris@gmail.com", ""
                        );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
                
            }
            
        }

        public async Task SendSmsAsync(string number, string message)
        {
            throw new NotImplementedException();
        }
    }
}
