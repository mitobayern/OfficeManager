namespace OfficeManager.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.Tenants;
    using OfficeManager.Models;

    public interface ITenantsService
    {
        Task CreateTenantAsync(CreateTenantViewModel tenant);

        Task UpdateTenantAsync(TenantToEditViewModel input);

        TenantToEditViewModel EditTenant(Tenant tenant);

        Tenant GetTenantById(int id);

        Tenant GetTenantByCompanyName(string tenantCompanyName);

        string GetTenantEIK(string tenantCompanyName);

        string GetTenantOfficesAsText(string tenantCompanyName);

        IEnumerable<EditOfficeViewModel> GetTenantOffices(TenantIdViewModel input);

        IQueryable<TenantOutputViewModel> GetAllTenants();
    }
}
