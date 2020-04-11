﻿namespace OfficeManager.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class TemperatureMetersService : ITemperatureMetersService
    {
        private readonly ApplicationDbContext dbContext;

        public TemperatureMetersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateTemperatureMeterAsync(string name)
        {
            var temperatureMeter = new TemperatureMeter
            {
                Name = name,
            };

            if (this.dbContext.TemperatureMeters.Any(x => x.Name == temperatureMeter.Name))
            {
                return;
            }

            await this.dbContext.TemperatureMeters.AddAsync(temperatureMeter);
            await this.dbContext.SaveChangesAsync();
        }

        public EditTemperatreMeterViewModel EditTemperatureMeter(int id)
        {
            var temperatureMeter = this.GetTemperatureMeterById(id);

            var temperatureMeterToEdit = new EditTemperatreMeterViewModel
            {
                Id = temperatureMeter.Id,
                Name = temperatureMeter.Name,
            };

            return temperatureMeterToEdit;
        }

        public async Task UpdateTemperatureMeterAsync(int id, string name)
        {
            TemperatureMeter temperatureMeterToEdit = this.GetTemperatureMeterById(id);

            temperatureMeterToEdit.Name = name;

            await this.dbContext.SaveChangesAsync();
        }

        public TemperatureMeter GetTemperatureMeterById(int id)
        {
            var temperatureMeter = this.dbContext.TemperatureMeters.FirstOrDefault(x => x.Id == id);

            return temperatureMeter;
        }

        public TemperatureMeter GetTemperatureMeterByName(string name)
        {
            var temperatureMeter = this.dbContext.TemperatureMeters.FirstOrDefault(x => x.Name == name);

            return temperatureMeter;
        }

        public IQueryable<TemperatureMeterOutputViewModel> GetAllTemperatureMeters()
        {
            var allTemperatureMeters = this.dbContext.TemperatureMeters.Select(x => new TemperatureMeterOutputViewModel
            {
                Id = x.Id,
                Name = x.Name,
                OfficeNumber = x.Office.Name,
            });

            return allTemperatureMeters;
        }
    }
}
