namespace AutoTelematicSystem.Data.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public required string LicensePlate { get; set; }
        public required string Color { get; set; }
        public required string VIN { get; set; }
        public required string Model { get; set; }
        public required string Make { get; set; }
        public required string YearOfManufacture { get; set; }
        public required string ImageUrl { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<Sensor> Sensors { get; set; } = [];
    }
}
