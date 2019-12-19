using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Classroom.SimpleCRM.WebApi.Models;

namespace Classroom.SimpleCRM.WebApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [Route("home")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            return View();
        }
        [Route("privacy")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
