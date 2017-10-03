namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface IEmailSender
    {
        void AddTo(string to);
        void SetSubject(string subject);
        void SetReplyTo(string replyTo);
        void SetContent(string content);
        bool SendEmail();
        void IsBodyHtml(bool isBodyHtml);
    }
}