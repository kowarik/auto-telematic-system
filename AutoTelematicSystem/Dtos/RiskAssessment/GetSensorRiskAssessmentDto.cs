namespace AutoTelematicSystem.Dtos.RiskAssessment
{
    public class GetSensorRiskAssessmentDto
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string RiskCode { get; set; }
        public string Recommendations { get; set; }
        public DateTime AssessedAt { get; set; }
    }
}
