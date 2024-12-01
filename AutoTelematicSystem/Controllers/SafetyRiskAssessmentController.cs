using AutoTelematicSystem.Data;
using AutoTelematicSystem.Data.Entities;
using AutoTelematicSystem.Dtos.Error;
using AutoTelematicSystem.Dtos.RiskAssessment;
using AutoTelematicSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoTelematicSystem.Controllers
{
    [Authorize]
    public class SafetyRiskAssessmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISensorService _sensorService;

        public SafetyRiskAssessmentController(AppDbContext context, ISensorService sensorService)
        {
            _context = context;
            _sensorService = sensorService;
        }

        [HttpGet]
        public async Task<IActionResult> CarDashboard(int carId)
        {
            var car = await _context.Cars
                .Include(c => c.Sensors)
                    .ThenInclude(s => s.SensorType)
                .Include(c => c.Sensors)
                    .ThenInclude(s => s.ErrorLogs)
                        .ThenInclude(e => e.Error)
                .Include(c => c.Sensors)
                    .ThenInclude(s => s.RiskAssessment)
                .FirstOrDefaultAsync(c => c.Id == carId);

            if (car == null)
            {
                return NotFound();
            }

            var viewModel = new GetCarDashboardDto
            {
                CarId = car.Id,
                LicensePlate = car.LicensePlate,
                Sensors = car.Sensors.Select(s => new GetSensorDashboardDto
                {
                    Id = s.Id,
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
                        Timestamp = e.TimeStamp,
                        SensorTypeId = e.Sensor.SensorTypeId
                    }).ToList(),
                    RiskAssessment = s.RiskAssessment == null ? null : new GetSensorRiskAssessmentDto
                    {
                        Row = s.RiskAssessment.Row,
                        Column = s.RiskAssessment.Column,
                        RiskCode = s.RiskAssessment.RiskCode,
                        Recommendations = s.RiskAssessment.Recommendations,
                        AssessedAt = s.RiskAssessment.AssessedAt
                    }
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssessRisk(int sensorId, string riskCode, string recommendations)
        {
            var riskRatings = new List<RiskRatingModel>
            {
                new() { Row = 5, Column = 1, Color = "danger", Code = "5A" },
                new() { Row = 5, Column = 2, Color = "danger", Code = "5B" },
                new() { Row = 5, Column = 3, Color = "danger", Code = "5C" },
                new() { Row = 5, Column = 4, Color = "warning", Code = "5D" },
                new() { Row = 5, Column = 5, Color = "warning", Code = "5E" },
                new() { Row = 4, Column = 1, Color = "danger", Code = "4A" },
                new() { Row = 4, Column = 2, Color = "danger", Code = "4B" },
                new() { Row = 4, Column = 3, Color = "warning", Code = "4C" },
                new() { Row = 4, Column = 4, Color = "warning", Code = "4D" },
                new() { Row = 4, Column = 5, Color = "warning", Code = "4E" },
                new() { Row = 3, Column = 1, Color = "danger", Code = "3A" },
                new() { Row = 3, Column = 2, Color = "warning", Code = "3B" },
                new() { Row = 3, Column = 3, Color = "warning", Code = "3C" },
                new() { Row = 3, Column = 4, Color = "warning", Code = "3D" },
                new() { Row = 3, Column = 5, Color = "success", Code = "3E" },
                new() { Row = 2, Column = 1, Color = "warning", Code = "2A" },
                new() { Row = 2, Column = 2, Color = "warning", Code = "2B" },
                new() { Row = 2, Column = 3, Color = "warning", Code = "2C" },
                new() { Row = 2, Column = 4, Color = "success", Code = "2D" },
                new() { Row = 2, Column = 5, Color = "success", Code = "2E" },
                new() { Row = 1, Column = 1, Color = "warning", Code = "1A" },
                new() { Row = 1, Column = 2, Color = "success", Code = "1B" },
                new() { Row = 1, Column = 3, Color = "success", Code = "1C" },
                new() { Row = 1, Column = 4, Color = "success", Code = "1D" },
                new() { Row = 1, Column = 5, Color = "success", Code = "1E" },
            };

            var riskRating = riskRatings.FirstOrDefault(r => r.Code == riskCode);
            if (riskRating == null)
            {
                return BadRequest("Invalid risk selection.");
            }

            var sensor = await _context.Sensors
                .Include(s => s.RiskAssessment)
                .FirstOrDefaultAsync(s => s.Id == sensorId);

            if (sensor == null)
            {
                return NotFound();
            }

            if (sensor.RiskAssessment == null)
            {
                sensor.RiskAssessment = new SensorRiskAssessment
                {
                    SensorId = sensorId,
                    Row = riskRating.Row,
                    Column = riskRating.Column,
                    RiskCode = riskRating.Code,
                    Recommendations = recommendations,
                    AssessedAt = DateTime.Now
                };
                _context.SensorRiskAssessments.Add(sensor.RiskAssessment);
            }
            else
            {
                sensor.RiskAssessment.Row = riskRating.Row;
                sensor.RiskAssessment.Column = riskRating.Column;
                sensor.RiskAssessment.RiskCode = riskRating.Code;
                sensor.RiskAssessment.Recommendations = recommendations;
                sensor.RiskAssessment.AssessedAt = DateTime.Now;
                _context.SensorRiskAssessments.Update(sensor.RiskAssessment);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("CarDashboard", new { carId = sensor.CarId });
        }
    }
}
