using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DB;
using Project.DTO.Admin;
using Project.Entities;
using Project.Interfaces;

namespace Project.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProjectDbContext _projectDbContext;

        public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ProjectDbContext projectDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _projectDbContext = projectDbContext;
        }

        public async Task CreateUser(string username, string email, string password, string firstName, string lastName, string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                throw new Exception($"Role '{role}' does not exist.");
            }

            var user = new User { UserName = username, Email = email, FirstName = firstName, LastName = lastName };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors));
            }

            result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to assign role: " + string.Join(", ", result.Errors));
            }
        }

        public async Task AssignRoleToUser(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == default)
                throw new Exception("User Not Found!");

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                throw new Exception($"Role '{role}' does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to assign role: " + string.Join(", ", result.Errors));
            }
        }

        public async Task EditUser(string id, string firstName, string lastName)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == default)
                throw new Exception("User Not Found!");

            user.FirstName = firstName;
            user.LastName = lastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user: " + string.Join(", ", result.Errors));
            }
        }

        public async Task<GetRatingsDTO> GetRatings(int id)
        {
            var product = await _projectDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if(product == default)
                throw new Exception("Product Not Found!");

            var result = new GetRatingsDTO(product.Id, product.Name);

            await _projectDbContext.Ratings.Where(x => x.ProductId == id).Include(x => x.User).ForEachAsync(x => result.AddRating(new RatingsDTO(x.Id, x.User.Email, x.RatingValue)));

            return result;
        }
    }
}
