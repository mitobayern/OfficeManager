namespace OfficeManager.SeedData
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class ApplicationDbContextSeeder
    {
        private const string InitialRoleName = "Admin";
        private const string InitialUserName = "Admin";
        private const string InitialUserPassword = "Administrati0n";
        private const string InitialUserEmail = "Admin@gmail.com";
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ApplicationDbContextSeeder(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
            this.userManager = serviceProvider.GetService<UserManager<User>>();
            this.roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        }

        public async Task SeedDataAsync()
        {
            await this.SeedUsersAsync();
            await this.SeedRolesAsync();
            await this.SeedUserToRolesAsync();
        }

        private async Task SeedUsersAsync()
        {
            var user = await this.userManager.FindByNameAsync(InitialUserName);

            if (user != null)
            {
                return;
            }

            await this.userManager.CreateAsync(
                new User
                {
                    UserName = InitialUserName,
                    Email = InitialUserEmail,
                    EmailConfirmed = true,
                },
                InitialUserPassword);
        }

        private async Task SeedRolesAsync()
        {
            var role = await this.roleManager.FindByNameAsync(InitialRoleName);

            if (role != null)
            {
                return;
            }

            await this.roleManager.CreateAsync(new IdentityRole
            {
                Name = InitialRoleName,
            });
        }

        private async Task SeedUserToRolesAsync()
        {
            var user = await this.userManager.FindByNameAsync(InitialUserName);
            var role = await this.roleManager.FindByNameAsync(InitialRoleName);

            var exists = this.dbContext.UserRoles.Any(x => x.RoleId == role.Id && x.UserId == user.Id);

            if (exists)
            {
                return;
            }

            this.dbContext.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            await this.dbContext.SaveChangesAsync();
        }
    }
}
