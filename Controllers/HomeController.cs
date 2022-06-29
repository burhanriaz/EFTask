using EFTask.Data;
using EFTask.Models;
using EFTask.Models.Custom_Bind_AppSetting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EFTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<MySettings> options;


        public HomeController(ILogger<HomeController> logger, IOptions<MySettings> options)
        {
            _logger = logger;
            this.options = options;
        }
        public IActionResult Index()
        {
            ViewBag.FirstName = options.Value.FirstName;
            ViewBag.LastName = options.Value.LastName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
