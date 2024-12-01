namespace AutoTelematicSystem.Dtos.Car
{
    public class AddCarDto
    {
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string YearOfManufacture { get; set; }
        public string ImageUrl { get; set; }
    }
}
