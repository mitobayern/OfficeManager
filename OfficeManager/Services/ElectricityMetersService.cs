using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
using OfficeManager.Data;
using OfficeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public class ElectricityMetersService : IElectricityMetersService
    {
        private readonly ApplicationDbContext dbContext;

        public ElectricityMetersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateElectricityMeter(CreateElectricityMeterViewModel input)
        {
            ElectricityMeter electricityMeter = new ElectricityMeter
            {
                Name = input.Name,
                PowerSupply = input.PowerSupply,
            };

            if (this.dbContext.ElectricityMeters.Any(x=>x.Name == electricityMeter.Name))
            {
                return;
            }

            this.dbContext.ElectricityMeters.Add(electricityMeter);
            this.dbContext.SaveChanges();
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

        public void UpdateElectricityMeter(ElectricityMeterOutputViewModel input)
        {
            ElectricityMeter electricityMeterToEdit = GetElectricityMeterById(input.Id);

            electricityMeterToEdit.Name = input.Name;
            electricityMeterToEdit.PowerSupply = input.PowerSupply;

            this.dbContext.SaveChanges();
        }
    }
}
