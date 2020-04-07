using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
using OfficeManager.Data;
using OfficeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public class OfficesService : IOfficesService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITenantsService tenantsService;
        private readonly IElectricityMetersService electricityMetersService;
        private readonly ITemperatureMetersService temperatureMetersService;

        public OfficesService(ApplicationDbContext dbContext,
                              ITenantsService tenantsService,
                              IElectricityMetersService electricityMetersService,
                              ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.tenantsService = tenantsService;
            this.electricityMetersService = electricityMetersService;
            this.temperatureMetersService = temperatureMetersService;
        }

        public void AddElectricityMeterToOffice(int id, string electricityMeterName)
        {
            var currentOffice = GetOfficeById(id);
            var electricityMeter = this.electricityMetersService.GetElectricityMeterByName(electricityMeterName);
            currentOffice.ElectricityMeter = electricityMeter;
            electricityMeter.Office = currentOffice;
            this.dbContext.SaveChanges();
        }

        public void AddOfficesToTenant(int id, List<string> offices)
        {
            var currentTenant = this.tenantsService.GetTenantById(id);

            foreach (var officeName in offices)
            {
                Office office = GetOfficeByName(officeName);
                if (office == null)
                {
                    return;
                }

                currentTenant.Offices.Add(office);
                office.isAvailable = false;
            }

            this.dbContext.SaveChanges();
        }

        public void AddTemperatureMetersToOffice(int id, List<string> temperatureMeters)
        {
            var currentOffice = GetOfficeById(id);

            foreach (var temperatureMeterName in temperatureMeters)
            {
                TemperatureMeter temperatureMeter = this.temperatureMetersService.GetTemperatureMeterByName(temperatureMeterName);
                if (temperatureMeter == null)
                {
                    return;
                }

                temperatureMeter.Office = currentOffice;
                this.dbContext.SaveChanges();
            }
        }

        public void CreateOffice(CreateOfficeViewModel input)
        {
            if (this.dbContext.Offices.Any(x=>x.Name == input.Name))
            {
                return;
            }

            Office office = new Office()
            {
                Name = input.Name,
                Area = input.Area,
                isAvailable = true,
                RentPerSqMeter = input.RentPerSqMeter,
            };

            this.dbContext.Offices.Add(office);
            this.dbContext.SaveChanges();
        }

        public EditOfficeViewModel EditOffice(int id)
        {
            var office = GetOfficeById(id);
            var allElectricityMeters = this.electricityMetersService.GetAllElectricityMeters().Select(x => x.Name).ToList();
            var allTemperatureMeters = this.temperatureMetersService.GetAllTemperatureMeters().Select(x => x.Name).ToList();
            List<string> officeTemperatureMeters = office.TemperatureMeters.OrderBy(x => x.Name).Select(x => x.Name).ToList();

            string officeElectricityMeter = "No electricity meter available";
            if (office.ElectricityMeter != null)
            {
                officeElectricityMeter = office.ElectricityMeter.Name;
            }

            EditOfficeViewModel officeToEdit = new EditOfficeViewModel
            {
                Id = office.Id,
                Name = office.Name,
                Area = office.Area,
                RentPerSqMeter = office.RentPerSqMeter,
                ElectricityMeter = officeElectricityMeter,
                //AllElecticityMeters = allElectricityMeters,
                TemperatureMeters = officeTemperatureMeters,
                //AllTemperatureMeters = allTemperatureMeters,
            };

            return officeToEdit;
        }

        public IQueryable<EditOfficeViewModel> GetAllAvailableOffices()
        {
            var availavleOffices = this.dbContext.Offices.Where(x => x.isAvailable == true).Include(y => y.TemperatureMeters).Select(x => new EditOfficeViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Area = x.Area,
                RentPerSqMeter = x.RentPerSqMeter
            });

            return availavleOffices;
        }

        public IQueryable<OfficeOutputViewModel> GetAllOffices()
        {
            var offices = this.dbContext.Offices.Select(x => new OfficeOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                TenantName = (x.Tenant == null) ? "Свободен" : x.Tenant.CompanyName,
                Area = x.Area,
                RentPerSqMeter = x.RentPerSqMeter,
            });
            return offices;
        }

        public Office GetOfficeById(int id)
        {
            var office = this.dbContext.Offices.Include(y => y.ElectricityMeter).Include(a => a.TemperatureMeters).FirstOrDefault(x => x.Id == id);
            return office;
        }

        public Office GetOfficeByName(string name)
        {
            Office office = this.dbContext.Offices.Include(y => y.ElectricityMeter).Include(a => a.TemperatureMeters).FirstOrDefault(x => x.Name == name);
            return office;
        }

        public IEnumerable<TemperatureMeterOutputViewModel> GetOfficeTemperatureMeters(int id)
        {
            var currentOffice = GetOfficeById(id);

            var officeTemperatureMeters = currentOffice.TemperatureMeters.Select(x => new TemperatureMeterOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                OfficeNumber = x.Office.Name,
            });

            return officeTemperatureMeters;
        }

        public void RemoveOfficesFromTenant(int id, List<string> offices)
        {
            var currentTenant = this.tenantsService.GetTenantById(id);

            foreach (var officeName in offices)
            {
                Office office = GetOfficeByName(officeName);
                if (office == null)
                {
                    return;
                }

                currentTenant.Offices.Remove(office);
                office.isAvailable = true;
            }

            this.dbContext.SaveChanges();
        }

        public void RemoveTemperatureMetersFromOffice(int id, List<string> temperatureMeters)
        {
            var currentOffice = GetOfficeById(id);

            foreach (var termperatreMeterName in temperatureMeters)
            {
                TemperatureMeter temperatureMeter = this.temperatureMetersService.GetTemperatureMeterByName(termperatreMeterName);
                if (temperatureMeter == null)
                {
                    return;
                }

                currentOffice.TemperatureMeters.Remove(temperatureMeter);
            }

            this.dbContext.SaveChanges();
        }

        public void RemoveElectricityMeterFromOffice(int id)
        {
            var currentOffice = GetOfficeById(id);
            var currentElMeter = currentOffice.ElectricityMeter;

            currentElMeter.Office = null;
            currentElMeter.OfficeId = null;
            currentOffice.ElectricityMeter = null;

            dbContext.SaveChanges();
        }

        public void UpdateOffice(int id, string name, decimal area, decimal rentPerSqMeter)
        {
            var officeToEdit = this.dbContext.Offices.FirstOrDefault(x => x.Id == id);
            officeToEdit.Name = name;
            officeToEdit.Area = area;
            officeToEdit.RentPerSqMeter = rentPerSqMeter;

            this.dbContext.SaveChanges();
        }
    }
}
