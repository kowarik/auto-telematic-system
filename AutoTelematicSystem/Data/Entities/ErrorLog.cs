namespace AutoTelematicSystem.Data.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public int ErrorId { get; set; }
        public Error Error { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
