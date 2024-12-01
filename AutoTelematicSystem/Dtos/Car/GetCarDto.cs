using AutoTelematicSystem.Dtos.Sensor;

namespace AutoTelematicSystem.Dtos.Car
{
    public class GetCarDto
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string YearOfManufacture { get; set; }
        public string ImageUrl { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<GetSensorDto> Sensors { get; set; } = [];
    }
}
