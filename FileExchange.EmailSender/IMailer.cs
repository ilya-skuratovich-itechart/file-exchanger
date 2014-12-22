using System;

namespace FileExchange.EmailSender
{
    public interface IMailer
    {
        void SendEmailTo(string sendEmailTo, string subject, string text, Array cc = null);

    }
}