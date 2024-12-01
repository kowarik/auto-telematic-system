using AutoTelematicSystem.Data;
using AutoTelematicSystem.Dtos.TelematicsData;
using AutoTelematicSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTelematicSystem.Controllers
{
    [Route("api/telematic")]
    [ApiController]
    public class TelematicApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ISensorService _sensorService;

        public TelematicApiController(AppDbContext context, ISensorService sensorService)
        {
            _context = context;
            _sensorService = sensorService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSensorData([FromBody] AddTelematicsDataDto sensorDataDto)
        {
            if (sensorDataDto == null || sensorDataDto.Sensors == null || sensorDataDto.Sensors.Count == 0)
            {
                return BadRequest("Invalid sensor data.");
            }

            var car = await _context.Cars.FindAsync(sensorDataDto.CarId);
            if (car == null)
            {
                return BadRequest("Invalid car.");
            }

            foreach (var sensor in sensorDataDto.Sensors)
            {
                await _sensorService.UpdateSensor(sensor, sensorDataDto.CarId);
            }

            await _sensorService.UpdateLocation(sensorDataDto.Latitude, sensorDataDto.Longitude, sensorDataDto.CarId);

            return Ok("Success!");
        }
    }
}
