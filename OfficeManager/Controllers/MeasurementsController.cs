namespace OfficeManager.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.Measurements;

    public class MeasurementsController : Controller
    {
        private const string CreatedOnDescending = "createdOn_desc";
        private readonly ApplicationDbContext dbContext;
        private readonly IMeasurementsService measurementsService;

        public MeasurementsController(ApplicationDbContext dbContext, IMeasurementsService measurementsService)
        {
            this.dbContext = dbContext;
            this.measurementsService = measurementsService;
        }

        public IActionResult CreateMeasurements()
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

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

            return this.Redirect("/Measurements/All");
        }

        public IActionResult InitialMeasurements()
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

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

            return this.Redirect("/Measurements/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            var allMeasurements = this.measurementsService.GetAllMeasurements();
            allMeasurements = this.OrderMeasurements(sortOrder, currentFilter, searchString, pageNumber, allMeasurements);
            int pageSize = GetPageSize(rowsPerPage, allMeasurements);

            this.ViewData["RowsPerPage"] = pageSize;

            return this.View(await PaginatedList<AllMeasurementsOutputViewModel>.CreateAsync(allMeasurements.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Edit(DateTime period)
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            var measurementsToEdit = this.measurementsService.GetMeasurementsByStartingPeriod(period);

            return this.View(measurementsToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateMeasurementsInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.measurementsService.EditAllMeasurementsAsync(input);

            return this.Redirect("/Measurements/All");
        }

        private static int GetPageSize(string rowsPerPage, IQueryable<AllMeasurementsOutputViewModel> allElectricityMeters)
        {

            int pageSize;

            if (string.IsNullOrEmpty(rowsPerPage))
            {
                pageSize = 5;
            }
            else if (rowsPerPage == "All")
            {
                pageSize = allElectricityMeters.Count();
            }
            else
            {
                pageSize = int.Parse(rowsPerPage);
            }

            return pageSize;
        }

        private IQueryable<AllMeasurementsOutputViewModel> OrderMeasurements(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<AllMeasurementsOutputViewModel> allElectricityMeters)
        {
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["MeasurementsSortParam"] = string.IsNullOrEmpty(sortOrder) ? CreatedOnDescending : string.Empty;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;

            allElectricityMeters = sortOrder switch
            {
                CreatedOnDescending => allElectricityMeters.OrderByDescending(s => s.StartOfPeriod),
                _ => allElectricityMeters.OrderBy(s => s.StartOfPeriod),
            };
            return allElectricityMeters;
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

        private bool ValidateUser(string userName)
        {
            if (this.dbContext.Users.First(d => d.UserName == userName).IsEnabled == null)
            {
                return false;
            }

            return true;
        }
    }
}
