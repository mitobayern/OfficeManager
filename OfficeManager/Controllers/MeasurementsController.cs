namespace OfficeManager.Controllers
{
    using System;
    using System.Collections.Generic;
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
            if (!ModelState.IsValid || 
                !ValidateMeasurements(input.Offices) || 
                !ValidatePeriod(input.StartOfPeriod, input.EndOfPeriod))
            {
                return this.View(input);
            }

            this.measurementsService.CreateAllMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        public IActionResult InitialMeasurements()
        {
            var resultViewModel = new CreateInitialMeasurementsInputViewModel
            {
                Offices = this.measurementsService.GetOfficesWithLastMeasurements(),
            };

            return this.View(resultViewModel);
        }

        [HttpPost]
        public IActionResult InitialMeasurements(CreateInitialMeasurementsInputViewModel input)
        {
            if (!ModelState.IsValid || ValidateMeasurements(input.Offices))
            {
                return this.View(input);
            }

            this.measurementsService.CreateInitialMeasurements(input);

            return this.Redirect("/Home/Index");
        }

        private bool ValidatePeriod(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            if (DateTime.Compare(endOfPeriod, startOfPeriod) != 1 ||
                    DateTime.Compare(startOfPeriod, this.measurementsService.GetEndOfLastPeriod()) != 1)
            {
                return false;
            }

            return true;
        }

        private bool ValidateMeasurements(List<OfficeMeasurementsInputViewModel> offices)
        {
            var lastMeasurements = this.measurementsService.GetOfficesWithLastMeasurements();

            foreach (var office in offices)
            {
                if (office.ElectricityMeter.NightTimeMeasurement <
                    lastMeasurements.FirstOrDefault(x => x.ElectricityMeter.Name == office.ElectricityMeter.Name)
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
