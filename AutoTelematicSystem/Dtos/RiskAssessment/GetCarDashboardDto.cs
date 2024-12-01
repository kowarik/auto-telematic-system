namespace AutoTelematicSystem.Dtos.RiskAssessment
{
    public class GetCarDashboardDto
    {
        public int CarId { get; set; }
        public string LicensePlate { get; set; }
        public List<GetSensorDashboardDto>? Sensors { get; set; }
    }

}
