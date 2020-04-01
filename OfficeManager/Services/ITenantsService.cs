using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.Tenants;
using OfficeManager.Models;
using OfficeManager.ViewModels.AccountingReports;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public interface ITenantsService
    {
        void CreateTenant(CreateTenantViewModel tenant);

        Tenant GetTenantById(int id);

        Tenant GetTenantByCompanyName(string tenantCompanyName);

        IQueryable<TenantOutputViewModel> GetAllTenants();

        TenantToEditViewModel EditTenant(Tenant tenant);

        void UpdateTenant(TenantToEditViewModel input);

        IEnumerable<EditOfficeViewModel> GetTenantOffices(TenantIdViewModel input);

        string GetTenantOfficesAsText(string tenantCompanyName);

        string GetTenantEIK(string tenantCompanyName);
    }
}
