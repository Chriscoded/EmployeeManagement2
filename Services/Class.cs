//namespace EmployeeManagement2.Services
//{
//    public class Class
//    {
//        public MailMessage ComposeMessageAsync(MessageStore messageStore, List<IFormFile> files)
//        {

//            string contentRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
//            var template = "";
//            //var webUrl = $"https://skills4pay.com";
//            var webUrl = $"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host}{_accessor.HttpContext.Request.PathBase}";
//            var bannerUrl = webUrl + "/assets/images/emailheader.png";
//            MailMessage message = new MailMessage();

//            if (messageStore.MessageType == MessageType.newsletter)
//            {
//                template = System.IO.File.ReadAllText(contentRootPath + "/template/newsletter.html");
//                if (string.IsNullOrEmpty(template))
//                {
//                    throw new NotSupportedException("Emtpy Template");
//                }


//                if (String.IsNullOrEmpty(messageStore.Banner))
//                {
//                    // messageStore.Banner = _generator.GetUriByPage(_accessor.HttpContext,
//                    //page: "/assets/images/emailheader.png",
//                    //handler: null,
//                    //values: new { area = "" });
//                    messageStore.Banner = bannerUrl;
//                }

//                if (String.IsNullOrEmpty(messageStore.Signature))
//                {
//                    messageStore.Signature = "Skills4Pay Team!";
//                }

//                if (messageStore.MessageType == MessageType.newsletter)
//                {

//                }

//                //if (files.Any())
//                //{

//                //    foreach(var file in files)
//                //    {
//                //        string fileName = Path.GetFileName(file.FileName);
//                //        message.Attachments.Add(new Attachment(file.OpenReadStream(), fileName));
//                //    }
//                //}
//                //var mailTemplate = template
//                //    .Replace("{banner}", messageStore.Banner)
//                //    .Replace("{message}", messageStore.Message)
//                //    .Replace("{link}", messageStore.Link)
//                //    .Replace("{signature}", messageStore.Signature)
//                //    .Replace("{currentYear}", DateTime.Now.Year.ToString());
//                //message.Subject = messageStore.Title;
//                //message.Body = mailTemplate;
//                //message.IsBodyHtml = true;
//                //message.From = new MailAddress("noreply@skills4pay.com", "Skills4Pay");

//                if (messageStore.AddressType == AddressType.Bulk)
//                {
//                    //foreach (var address in messageStore.EmailAddress.Split(','))
//                    //{
//                    if (files.Any())
//                    {

//                        foreach (var file in files)
//                        {
//                            string fileName = Path.GetFileName(file.FileName);
//                            message.Attachments.Add(new Attachment(file.OpenReadStream(), fileName));
//                        }
//                    }
//                    //var linko = messageStore.Link + "?email=" + address;
//                    var mailTemplate = template
//                        .Replace("{banner}", messageStore.Banner)
//                        .Replace("{message}", messageStore.Message)
//                        //.Replace("{link}", messageStore.Link)
//                        .Replace("{link}", messageStore.Link)
//                        .Replace("{signature}", messageStore.Signature)
//                        .Replace("{currentYear}", DateTime.Now.Year.ToString());
//                    message.Subject = messageStore.Title;
//                    message.Body = mailTemplate;
//                    message.IsBodyHtml = true;
//                    message.From = new MailAddress("noreply@skills4pay.com", "Skills4Pay");
//                    //message.To.Add("skillsforpay@gmail.com");
//                    message.Bcc.Add("skillsforpay@gmail.com");
//                    message.Bcc.Add(messageStore.EmailAddress);
//                    //foreach (var address in messageStore.EmailAddress.Split(','))
//                    //{
//                    //    message.To.Add(address);
//                    //    Thread.Sleep(2000);

//                    //}

//                    //messageStore.Link = messageStore.Link + "?email=" + address;

//                    //}
//                }
//                else if (messageStore.AddressType == AddressType.Single)
//                {
//                    if (files.Any())
//                    {

