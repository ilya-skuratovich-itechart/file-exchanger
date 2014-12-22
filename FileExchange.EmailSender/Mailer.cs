using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FileExchange.EmailSender
{
    public class Mailer:IMailer
    {
        protected string UserName { get; set; }
        protected string UserPass { get; set; }
        protected string HostAddr { get; set; }
        protected int SmtpPort { get; set; }
        protected string FakeFakeRecepient { get; set; }
        protected bool UsingFakeRecepient { get; set; }

        public Mailer()
        {
            
        }

        public Mailer(
            bool? usingFakeRecepient = null, string fakeRecepient = null)
        {
            FakeFakeRecepient = fakeRecepient;
            UsingFakeRecepient = usingFakeRecepient ?? false;
        }

        public Mailer(string userName, string userPass, string hostAddr, int smtpPort, 
            bool? usingFakeRecepient=null, string fakeRecepient = null)
        {
            UserName = userName;
            HostAddr = hostAddr;
            SmtpPort = smtpPort;
            UserPass = userPass;
            FakeFakeRecepient = fakeRecepient;
            UsingFakeRecepient = usingFakeRecepient??false;
        }

        public void SendEmailTo(string sendEmailTo, string subject, string text, Array cc = null)
        {

            if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(UserPass) && string.IsNullOrEmpty(subject) &&
                string.IsNullOrEmpty(text) && string.IsNullOrEmpty(HostAddr) && SmtpPort > 0)
                throw new Exception("Icorrect sending data");

            if (UsingFakeRecepient)
                sendEmailTo = FakeFakeRecepient;

            var smtpClient = new SmtpClient(HostAddr);
            var mailMessage = new MailMessage();
            if (cc != null && cc.Length > 0)
            {
                foreach (var mailCopy in cc)
                {
                    mailMessage.CC.Add(mailCopy.ToString());
                }
            }
            smtpClient.Timeout = 10000;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Port = SmtpPort;
            mailMessage.IsBodyHtml = true;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            smtpClient.Credentials = new NetworkCredential(UserName, UserPass);
            mailMessage.To.Add(sendEmailTo);
            mailMessage.From = new MailAddress(UserName);
            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = UTF8Encoding.UTF8;
            mailMessage.Body = text;
            smtpClient.Send(mailMessage);
        }
    }
}