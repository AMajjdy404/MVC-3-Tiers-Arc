using MVC.DAL.Models;

namespace MVC.PL.Helpers.SendEmail
{
    public interface IMailService
    {
        void SendEmail(Email email);
    }
}
