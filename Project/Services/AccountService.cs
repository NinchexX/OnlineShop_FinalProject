using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.DTO.Account;
using Project.Entities;
using Project.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Project.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailSender = emailSender;
        }

        public async Task SignUp(SignUpDTO request)
        {
            if (request.Password != request.ConfirmPassword)
                throw new Exception("Password And Confirm Password Should Match!");

            bool existingUser = await _userManager.Users
                .Where(x => x.Email == request.Email
                            || x.UserName == request.Username)
                .AnyAsync();

            if (existingUser) 
                throw new Exception("User Already Exists!");

            User user = new User(request.Email, request.Username, request.FirstName, request.LastName);

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded) 
                throw new Exception(JsonSerializer.Serialize(createUserResult.Errors));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!addToRoleResult.Succeeded)
                throw new Exception(JsonSerializer.Serialize(addToRoleResult.Errors));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _emailSender.SendEmailAsync(request.Email, token);

            return;
        }

        public async Task<string> SignIn(SignInDTO request)
        {
            var user = await _userManager.Users
                .Where(x => x.NormalizedEmail == request.UserName.ToUpper() || x.NormalizedUserName == request.UserName.ToUpper())
                .FirstOrDefaultAsync();

            if (user == default)
                throw new Exception("User Not Found!");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                request.Password,
                true,
                false);

            if (!result.Succeeded)
                throw new Exception("Username Or Password Is Incorrect");

            var userRoles = await _userManager.GetRolesAsync(user);

            IdentityOptions _options = new IdentityOptions();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName.ToString()),
                        new Claim(_options.ClaimsIdentity.EmailClaimType, user.Email.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, string.Join(",", userRoles)),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["SecretKey"])),
                            SecurityAlgorithms.HmacSha256Signature
                    )
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public async Task VerifyEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == default)
                throw new ArgumentNullException(nameof(user));

            var decodedBytes = Convert.FromBase64String(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                throw new Exception("Token Is Incorrect!");
        }
    }
}
