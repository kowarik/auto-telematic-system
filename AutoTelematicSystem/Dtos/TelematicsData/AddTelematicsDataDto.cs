namespace AutoTelematicSystem.Dtos.TelematicsData
{
    public class AddTelematicsDataDto
    {
        public int CarId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<AddSensorDataDto>? Sensors { get; set; }
    }
}
