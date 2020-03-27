using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.Tenants;
using OfficeManager.Models;
using OfficeManager.ViewModels.AccountingReports;
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

        Tenant GetTenantByName(string name);

        IQueryable<TenantOutputViewModel> GetAllTenants();

        TenantToEditViewModel EditTenant(Tenant tenant);

        void UpdateTenant(TenantToEditViewModel input);

        IEnumerable<EditOfficeViewModel> GetTenantOffices(TenantIdViewModel input);

    }
}
