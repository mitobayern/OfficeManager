namespace OfficeManager.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class ElectricityMetersService : IElectricityMetersService
    {
        private readonly ApplicationDbContext dbContext;

        public ElectricityMetersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateElectricityMeterAsync(string name, decimal powerSupply)
        {
            ElectricityMeter electricityMeter = new ElectricityMeter
            {
                Name = name,
                PowerSupply = powerSupply,
            };

            if (this.dbContext.ElectricityMeters.Any(x => x.Name == electricityMeter.Name))
            {
                return;
            }

            await this.dbContext.ElectricityMeters.AddAsync(electricityMeter);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateElectricityMeterAsync(int id, string name, decimal powerSupply)
        {
            ElectricityMeter electricityMeterToEdit = this.GetElectricityMeterById(id);

            electricityMeterToEdit.Name = name;
            electricityMeterToEdit.PowerSupply = powerSupply;

            await this.dbContext.SaveChangesAsync();
        }

        public ElectricityMeter GetElectricityMeterById(int id)
        {
            var electricityMeterToEdit = this.dbContext.ElectricityMeters.FirstOrDefault(x => x.Id == id);

            return electricityMeterToEdit;
        }

        public ElectricityMeter GetElectricityMeterByName(string name)
        {
            var electricityMeter = this.dbContext.ElectricityMeters.FirstOrDefault(x => x.Name == name);

            return electricityMeter;
        }

        public IQueryable<ElectricityMeterOutputViewModel> GetAllElectricityMeters()
        {
            var electricityMeteres = this.dbContext.ElectricityMeters.Select(x => new ElectricityMeterOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PowerSupply = x.PowerSupply,
                OfficeNumber = x.Office.Name,
            });

            return electricityMeteres;
        }
    }
}
