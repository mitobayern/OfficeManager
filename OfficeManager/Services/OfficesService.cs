namespace OfficeManager.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class OfficesService : IOfficesService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITenantsService tenantsService;
        private readonly IElectricityMetersService electricityMetersService;
        private readonly ITemperatureMetersService temperatureMetersService;

        public OfficesService(
            ApplicationDbContext dbContext,
            ITenantsService tenantsService,
            IElectricityMetersService electricityMetersService,
            ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.tenantsService = tenantsService;
            this.electricityMetersService = electricityMetersService;
            this.temperatureMetersService = temperatureMetersService;
        }

        public async Task CreateOfficeAsync(string name, decimal area, decimal rentPerSqMeter)
        {
            if (this.dbContext.Offices.Any(x => x.Name == name))
            {
                return;
            }

            Office office = new Office()
            {
                Name = name,
                Area = area,
                IsAvailable = true,
                RentPerSqMeter = rentPerSqMeter,
                IsDeleted = false,
            };

            await this.dbContext.Offices.AddAsync(office);
            await this.dbContext.SaveChangesAsync();
        }

        public EditOfficeViewModel EditOffice(int id)
        {
            var office = this.GetOfficeById(id);
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
                TemperatureMeters = officeTemperatureMeters,
            };

            return officeToEdit;
        }

        public async Task UpdateOfficeAsync(int id, string name, decimal area, decimal rentPerSqMeter)
        {
            var officeToEdit = this.dbContext.Offices.FirstOrDefault(x => x.Id == id);
            officeToEdit.Name = name;
            officeToEdit.Area = area;
            officeToEdit.RentPerSqMeter = rentPerSqMeter;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task AddOfficesToTenantAsync(int id, List<string> offices)
        {
            var currentTenant = this.tenantsService.GetTenantById(id);

            foreach (var officeName in offices)
            {
                Office office = this.GetOfficeByName(officeName);
                if (office == null)
                {
                    return;
                }

                currentTenant.Offices.Add(office);
                office.IsAvailable = false;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveOfficesFromTenantAsync(int id, List<string> offices)
        {
            var currentTenant = this.tenantsService.GetTenantById(id);

            foreach (var officeName in offices)
            {
                Office office = this.GetOfficeByName(officeName);
                if (office == null)
                {
                    return;
                }

                currentTenant.Offices.Remove(office);
                office.IsAvailable = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task AddTemperatureMetersToOfficeAsync(int id, List<string> temperatureMeters)
        {
            var currentOffice = this.GetOfficeById(id);

            foreach (var temperatureMeterName in temperatureMeters)
            {
                TemperatureMeter temperatureMeter = this.temperatureMetersService.GetTemperatureMeterByName(temperatureMeterName);
                if (temperatureMeter == null)
                {
                    return;
                }

                temperatureMeter.Office = currentOffice;
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveTemperatureMetersFromOfficeAsync(int id, List<string> temperatureMeters)
        {
            var currentOffice = this.GetOfficeById(id);

            foreach (var termperatreMeterName in temperatureMeters)
            {
                TemperatureMeter temperatureMeter = this.temperatureMetersService.GetTemperatureMeterByName(termperatreMeterName);
                if (temperatureMeter == null)
                {
                    return;
                }

                currentOffice.TemperatureMeters.Remove(temperatureMeter);
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task AddElectricityMeterToOfficeAsync(int id, string electricityMeterName)
        {
            var currentOffice = this.GetOfficeById(id);
            var electricityMeter = this.electricityMetersService.GetElectricityMeterByName(electricityMeterName);
            currentOffice.ElectricityMeter = electricityMeter;
            electricityMeter.Office = currentOffice;
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveElectricityMeterFromOfficeAsync(int id)
        {
            var currentOffice = this.GetOfficeById(id);
            var currentElMeter = currentOffice.ElectricityMeter;

            currentElMeter.Office = null;
            currentElMeter.OfficeId = null;
            currentOffice.ElectricityMeter = null;

            await this.dbContext.SaveChangesAsync();
        }

        public Office GetOfficeById(int id)
        {
            var office = this.dbContext.Offices.Include(y => y.ElectricityMeter).FirstOrDefault(x => x.Id == id);
            return office;
        }

        public Office GetOfficeByName(string name)
        {
            Office office = this.dbContext.Offices.Include(y => y.ElectricityMeter).FirstOrDefault(x => x.Name == name);
            return office;
        }

        public IQueryable<EditOfficeViewModel> GetAllAvailableOffices()
        {
            var availavleOffices = this.dbContext.Offices.Where(x => x.IsAvailable == true && x.IsDeleted == false).Select(x => new EditOfficeViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Area = x.Area,
                RentPerSqMeter = x.RentPerSqMeter,
            });

            return availavleOffices;
        }

        public IQueryable<OfficeOutputViewModel> GetAllOffices()
        {
            var offices = this.dbContext.Offices.Where(x => x.IsDeleted == false).Select(x => new OfficeOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                TenantName = (x.Tenant == null) ? "Свободен" : x.Tenant.CompanyName,
                Area = x.Area,
                RentPerSqMeter = x.RentPerSqMeter,
            });
            return offices;
        }

        public IEnumerable<TemperatureMeterOutputViewModel> GetOfficeTemperatureMeters(int id)
        {
            var currentOffice = this.GetOfficeById(id);

            var officeTemperatureMeters = currentOffice.TemperatureMeters.Select(x => new TemperatureMeterOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                OfficeNumber = x.Office.Name,
            });

            return officeTemperatureMeters;
        }

        public async Task DeleteOfficeAsync(int id)
        {
            var office = this.GetOfficeById(id);
            if (office.Tenant != null)
            {
                var tenant = office.Tenant;
                var officesToRemove = new List<string> { office.Name };
                await this.RemoveOfficesFromTenantAsync(tenant.Id, officesToRemove);
            }

            if (office.ElectricityMeter != null)
            {
                await this.RemoveElectricityMeterFromOfficeAsync(id);
            }

            office.TemperatureMeters.Clear();
            office.IsDeleted = true;
            office.IsAvailable = true;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
