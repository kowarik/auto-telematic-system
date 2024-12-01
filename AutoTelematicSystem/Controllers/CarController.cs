using AutoTelematicSystem.Data;
using AutoTelematicSystem.Data.Entities;
using AutoTelematicSystem.Dtos.Car;
using AutoTelematicSystem.Dtos.Error;
using AutoTelematicSystem.Dtos.Sensor;
using AutoTelematicSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoTelematicSystem.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISensorService _sensorService;

        public CarController(AppDbContext context, ISensorService sensorService)
        {
            _context = context;
            _sensorService = sensorService;
        }

        [HttpGet]
        public async Task<IActionResult> CarList(string search)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => EF.Functions.Like(c.LicensePlate, $"%{search}%"));
                ViewData["Search"] = search;
            }

            var cars = await query
                .Select(c => new GetCarDto
                {
                    Id = c.Id,
                    LicensePlate = c.LicensePlate,
                    Model = c.Model,
                    Make = c.Make,
                    ImageUrl = c.ImageUrl,
                }).ToListAsync();

            return View(cars);
        }

        [HttpGet]
        public async Task<IActionResult> CarDetail(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Sensors)
                    .ThenInclude(s => s.SensorType)
                .Include(c => c.Sensors)
                .ThenInclude(s => s.ErrorLogs)
                .ThenInclude(e => e.Error)
                .Where(c => c.Id == id)
                .Select(c => new GetCarDto
                {
                    Id = c.Id,
                    LicensePlate = c.LicensePlate,
                    Color = c.Color,
                    VIN = c.VIN,
                    Model = c.Model,
                    Make = c.Make,
                    YearOfManufacture = c.YearOfManufacture,
                    ImageUrl = c.ImageUrl,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Sensors = c.Sensors.Select(s => new GetSensorDto
                    {
                        Id = s.Id,
                        CarId = s.CarId,
                        SensorTypeId = s.SensorType.Id,
                        SensorType = s.SensorType.Name,
                        Value = $"{s.SensorType.Description}: {s.Value}",
                        ValueEvaluation = _sensorService.EvaluateSensor(s.Value, s.SensorType.MinValue, s.SensorType.MaxValue),
                        Description = s.Description,
                        TimeStamp = s.TimeStamp,
                        Errors = s.ErrorLogs.Select(e => new GetErrorDto
                        {
                            Id = e.Id,
                            Code = e.Error.Code,
                            Description = e.Error.Description,
                            Timestamp = e.TimeStamp
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return View(car);
        }

        [HttpGet]
        public IActionResult CarAdd()
        {
            return View(new AddCarDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarAdd(AddCarDto carDto)
        {
            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    LicensePlate = carDto.LicensePlate,
                    Color = carDto.Color,
                    VIN = carDto.VIN,
                    Model = carDto.Model,
                    Make = carDto.Make,
                    YearOfManufacture = carDto.YearOfManufacture,
                    ImageUrl = carDto.ImageUrl,
                };
                await _context.AddAsync(car);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(CarList));
            }
            return View(carDto);
        }

        [HttpGet]
        public async Task<IActionResult> CarEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Select(c => new GetCarForEditDto
                {
                    Id = c.Id,
                    LicensePlate = c.LicensePlate,
                    Color = c.Color,
                    VIN = c.VIN,
                    Model = c.Model,
                    Make = c.Make,
                    YearOfManufacture = c.YearOfManufacture,
                    ImageUrl = c.ImageUrl,
                    Sensors = c.Sensors.Select(s => new GetSensorForCarEditDto
                    {
                        SensorTypeId = s.SensorTypeId,
                        SensorTypeName = s.SensorType.Name,
                        Description = s.Description
                    }).ToList()
                })
                .FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.SensorTypes = await _context.SensorTypes.ToListAsync();

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarEdit(int id, GetCarForEditDto carDto)
        {
            if (id != carDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var car = await _context.Cars
                    .Include(c => c.Sensors)
                    .FirstOrDefaultAsync(c => c.Id == carDto.Id);

                if (car != null)
                {
                    car.LicensePlate = carDto.LicensePlate;
                    car.Color = carDto.Color;
                    car.VIN = carDto.VIN;
                    car.Model = carDto.Model;
                    car.Make = carDto.Make;
                    car.YearOfManufacture = carDto.YearOfManufacture;
                    car.ImageUrl = carDto.ImageUrl;

                    // Обновление сенсоров
                    foreach (var sensorDto in carDto.Sensors)
                    {
                        var existingSensor = car.Sensors.FirstOrDefault(s => s.SensorTypeId == sensorDto.SensorTypeId && s.Description == sensorDto.Description);

                        if (existingSensor == null)
                        {
                            car.Sensors.Add(new Sensor { CarId = car.Id, SensorTypeId = sensorDto.SensorTypeId, Description = sensorDto.Description });
                        }
                        else
                        {
                            existingSensor.Description = sensorDto.Description;
                        }
                    }
                    var removedSensors = car.Sensors
                        .Where(s => !carDto.Sensors.Any(dto => dto.SensorTypeId == s.SensorTypeId && dto.Description == s.Description))
                        .ToList();
                    foreach (var sensor in removedSensors)
                    {
                        car.Sensors.Remove(sensor);
                    }

                    _context.Entry(car).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(CarDetail), new { car.Id });
                }
            }

            ViewBag.SensorTypes = await _context.SensorTypes.ToListAsync();

            return View(carDto);
        }

        public async Task<IActionResult> CarDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Select(c => new GetCarDto
                {
                    Id = c.Id,
                    LicensePlate = c.LicensePlate,
                    YearOfManufacture = c.YearOfManufacture,
                    Model = c.Model,
                    Make = c.Make,
                    ImageUrl = c.ImageUrl
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("CarDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarDeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CarList));
        }
    }
}
