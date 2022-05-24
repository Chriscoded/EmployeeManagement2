using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Vonage;
using Vonage.Request;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement2.Services
{
    public class MessageServices : IEmailSender, ISmsSender
    {
       

        public SMSoptions Options { get; } // set only via Secret Manager
        public IWebHostEnvironment Env { get; }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            //try
            //{
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
                    client.Authenticate("amaemechris@gmail.com", "Password"
                        );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
                
            //}
            //catch (Exception ex)
            //{
                return false;
                //throw ex;
                
            //}
            
        }

        public async Task<bool> SendSmsAsync(string number, string message)
        {

            var credentials = Credentials.FromApiKeyAndSecret(
            "e69c356a",
            "Fgy2TO5Y1tXoGAd3"
            );

            var VonageClient = new VonageClient(credentials);

            var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
            {
                To = number,
                From = "Chris",
                Text = message
            });

            return true;
        }
    }
}
