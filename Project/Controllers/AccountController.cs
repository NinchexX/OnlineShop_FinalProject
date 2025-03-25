using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Project.DTO.Account;
using Project.Interfaces;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(nameof(SignUp))]
        public async Task<IActionResult> SignUp(SignUpDTO request)
        {
            await _accountService.SignUp(request);
            return Ok();
        }

        [HttpPost(nameof(SignIn))]
        public async Task<IActionResult> SignIn(SignInDTO request)
        {
            var data = await _accountService.SignIn(request);
            return Ok(data);
        }

        [HttpGet("Verify/{email}/{token}")]
        public async Task<IActionResult> VerifyEmail([FromRoute] string email, [FromRoute] string token)
        {
            await _accountService.VerifyEmail(email, token);
            return Ok();
        }
    }
}
