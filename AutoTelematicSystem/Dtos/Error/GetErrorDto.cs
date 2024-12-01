namespace AutoTelematicSystem.Dtos.Error
{
    public class GetErrorDto
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public string? Description { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? SensorTypeId { get; set; }
    }
}
