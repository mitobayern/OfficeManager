using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.Tenants;
using OfficeManager.Data;
using OfficeManager.Models;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public class TenantsService : ITenantsService
    {
        private readonly ApplicationDbContext dbContext;

        public TenantsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateTenant(CreateTenantViewModel input)
        {
            if (this.dbContext.Tenants.Any(x=>x.CompanyName == input.CompanyName))
            {
                return;
            }

            Tenant tenant = new Tenant()
            {
                CompanyName = input.CompanyName,
                CompanyOwner = input.CompanyOwner,
                Bulstat = input.Bulstat,
                Address = input.Address,
                Email = input.Email,
                Phone = input.Phone,
                StartOfContract = input.StartOfContract
            };
            this.dbContext.Tenants.Add(tenant);
            this.dbContext.SaveChanges();
        }

        public TenantToEditViewModel EditTenant(Tenant currentTenant)
        {
            List<string> tenantOffices = currentTenant.Offices.OrderBy(x=>x.Name).Select(x => x.Name).ToList();
            List<string> allOffices = this.dbContext.Offices.Select(x => x.Name).ToList();

            TenantToEditViewModel tenantToEdit = new TenantToEditViewModel
            {
                Id = currentTenant.Id,
                CompanyName = currentTenant.CompanyName,
                CompanyOwner = currentTenant.CompanyOwner,
                Address = currentTenant.Address,
                Bulstat = currentTenant.Bulstat,
                Email = currentTenant.Email,
                Phone = currentTenant.Phone,
                StartOfContract = currentTenant.StartOfContract,
                Offices = tenantOffices,
                AllOffices = allOffices
            };

            return tenantToEdit;
        }

        public IQueryable<TenantOutputViewModel> GetAllTenants()
        {
            var allTenants = this.dbContext.Tenants.Select(x => new TenantOutputViewModel
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                CompanyOwner = x.CompanyOwner,
                Bulstat = x.Bulstat,
                Address = x.Address
            });
            return allTenants;
        }

        public Tenant GetTenantById(int id)
        {
            var tenant = this.dbContext.Tenants.Include(y => y.Offices).FirstOrDefault(x => x.Id == id);
            return tenant;
        }

        public Tenant GetTenantByCompanyName(string name)
        {
            var tenant = this.dbContext.Tenants.Include(y => y.Offices).FirstOrDefault(x => x.CompanyName == name);
            return tenant;
        }

        public string GetTenantEIK(string tenantCompanyName)
        {
            var tenant = GetTenantByCompanyName(tenantCompanyName);
            string eik;
            if (tenant.Bulstat.StartsWith("BG"))
            {
                eik = tenant.Bulstat.Substring(2);
            }
            else
            {
                eik = tenant.Bulstat;
            }

            return eik;
        }

        public IEnumerable<EditOfficeViewModel> GetTenantOffices(TenantIdViewModel input)
        {
            var currentTenant = GetTenantById(input.Id);

            var currentTenantOffices = currentTenant.Offices.Select(x => new EditOfficeViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Area = x.Area,
                RentPerSqMeter = x.RentPerSqMeter
            });

            return currentTenantOffices;
        }

        public string GetTenantOfficesAsText(string tenantCompanyName)
        {
            Tenant tenant = GetTenantByCompanyName(tenantCompanyName);

            string tenantOfficesAsText;

            if (tenant.Offices.Count > 1)
            {
                tenantOfficesAsText = "офиси ";
                List<string> currentTenantOffices = tenant.Offices.OrderBy(x => x.Name).Select(x => x.Name).ToList();

                for (int i = 0; i < currentTenantOffices.Count - 1; i++)
                {
                    if (currentTenantOffices[i] != currentTenantOffices[currentTenantOffices.Count - 2])
                    {
                        tenantOfficesAsText += currentTenantOffices[i] + ", ";
                    }
                    else
                    {
                        tenantOfficesAsText += currentTenantOffices[i] + " и ";
                    }
                }
                tenantOfficesAsText += currentTenantOffices[currentTenantOffices.Count - 1];
            }
            else
            {
                tenantOfficesAsText = "офис " + string.Join(", ", tenant.Offices.OrderBy(x => x.Name).Select(x => x.Name));
            }

            return tenantOfficesAsText;
        }

        public void UpdateTenant(TenantToEditViewModel input)
        {
            Tenant tenantToEdit = GetTenantById(input.Id);

            tenantToEdit.CompanyOwner = input.CompanyOwner;
            tenantToEdit.CompanyName = input.CompanyName;
            tenantToEdit.Bulstat = input.Bulstat;
            tenantToEdit.Phone = input.Phone;
            tenantToEdit.Email = input.Email;
            tenantToEdit.Address = input.Address;
            tenantToEdit.StartOfContract = input.StartOfContract;

            this.dbContext.SaveChanges();
        }
    }
}
