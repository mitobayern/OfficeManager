namespace OfficeManager.Controllers
{
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

            return this.View(new CreateElectricityMeasurementsInputViewModel { LastPeriod = period, ElectricityMeters = elMeters });
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

        [HttpPost]
        public IActionResult CreateElectricityMeasurements(CreateElectricityMeasurementsInputViewModel input)
        {
            this.measurementsService.CreateElectricityMeasurements(input);

            return this.Redirect("/Home/Index");
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
        public IActionResult CreateTemperatureMeasurements(CreateTemperatureMeasurementsInputViewModel input)
        {
            this.measurementsService.CreateTemperatureMeasurements(input);

            return this.Redirect("/Home/Index");
        }
    }
}
