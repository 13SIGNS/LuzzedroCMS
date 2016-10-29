using LuzzedroCMS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace LuzzedroCMS.Concrete
{
    public class EmailSender : IEmailSender
    {
        MailMessage message = new MailMessage();

        public void AddTo(string to)
        {
            message.To.Add(to);
        }

        public void IsBodyHtml(bool isBodyHtml)
        {
            message.IsBodyHtml = isBodyHtml;
        }

        public bool SendEmail()
        {
            bool feedback = true;
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (Exception e)
            {
                feedback = false;
            }
            return feedback;
        }

        public void SetContent(string content)
        {
            message.Body = content;
        }

        public void SetReplyTo(string replyTo)
        {
            message.ReplyToList.Add(replyTo);
        }

        public void SetSubject(string subject)
        {
            message.Subject = subject;
        }
    }
}