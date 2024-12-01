namespace AutoTelematicSystem.Dtos.TelematicsData
{
    public class AddSensorDataDto
    {
        public int SensorId { get; set; }
        public required double Value { get; set; }
        public ICollection<string>? ErrorCodes { get; set; }
    }
}
