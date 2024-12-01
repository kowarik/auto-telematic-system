namespace AutoTelematicSystem.Data.Entities
{
    public class SensorRiskAssessment
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }
        public string RiskCode { get; set; }
        public string Recommendations { get; set; }
        public DateTime AssessedAt { get; set; }
    }
}