//                        foreach (var file in files)
//                        {
//                            string fileName = Path.GetFileName(file.FileName);
//                            message.Attachments.Add(new Attachment(file.OpenReadStream(), fileName));
//                        }
//                    }
//                    var linko = messageStore.Link + "?email=" + messageStore.EmailAddress;
//                    var mailTemplate = template
//                        .Replace("{banner}", messageStore.Banner)
//                        .Replace("{message}", messageStore.Message)
//                        //.Replace("{link}", messageStore.Link)
//                        .Replace("{link}", linko)
//                        .Replace("{signature}", messageStore.Signature)
//                        .Replace("{currentYear}", DateTime.Now.Year.ToString());
//                    message.Subject = messageStore.Title;
//                    message.Body = mailTemplate;
//                    message.IsBodyHtml = true;
//                    message.From = new MailAddress("noreply@skills4pay.com", "Skills4Pay");
//                    message.To.Add(messageStore.EmailAddress);
//                    //message.To.Add("skillsforpay@gmail.com");
//                }
//                else
//                {
//                    throw new NotSupportedException("AddressType");
//                }

//                return message;
//            }

//            else if (messageStore.MessageType == MessageType.allmail)
//            {
//                template = System.IO.File.ReadAllText(contentRootPath + "/template/allmail.html");
//                if (string.IsNullOrEmpty(template))
//                {
//                    throw new NotSupportedException("Emtpy Template");
//                }


//                if (String.IsNullOrEmpty(messageStore.Banner))
//                {
//                    // messageStore.Banner = _generator.GetUriByPage(_accessor.HttpContext,
//                    //page: "/assets/images/emailheader.png",
//                    //handler: null,
//                    //values: new { area = "" });
//                    messageStore.Banner = bannerUrl;
//                }

//                if (String.IsNullOrEmpty(messageStore.Signature))
//                {
//                    messageStore.Signature = "Skills4Pay Team!";
//                }

//                if (messageStore.MessageType == MessageType.allmail)
//                {

//                }
//                var mailTemplate = template
//                    .Replace("{banner}", messageStore.Banner)
//                    .Replace("{message}", messageStore.Message)
//                    .Replace("{signature}", messageStore.Signature)
//                    .Replace("{currentYear}", DateTime.Now.Year.ToString());
//                message.Subject = messageStore.Title;
//                message.Body = mailTemplate;
//                message.IsBodyHtml = true;
//                message.From = new MailAddress("noreply@skills4pay.com", "Skills4Pay");
//                message.To.Add("skillsforpay@gmail.com");


//                if (messageStore.AddressType == AddressType.Bulk)
//                {
//                    foreach (var address in messageStore.EmailAddress.Split(','))
//                    {
//                        message.Bcc.Add(address);
//                    }
//                }
//                else if (messageStore.AddressType == AddressType.Single)
//                {
//                    message.To.Add(messageStore.EmailAddress);
//                }
//                else
//                {
//                    throw new NotSupportedException("AddressType");
//                }

//                return message;
//            }

//            else if (messageStore.MessageType == MessageType.allmailnoattachment)
//            {
//                template = System.IO.File.ReadAllText(contentRootPath + "/template/allmail.html");
//                if (string.IsNullOrEmpty(template))
//                {
//                    throw new NotSupportedException("Emtpy Template");
//                }


//                if (String.IsNullOrEmpty(messageStore.Banner))
//                {
//                    // messageStore.Banner = _generator.GetUriByPage(_accessor.HttpContext,
//                    //page: "/assets/images/emailheader.png",
//                    //handler: null,
//                    //values: new { area = "" });
//                    messageStore.Banner = bannerUrl;
//                }

//                if (String.IsNullOrEmpty(messageStore.Signature))
//                {
//                    messageStore.Signature = "Skills4Pay Team!";
//                }

//                if (messageStore.MessageType == MessageType.change_emial)
//                {

//                }
//                var mailTemplate = template
//                    .Replace("{banner}", messageStore.Banner)
//                    .Replace("{message}", messageStore.Message)
//                    .Replace("{signature}", messageStore.Signature)
//                    .Replace("{currentYear}", DateTime.Now.Year.ToString());
//                message.Subject = messageStore.Title;
//                message.Body = mailTemplate;
//                message.IsBodyHtml = true;
//                message.From = new MailAddress("noreply@skills4pay.com", "Skills4Pay");


//                if (messageStore.AddressType == AddressType.Bulk)
//                {
//                    foreach (var address in messageStore.EmailAddress.Split(','))
//                    {
//                        message.Bcc.Add(address);
//                    }
//                }
//                else if (messageStore.AddressType == AddressType.Single)
//                {
//                    message.To.Add(messageStore.EmailAddress);
//                }
//                else
//                {
//                    throw new NotSupportedException("AddressType");
//                }

//                return message;
//            }

//            return message;

//        }

//    }
//}
