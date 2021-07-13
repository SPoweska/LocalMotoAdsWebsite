using LocalMotoAdsWebsite.Data;
using LocalMotoAdsWebsite.Models;
using LocalMotoAdsWebsite.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMotoAdsWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMakeRepository makeRepository;

        public HomeController(IMakeRepository makeRepository)
        {
            this.makeRepository = makeRepository;
        }

        public IActionResult Index()
        {
            var result = makeRepository.GetAllMakes();
            ViewData["make"] = result;
            return View();
        }

        public IActionResult Privacy()
        {
            var result = makeRepository.GetMake(1);
            ViewData["make"] = result;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
