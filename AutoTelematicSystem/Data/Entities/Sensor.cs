namespace AutoTelematicSystem.Data.Entities
{
    public class Sensor
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }

        public int SensorTypeId { get; set; }
        public SensorType SensorType { get; set; }

        public double? Value{ get; set; }
        public string? Description { get; set; }

        public DateTime? TimeStamp { get; set; }

        public List<ErrorLog> ErrorLogs { get; set; } = [];

        public SensorRiskAssessment? RiskAssessment { get; set; }
    }
}
