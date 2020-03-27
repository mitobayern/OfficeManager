namespace OfficeManager.Services
{
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using System.Linq;

    public class TemperatureMetersService : ITemperatureMetersService
    {
        private readonly ApplicationDbContext dbContext;

        public TemperatureMetersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateTemperatureMeter(CreateTemperatureMeterViewModel input)
        {
            var temperatureMeter = new TemperatureMeter
            {
                Name = input.Name,
            };

            this.dbContext.TemperatureMeters.Add(temperatureMeter);
            this.dbContext.SaveChanges();
        }

        public EditTemperatreMeterViewModel EditTemperatureMeter(int id)
        {
            var temperatureMeter = GetTemperatureMeterById(id);
            
            var temperatureMeterToEdit = new EditTemperatreMeterViewModel
            { 
                Id = temperatureMeter.Id,
                Name = temperatureMeter.Name,
            };

            return temperatureMeterToEdit;
        }

        public IQueryable<TemperatureMeterOutputViewModel> GetAllTemperatureMeters()
        {
            var allTemperatureMeters = this.dbContext.TemperatureMeters.Include(x => x.Office).Select(x => new TemperatureMeterOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                OfficeNumber = x.Office.Name
            });

            return allTemperatureMeters;
        }

        public TemperatureMeter GetTemperatureMeterById(int id)
        {
            var temperatureMeter = this.dbContext.TemperatureMeters.FirstOrDefault(x => x.Id == id);

            return temperatureMeter;
        }

        public TemperatureMeter GetTemperatureMeterByName(string name)
        {
            var temperatureMeter = this.dbContext.TemperatureMeters.Include(x=>x.Office).FirstOrDefault(x => x.Name == name);

            return temperatureMeter;
        }

        public void UpdateTemperatureMeter(EditTemperatreMeterViewModel input)
        {
            TemperatureMeter temperatureMeterToEdit = GetTemperatureMeterById(input.Id);

            temperatureMeterToEdit.Name = input.Name;

            this.dbContext.SaveChanges();
        }
    }
}
