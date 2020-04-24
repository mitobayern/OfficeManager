namespace OfficeManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OfficeManager.Areas.Administration.ViewModels.Users;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersService(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
            this.userManager = serviceProvider.GetService<UserManager<User>>();
            this.roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        }

        public async Task PromoteUserToAdminAsync(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            user.IsEnabled = true;
            await this.userManager.AddToRoleAsync(user, "Admin");
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DemoteAdminToUserAsync(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            await this.userManager.RemoveFromRoleAsync(user, "Admin");
        }

        public async Task DeleteUserAsync(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            var role = await this.roleManager.FindByNameAsync("Admin");

            var userRole = await this.dbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id && x.RoleId == role.Id);
            if (userRole != null)
            {
                this.dbContext.Remove(userRole);
            }

            this.dbContext.Users.Remove(user);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var allUsers = this.dbContext.Users.Select(x => x.UserName).ToList();

            string roleName = string.Empty;
            List<UserViewModel> userList = new List<UserViewModel>();

            foreach (var currentUser in allUsers)
            {
                var user = await this.userManager.FindByNameAsync(currentUser);
                var role = this.dbContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id);

                if (user != null && role != null)
                {
                    var userRole = await this.roleManager.FindByIdAsync(role.RoleId);
                    roleName = userRole.Name;
                }
                else
                {
                    roleName = "User";
                }

                var userViewModel = new UserViewModel
                {
                    UserName = currentUser,
                    Role = roleName,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled != null && user.IsEnabled == true ? true : false,
                };
                userList.Add(userViewModel);
            }

            return userList;
        }

        public async Task EnableUserAsync(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            user.IsEnabled = true;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
