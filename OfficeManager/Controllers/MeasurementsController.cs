namespace OfficeManager.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.Measurements;

    public class MeasurementsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMeasurementsService measurementsService;

        public MeasurementsController(ApplicationDbContext dbContext, IMeasurementsService measurementsService)
        {
            this.dbContext = dbContext;
            this.measurementsService = measurementsService;
        }
        public IActionResult CreateElectricityMeasurements()
        {
            if (this.dbContext.ElectricityMeasurements.Count() == 0)
            {
                return this.Redirect("/Measurements/InitialElectricityMeasurements");
            }

            string period = this.dbContext.ElectricityMeasurements.OrderByDescending(x => x.Id).First().Period;

            var elMeters = this.dbContext.ElectricityMeters.Select(x => new ElectricityMeasurementInputViewModel
            {
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToList();

            //var temperatureMeters = this.dbContext.TemperatureMeters.Select(x => new TemperatureMeasurementInputViewModel
            //{
            //    Name = x.Name
            //})
            //.OrderBy(x => x.Name)
            //.ToList();


            return this.View(new CreateElectricityMeasurementsInputViewModel { LastPeriod = period, ElectricityMeters = elMeters, /*TemperatureMeters = temperatureMeters*/ });
        }

        public IActionResult InitialElectricityMeasurements()
        {
            var elMeters = this.dbContext.ElectricityMeters.Select(x => new ElectricityMeasurementInputViewModel
            {
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToList();

            return this.View(new CreateElectricityMeasurementsInputViewModel { ElectricityMeters = elMeters });
        }
        

        public IActionResult CreateTemperatureMeasurements()
        {
            if (this.dbContext.TemperatureMeasurements.Count() == 0)
            {
                return this.Redirect("/Measurements/InitialTemperatureMeasurements");
            }

            string period = this.dbContext.TemperatureMeasurements.OrderByDescending(x => x.Id).First().Period;

            var temperatureMeters = this.dbContext.TemperatureMeters.Select(x => new TemperatureMeasurementInputViewModel
            {
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToList();

            return this.View(new CreateTemperatureMeasurementsInputViewModel { LastPeriod = period, TemperatureMeters = temperatureMeters });
        }

        public IActionResult InitialTemperatureMeasurements()
        {
            var temperatureMeters = this.dbContext.TemperatureMeters.Select(x => new TemperatureMeasurementInputViewModel
            {
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToList();

            return this.View(new CreateTemperatureMeasurementsInputViewModel { TemperatureMeters = temperatureMeters });
        }

        [HttpPost]
        public IActionResult CreateElectricityMeasurements(CreateElectricityMeasurementsInputViewModel input)
        {
            this.measurementsService.CreateElectricityMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult CreateTemperatureMeasurements(CreateTemperatureMeasurementsInputViewModel input)
        {
            this.measurementsService.CreateTemperatureMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        public IActionResult CreateMeasurements()
        {
            if (this.dbContext.ElectricityMeasurements.Count() == 0)
            {
                return this.Redirect("/Measurements/InitialElectricityMeasurements");
            }

            string period = this.dbContext.ElectricityMeasurements.OrderByDescending(x => x.Id).First().Period;
            var endOfLastPeriod = this.dbContext.ElectricityMeasurements.OrderByDescending(x => x.Id).First().EndOfPeriod;
            var startOfNewPerwiod = endOfLastPeriod.AddDays(1);

            var elMeters = this.dbContext.ElectricityMeters.Select(x => new ElectricityMeasurementInputViewModel
            {
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToList();

            var temperatureMeters = this.dbContext.TemperatureMeters.Select(x => new TemperatureMeasurementInputViewModel
            {
                Name = x.Name
            })
            .OrderBy(x => x.Name)
            .ToList();

            var offices = this.dbContext.Offices.Select(x => new OfficeMeasurementsInputViewModel
            {
                Name = x.Name,
                ElectricityMeter = new ElectricityMeasurementInputViewModel 
                {  
                 Name = x.ElectricityMeter.Name
                },
                TemperatureMeters = x.TemperatureMeters.Select(y=> new TemperatureMeasurementInputViewModel 
                {
                    Name = y.Name                    
                }).ToList()
            }).ToList();

            var result = new CreateMeasurementsInputViewModel
            {
                StarOfPeriod = startOfNewPerwiod,
                LastPeriod = period,
                Offices = offices
            };

            return this.View(result);
        }

        [HttpPost]
        public IActionResult CreateMeasurements(CreateMeasurementsInputViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            if (DateTime.Compare(input.EndOfPeriod, input.StarOfPeriod) != 1)
            {
                return View(input);
            }



            return this.Redirect("/Home/Index");
        }
    }
}
