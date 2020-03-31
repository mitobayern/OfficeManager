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
            //this.measurementsService.CreateElectricityMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult CreateTemperatureMeasurements(CreateTemperatureMeasurementsInputViewModel input)
        {
            //this.measurementsService.CreateTemperatureMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        public IActionResult CreateMeasurements()
        {
            if (this.dbContext.ElectricityMeasurements.Count() == 0)
            {
                return this.Redirect("/Measurements/InitialMeasurements");
            }

            var resultViewModel = new CreateMeasurementsInputViewModel
            {
                EndOfLastPeriod = this.measurementsService.GetEndOfLastPeriod(),
                LastPeriod = this.measurementsService.GetLastPeriodAsText(),
                StartOfPeriod = this.measurementsService.GetStartOfNewPeroid(),
                EndOfPeriod = this.measurementsService.GetEndOfNewPeriod(),
                Offices = this.measurementsService.GetOfficesWithLastMeasurements(),
            };

            return this.View(resultViewModel);
        }

        [HttpPost]
        public IActionResult CreateMeasurements(CreateMeasurementsInputViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            if (!Validate(input))
            {
                return this.View(input);
            }

            this.measurementsService.CreateAllMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        private bool Validate(CreateMeasurementsInputViewModel input)
        {
            if (DateTime.Compare(input.EndOfPeriod, input.StartOfPeriod) != 1 ||
                DateTime.Compare(input.StartOfPeriod, this.measurementsService.GetEndOfLastPeriod()) != 1)
            {
                return false;
            }

            var lastMeasurements = this.measurementsService.GetOfficesWithLastMeasurements();

            foreach (var office in input.Offices)
            {
                if (office.ElectricityMeter.NightTimeMeasurement < 
                    lastMeasurements.FirstOrDefault(x=>x.ElectricityMeter.Name == office.ElectricityMeter.Name)
                    .ElectricityMeter.NightTimeMinValue)
                {
                    return false;      
                }
                if (office.ElectricityMeter.DayTimeMeasurement <
                    lastMeasurements.FirstOrDefault(x => x.ElectricityMeter.Name == office.ElectricityMeter.Name)
                    .ElectricityMeter.DayTimeMinValue)
                {
                    return false;
                }

                foreach (var temperatureMeter in office.TemperatureMeters)
                {
                    var lastTemperatureMeter = lastMeasurements.SelectMany(x => x.TemperatureMeters).FirstOrDefault(x => x.Name == temperatureMeter.Name);
                    if (temperatureMeter.HeatingMeasurement < lastTemperatureMeter.HeatingMinValue)
                    {
                        return false;
                    }
                    if (temperatureMeter.CoolingMeasurement < lastTemperatureMeter.CoolingMinValue)
                    {
                        return false;
                    }
                }

            }

            return true;
        }
    }
}
