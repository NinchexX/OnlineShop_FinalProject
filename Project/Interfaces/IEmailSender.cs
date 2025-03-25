namespace Project.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string token);
    }
}
