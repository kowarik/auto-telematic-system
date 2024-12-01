namespace AutoTelematicSystem.Dtos.Car
{
    public class GetCarForEditDto
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string YearOfManufacture { get; set; }
        public string ImageUrl { get; set; }

        public List<GetSensorForCarEditDto> Sensors { get; set; } = new List<GetSensorForCarEditDto>();
    }

    public class GetSensorForCarEditDto
    {
        public int SensorTypeId { get; set; }
        public string? SensorTypeName { get; set; }
        public string? Description { get; set; }
    }
}
