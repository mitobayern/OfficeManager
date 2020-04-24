namespace OfficeManager.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUsersService usersService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(ApplicationDbContext dbContext, IServiceProvider serviceProvider, IUsersService usersService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
            this.userManager = serviceProvider.GetService<UserManager<User>>();
            this.roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        }

        public async Task<IActionResult> AllAsync()
        {
            var allUsers = await this.usersService.GetAllUsersAsync();

            return this.View(allUsers);
        }

        public async Task<IActionResult> Promote(string userName)
        {
            await this.usersService.PromoteUserToAdminAsync(userName);

            return this.Redirect("/Administration/Users/All");
        }

        public async Task<IActionResult> Demote(string userName)
        {
            await this.usersService.DemoteAdminToUserAsync(userName);

            return this.Redirect("/Administration/Users/All");
        }

        public async Task<IActionResult> Enable(string userName)
        {
            await this.usersService.EnableUserAsync(userName);
            return this.Redirect("/Administration/Users/All");
        }

        public async Task<IActionResult> Delete(string userName)
        {
            await this.usersService.DeleteUserAsync(userName);

            return this.Redirect("/Administration/Users/All");
        }
    }
}
