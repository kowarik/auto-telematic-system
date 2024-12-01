using AutoTelematicSystem.Dtos.Error;

namespace AutoTelematicSystem.Dtos.SensorType
{
    public class GetSensorTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<GetErrorDto> Errors { get; set; } = [];
    }
}
