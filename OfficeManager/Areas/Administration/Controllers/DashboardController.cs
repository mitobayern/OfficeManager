namespace OfficeManager.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OfficeManager.Areas.Administration.ViewModels.Dashboard;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using System.Linq;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IOfficesService officesService;

        public DashboardController(ApplicationDbContext dbContext, IOfficesService officesService)
        {
            this.dbContext = dbContext;
            this.officesService = officesService;
        }

        public IActionResult Index()
        {
            var totalArea = this.officesService.GetAllOffices().Sum(x => x.Area);
            var availavleArea = this.officesService.GetAllAvailableOffices().Sum(x => x.Area);

            var result = new DashboardOutputViewModel
            {
                TotalArea = totalArea,
                RentedArea = totalArea - availavleArea,
                AvailableArea = availavleArea,
            };

            return this.View(result);
        }
    }
}
