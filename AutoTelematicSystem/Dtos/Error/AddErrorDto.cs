namespace AutoTelematicSystem.Dtos.Error
{
    public class AddErrorDto
    {
        public int SensorTypeId { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
    }
}
