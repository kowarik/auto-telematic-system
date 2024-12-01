namespace AutoTelematicSystem.Data.Entities
{
    public class SensorType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public List<Sensor> Sensors { get; set; } = [];
        public List<Error> Errors { get; set; } = [];
    }
}
