using AutoTelematicSystem.Dtos.Error;

namespace AutoTelematicSystem.Dtos.RiskAssessment
{
    public class GetSensorDashboardDto
    {
        public int Id { get; set; }
        public string SensorType { get; set; }
        public string? Value { get; set; }
        public string? ValueEvaluation { get; set; }
        
        public string? Description { get; set; }
        public DateTime? TimeStamp { get; set; }
        public List<GetErrorDto> Errors { get; set; }
        public GetSensorRiskAssessmentDto? RiskAssessment { get; set; }
    }
}
