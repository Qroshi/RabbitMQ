using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace EmailLibrary
{
    public class Email
    {
        public string emailTo;
        public string emailSubject;
        public string emailBody;

        public Email() { }
        public Email(string emailTo, string emailSubject, string emailBody)
        {
            this.emailTo = emailTo;
            this.emailSubject = emailSubject;
            this.emailBody = emailBody;
        }

        public string EmailToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void JSONToEmail(string json)
        {
            var email = JsonConvert.DeserializeObject<Email>(json);

            this.emailTo = email.emailTo;
            this.emailSubject = email.emailSubject;
            this.emailBody = email.emailBody;
        }

        public void Send(string emailFrom)
        {
            MailMessage message = new MailMessage(emailFrom, this.emailTo);

            message.Subject = this.emailSubject;
            message.Body = this.emailBody;

            using (SmtpClient client = new SmtpClient("smtp.gmail.com")
                                            {
                                                Port = 587,
                                                Credentials = new NetworkCredential("szymon.j.jedrzejewski@gmail.com", "qnjvvvkwmhidjsol"),
                                                EnableSsl = true,
                                            })
            {

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}