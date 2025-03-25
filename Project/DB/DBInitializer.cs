using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.DB
{
    public class DBInitializer
    {
        public static async Task InitializeDatabase(IServiceProvider serviceProvider, ProjectDbContext context)
        {

            #region Migrations
            using IServiceScope serviceScope = serviceProvider.CreateScope();

            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                DBInitializer initializer = new DBInitializer();
                await initializer.Seed(context, serviceScope);
            }
            catch (Exception ex)
            {
                ILogger<ProjectDbContext> logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<ProjectDbContext>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
            #endregion
        }

        private async Task Seed(ProjectDbContext context, IServiceScope serviceScope)
        {
            context.Database.EnsureCreated();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (await roleManager.Roles.AnyAsync())
                return;

            var role = new IdentityRole();

            role = new IdentityRole { Name = "Admin" };

            await roleManager.CreateAsync(role);

            role = new IdentityRole { Name = "User" };

            await roleManager.CreateAsync(role);

            var user = new User("admin@gmail.com", "admin", "Admin", "istrator");

            await userManager.CreateAsync(user, "Test1234!");

            await userManager.AddToRoleAsync(user, "Admin");

            await context.SaveChangesAsync();
        }
    }
}
