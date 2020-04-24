namespace OfficeManager.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Users;

    public interface IUsersService
    {
        Task PromoteUserToAdminAsync(string userName);

        Task DemoteAdminToUserAsync(string userName);

        Task DeleteUserAsync(string userName);

        Task EnableUserAsync(string userName);

        Task<List<UserViewModel>> GetAllUsersAsync();
    }
}
