namespace OfficeManager.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using OfficeManager.Data;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Users;
    using System.Collections.Generic;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IServiceProvider serviceProvider;
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
                };
                userList.Add(userViewModel);
            }

            return this.View(userList);
        }

        public async Task<IActionResult> Promote(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);
            var role = await this.roleManager.FindByNameAsync("Admin");


            this.dbContext.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            await this.dbContext.SaveChangesAsync();

            return this.Redirect("/Administration/Users/All");
        }
    }
}
