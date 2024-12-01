using AutoTelematicSystem.Data;
using AutoTelematicSystem.Data.Entities;
using AutoTelematicSystem.Dtos.TelematicsData;
using Microsoft.EntityFrameworkCore;

namespace AutoTelematicSystem.Services
{
    public interface ISensorService
    {
        public Task UpdateSensor(AddSensorDataDto sensorDto, int carId);
        public Task UpdateLocation(double latitude, double longitude, int carId);
        public string EvaluateSensor(double? value, double minValue, double maxValue);
    }

    public class SensorService : ISensorService
    {
        private readonly AppDbContext _context;

        public SensorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateLocation(double latitude, double longitude, int carId)
        {
            var car = await _context.Cars.FindAsync(carId);

            if (car == null)
            {
                throw new Exception("Car not found.");
            }

            car.Latitude = latitude;
            car.Longitude = longitude;

            _context.Entry(car).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateSensor(AddSensorDataDto sensorDto, int carId)
        {
            var sensor = await _context.Sensors
                .Include(s => s.SensorType)
                .Include(s => s.ErrorLogs)
                .ThenInclude(s => s.Error)
                .FirstOrDefaultAsync(s => s.Id == sensorDto.SensorId && s.CarId == carId);

            if (sensor == null)
            {
                throw new Exception("Sensor not found.");
            }

            sensor.Value = sensorDto.Value;
            sensor.TimeStamp = DateTime.Now;

            if (sensorDto.ErrorCodes != null)
            {
                var newErrorCodesSet = new HashSet<string>(sensorDto.ErrorCodes);

                var errorLogsToRemove = sensor.ErrorLogs
                    .Where(e => !newErrorCodesSet.Contains(e.Error.Code))
                    .ToList();

                foreach (var errorLog in errorLogsToRemove)
                {
                    _context.ErrorLogs.Remove(errorLog);
                }

                foreach (var errorCode in sensorDto.ErrorCodes)
                {
                    var error = await _context.Errors
                        .FirstOrDefaultAsync(e => e.Code == errorCode && e.SensorTypeId == sensor.SensorType.Id);

                    if (error == null)
                    {
                        throw new Exception("Error not found.");
                    }

                    if (!sensor.ErrorLogs.Any(e => e.Error.Code == error.Code))
                    {
                        var newErrorLog = new ErrorLog
                        {
                            ErrorId = error.Id,
                            SensorId = sensor.Id,
                            TimeStamp = DateTime.Now
                        };
                        await _context.ErrorLogs.AddAsync(newErrorLog);
                    }
                }
            }

            _context.Entry(sensor).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public string EvaluateSensor(double? value, double minValue, double maxValue)
        {
            if (value == null)
            {
                return "warning";
            }

            if (value < minValue || value > maxValue)
            {
                return "danger";
            }

            return "success";
        }
    }
}
