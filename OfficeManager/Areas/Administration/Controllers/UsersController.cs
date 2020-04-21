namespace OfficeManager.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OfficeManager.Areas.Administration.ViewModels.Users;
    using OfficeManager.Data;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
            this.userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            this.roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        }

        public async Task<IActionResult> All()
        {
            var allUsers = this.dbContext.Users.Select(x => x.UserName).ToList();

            string roleName = string.Empty;
            List<UserViewModel> userList = new List<UserViewModel>();

            foreach (var identityUser in allUsers)
            {
                var user = await this.userManager.FindByNameAsync(identityUser);
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
                    UserName = identityUser,
                    Role = roleName,
                    Email = user.Email,
                };
                userList.Add(userViewModel);
            }

            return this.View(userList);
        }

        public async Task<IActionResult> Promote(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            await this.userManager.AddToRoleAsync(user, "Admin");

            return this.Redirect("/Administration/Users/All");
        }

        public async Task<IActionResult> Demote(string userName)
        {

            var user = await this.userManager.FindByNameAsync(userName);
            await this.userManager.RemoveFromRoleAsync(user, "Admin");

            return this.Redirect("/Administration/Users/All");
        }

        public async Task<IActionResult> Delete(string userName)
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

            return this.Redirect("/Administration/Users/All");
        }
    }
}
