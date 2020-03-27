namespace OfficeManager.SeedData
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationDbContextSeeder
    {
        private const string initialRoleName = "Admin";
        private const string initialUserName = "Admin";
        private const string initialUserPassword = "Administrati0n";

        private const string initialUserEmail = "Admin@gmail.com";



        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public ApplicationDbContextSeeder(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
            this.userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            this.roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

        }

        public async Task SeedDataAsync()
        {
            await SeedUsersAsync();
            await SeedRolesAsync();
            await SeedUserToRolesAsync();

        }
        private async Task SeedUsersAsync()
        {
            var user = await userManager.FindByNameAsync(initialUserName);

            if (user != null)
            {
                return;
            }

            await userManager.CreateAsync(new IdentityUser
            {
                UserName = initialUserName,
                Email = initialUserEmail,
                EmailConfirmed = true,
            }, initialUserPassword);
        }

        private async Task SeedRolesAsync()
        {
            var role = await roleManager.FindByNameAsync(initialRoleName);

            if (role != null)
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole
            {
                Name = initialRoleName
            });
        }

        private async Task SeedUserToRolesAsync()
        {
            var user = await userManager.FindByNameAsync(initialUserName);
            var role = await roleManager.FindByNameAsync(initialRoleName);

            var exists = this.dbContext.UserRoles.Any(x => x.RoleId == role.Id && x.UserId == user.Id);

            if (exists)
            {
                return;
            }

            dbContext.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id
            });

            await dbContext.SaveChangesAsync();
        }



    }
}
