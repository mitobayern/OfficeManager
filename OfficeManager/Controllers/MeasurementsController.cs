namespace OfficeManager.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
                EndOfLastPeriod = this.measurementsService.IsFirstPeriod()
                                ? this.measurementsService.GetEndOfLastPeriod().AddDays(-1)
                                : this.measurementsService.GetEndOfLastPeriod(),
                LastPeriod = this.measurementsService.GetLastPeriodAsText(),
                StartOfPeriod = this.measurementsService.GetStartOfNewPeroid(),
                EndOfPeriod = this.measurementsService.GetEndOfNewPeriod(),
                Offices = this.measurementsService.GetOfficesWithLastMeasurements(),
            };

            return this.View(resultViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeasurementsAsync(CreateMeasurementsInputViewModel input)
        {
            if (!this.ModelState.IsValid ||
                !this.ValidateMeasurements(input.Offices) ||
                !this.ValidatePeriod(input.StartOfPeriod, input.EndOfPeriod))
            {
                return this.View(input);
            }

            await this.measurementsService.CreateAllMeasurementsAsync(input);

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
        public async Task<IActionResult> InitialMeasurementsAsync(CreateInitialMeasurementsInputViewModel input)
        {
            if (!this.ModelState.IsValid || !this.ValidateMeasurements(input.Offices))
            {
                return this.View(input);
            }

            await this.measurementsService.CreateInitialMeasurementsAsync(input);

            return this.Redirect("/Home/Index");
        }

        private bool ValidatePeriod(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            DateTime endOfLastPeriod = this.measurementsService.IsFirstPeriod()
                                ? this.measurementsService.GetEndOfLastPeriod().AddDays(-1)
                                : this.measurementsService.GetEndOfLastPeriod();

            if (DateTime.Compare(endOfPeriod, startOfPeriod) != 1 ||
                    DateTime.Compare(startOfPeriod, endOfLastPeriod) != 1)
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
