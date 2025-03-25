using Project.DTO.Account;

namespace Project.Interfaces
{
    public interface IAccountService
    {
        Task SignUp(SignUpDTO request);
        Task<string> SignIn(SignInDTO request);
        Task VerifyEmail(string email, string token);
    }
}
