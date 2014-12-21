namespace Mobimarket.CrosscuttingFunctionality.Senders
{
    public interface IMailSender
    {
        bool Send(string topic, string text, string userMail);
    }
}
