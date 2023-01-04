using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Project.COMMON.Tools
{
    public static class MailService
    {
        public static void Send(string receiver,string password="echo.book78",string body="Test mesajıdır",string subject="Email Testi",string sender="echobook87@gmail.com")
        {
            MailAddress senderEmail = new MailAddress(sender);
            MailAddress receiverEmail = new MailAddress(receiver);

            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address,password)
            };

            using(MailMessage message = new MailMessage(senderEmail, receiverEmail) 
            {
                Subject = subject,
                Body = body,
                
            })
            {
                smtp.Send(message);
            }

        }

        public static void Send(object email, string body, string subject)
        {
            throw new NotImplementedException();
        }
    }
}