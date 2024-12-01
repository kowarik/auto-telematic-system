using AutoTelematicSystem.Data;
using AutoTelematicSystem.Dtos.Car;
using AutoTelematicSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AutoTelematicSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars
                        .Select(c => new GetCarDto
                        {
                            Id = c.Id,
                            LicensePlate = c.LicensePlate,
                            Model = c.Model,
                            Make = c.Make,
                            ImageUrl = c.ImageUrl,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude
                        })
                        .ToListAsync();

            return View(cars);
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
