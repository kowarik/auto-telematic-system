namespace AutoTelematicSystem.Data.Entities
{
    public class Error
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }

        public int SensorTypeId { get; set; }
        public SensorType SensorType { get; set; }

        public List<ErrorLog> ErrorLogs { get; set; }
    }
}
