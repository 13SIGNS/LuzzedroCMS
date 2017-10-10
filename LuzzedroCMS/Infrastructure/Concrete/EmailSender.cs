using LuzzedroCMS.Infrastructure.Abstract;
using System.Net.Mail;

namespace LuzzedroCMS.Infrastructure.Concrete
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
            using (SmtpClient smtp = new SmtpClient())
            {
                try
                {
                    smtp.Send(message);
                }
                catch
                {
                    feedback = false;
                }
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