namespace OfficeManager.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ChartsController : Controller
    {
        public IActionResult Index(int id)
        {

            return this.View(id);
        }
    }
}
