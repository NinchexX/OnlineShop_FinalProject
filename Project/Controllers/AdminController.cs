using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Entities;
using Project.Interfaces;

namespace Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(string username, string email, string password, string firstName, string lastName, string role)
        {
            await _adminService.CreateUser(username, email, password, firstName, lastName role);
            return Ok();
        }

        [HttpPost("ChangeRole")]
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            await _adminService.AssignRoleToUser(userId, role);
            return Ok();
        }

        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser(string userId, string firstName, string lastName)
        {
            await _adminService.EditUser(userId, firstName, lastName);
            return Ok();
        }


        [HttpPost("GetRatings")]
        public async Task<IActionResult> GetRatings(int id)
        {
            return Ok(await _adminService.GetRatings(id));
        }
    }
}
