using AutoTelematicSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTelematicSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<SensorRiskAssessment> SensorRiskAssessments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ErrorLog>()
                .HasOne(log => log.Sensor)
                .WithMany(sensor => sensor.ErrorLogs)
                .HasForeignKey(log => log.SensorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ErrorLog>()
                .HasOne(log => log.Error)
                .WithMany(error => error.ErrorLogs)
                .HasForeignKey(log => log.ErrorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Sensor>()
            .HasOne(s => s.RiskAssessment)
            .WithOne(r => r.Sensor)
            .HasForeignKey<SensorRiskAssessment>(r => r.SensorId);

            base.OnModelCreating(modelBuilder);
            SeedDatabase(modelBuilder);
        }

        private void SeedDatabase(ModelBuilder modelBuilder)
        {
            var sensorTypes = new List<SensorType>
            {
                new() {Id = 1, Name = "Speed Sensor", Description = "Speed (km/h)", MinValue = 0, MaxValue = 200},
                new() {Id = 2, Name = "Fuel Level Sensor", Description = "Fuel Level (%)", MinValue = 0, MaxValue = 100},
                new() {Id = 3, Name = "Coolant Temperature Sensor", Description = "Coolant Temperature (°C)", MinValue = -40, MaxValue = 120},
                new() {Id = 4, Name = "Oil Temperature Sensor", Description = "Oil Temperature (°C)", MinValue = -20, MaxValue = 150},
                new() {Id = 5, Name = "Oil Pressure Sensor", Description = "Oil Pressure (Bar)", MinValue = 0, MaxValue = 10},
                new() {Id = 6, Name = "Oxygen Sensor", Description = "Oxygen Concentration (%)", MinValue = 0, MaxValue = 21},
                new() {Id = 7, Name = "Throttle Position Sensor", Description = "Throttle Position (%)", MinValue = 0, MaxValue = 100},
                new() {Id = 8, Name = "Mass Air Flow Sensor", Description = "Air Flow (g/s)", MinValue = 0, MaxValue = 500},
                new() {Id = 9, Name = "Manifold Absolute Pressure Sensor", Description = "Pressure (kPa)", MinValue = 0, MaxValue = 300},
                new() {Id = 10, Name = "Crankshaft Position Sensor", Description = "Crankshaft Position (Degrees)", MinValue = 0, MaxValue = 360},
                new() {Id = 11, Name = "Camshaft Position Sensor", Description = "Camshaft Position (Degrees)", MinValue = 0, MaxValue = 360},
                new() {Id = 12, Name = "Knock Sensor", Description = "Knock (Hz)", MinValue = 0, MaxValue = 100},
                new() {Id = 13, Name = "Intake Air Temperature Sensor", Description = "Intake Air Temperature (°C)", MinValue = -40, MaxValue = 150},
                new() {Id = 14, Name = "Steering Angle Sensor", Description = "Steering Angle (Degrees)", MinValue = -540, MaxValue = 540},
                new() {Id = 15, Name = "Rain Sensor", Description = "Rain Intensity (mm/h)", MinValue = 0, MaxValue = 200},
                new() {Id = 16, Name = "Light Sensor", Description = "Light Intensity (Lux)", MinValue = 0, MaxValue = 100000},
                new() {Id = 17, Name = "Tire Pressure Sensor", Description = "Tire Pressure (PSI)", MinValue = 20, MaxValue = 50},
                new() {Id = 18, Name = "Accelerometer", Description = "Acceleration (m/s²)", MinValue = -10, MaxValue = 10},
                new() {Id = 19, Name = "Gyroscope", Description = "Angular Velocity (°/s)", MinValue = -1000, MaxValue = 1000},
                new() {Id = 20, Name = "Parking Sensor", Description = "Distance to Obstacle (cm)", MinValue = 0, MaxValue = 500},
                new() {Id = 21, Name = "Camera", Description = "Image Data (pixels)", MinValue = 0, MaxValue = 3000},
                new() {Id = 22, Name = "Ultrasonic Sensor", Description = "Distance (cm)", MinValue = 0, MaxValue = 400},
                new() {Id = 23, Name = "Radar Sensor", Description = "Distance (m)", MinValue = 0, MaxValue = 200},
                new() {Id = 24, Name = "Lidar Sensor", Description = "Distance (m)", MinValue = 0, MaxValue = 1000},
                new() {Id = 25, Name = "Exhaust Gas Temperature Sensor", Description = "Exhaust Gas Temperature (°C)", MinValue = 0, MaxValue = 1000},
                new() {Id = 26, Name = "Cabin Temperature Sensor", Description = "Cabin Temperature (°C)", MinValue = -30, MaxValue = 50},
                new() {Id = 27, Name = "Air Quality Sensor", Description = "Air Quality (AQI)", MinValue = 0, MaxValue = 500},
                new() {Id = 28, Name = "Axle Load Sensor", Description = "Axle Load (kg)", MinValue = 0, MaxValue = 10000},
                new() {Id = 29, Name = "Seat Position Sensor", Description = "Seat Position (mm)", MinValue = 0, MaxValue = 1000},
                new() {Id = 30, Name = "Odometer", Description = "Distance Travelled (km)", MinValue = 0, MaxValue = 100000}
            };

            modelBuilder.Entity<SensorType>().HasData(sensorTypes);

            var cars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    LicensePlate = "AA1234BB",
                    Color = "Red",
                    VIN = "1HGCM82633A123456",
                    Model = "Civic",
                    Make = "Honda",
                    YearOfManufacture = "2020",
                    ImageUrl = "https://cdn4.riastatic.com/photosnew/auto/photo/honda_civic__537639209f.jpg",
                    Latitude = 50.453,
                    Longitude = 30.524
                },
                new Car
                {
                    Id = 2,
                    LicensePlate = "AB5678CD",
                    Color = "Blue",
                    VIN = "2T1BURHE0JC123456",
                    Model = "Corolla",
                    Make = "Toyota",
                    YearOfManufacture = "2021",
                    ImageUrl = "https://d2s8i866417m9.cloudfront.net/photo/10145916/photo/medium-2f65259ed6f6c1be63e4ec40feb3462a.jpg",
                    Latitude = 50.423,
                    Longitude = 30.601
                },
                new Car
                {
                    Id = 3,
                    LicensePlate = "AC3456EF",
                    Color = "Black",
                    VIN = "3GNEC13T73G123456",
                    Model = "Tahoe",
                    Make = "Chevrolet",
                    YearOfManufacture = "2019",
                    ImageUrl = "https://vehicle-images.dealerinspire.com/e254-110006886/1GNSKTKL8RR138719/5138c277917d183776cc2fe734a2ab9e.jpg",
                    Latitude = 50.467,
                    Longitude = 30.612
                },
                new Car
                {
                    Id = 4,
                    LicensePlate = "AD7890GH",
                    Color = "White",
                    VIN = "5XXGR4A65FG123456",
                    Model = "Optima",
                    Make = "Kia",
                    YearOfManufacture = "2018",
                    ImageUrl = "https://vehicle-photos-published.vauto.com/e0/d9/27/24-64d1-4874-9c19-938e0d4e436e/image-1.jpg",
                    Latitude = 50.410,
                    Longitude = 30.590
                },
                new Car
                {
                    Id = 5,
                    LicensePlate = "AE1234IJ",
                    Color = "Silver",
                    VIN = "1FTFW1EG6JK123456",
                    Model = "F-150",
                    Make = "Ford",
                    YearOfManufacture = "2020",
                    ImageUrl = "https://i.ytimg.com/vi/Deo5Z6a1Mfs/maxresdefault.jpg",
                    Latitude = 50.452,
                    Longitude = 30.545
                },
                new Car
                {
                    Id = 6,
                    LicensePlate = "AF5678KL",
                    Color = "Green",
                    VIN = "WDDGF4HB0JF123456",
                    Model = "Astra K",
                    Make = "Opel",
                    YearOfManufacture = "2018",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2d/Opel_Astra_1.6_CDTI_ecoFLEX_Dynamic_%28K%29_%E2%80%93_Frontansicht%2C_23._Dezember_2016%2C_D%C3%BCsseldorf.jpg",
                    Latitude = 50.400,
                    Longitude = 30.531
                },
                new Car
                {
                    Id = 7,
                    LicensePlate = "AG3456MN",
                    Color = "Red",
                    VIN = "ZFF78A1F000123456",
                    Model = "Niro",
                    Make = "Kia",
                    YearOfManufacture = "2019",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b3/2023_Kia_Niro_Hybrid_EX_in_Runway_Red%2C_Front_Right%2C_10-09-2023.jpg",
                    Latitude = 50.460,
                    Longitude = 30.520
                },
                new Car
                {
                    Id = 8,
                    LicensePlate = "AH6789OP",
                    Color = "Orange",
                    VIN = "3D4PH5FV5JT123456",
                    Model = "Rio",
                    Make = "Kia",
                    YearOfManufacture = "2020",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/60/2018_Kia_Rio_EX_-_Side.jpg",
                    Latitude = 50.430,
                    Longitude = 30.607
                },
                new Car
                {
                    Id = 9,
                    LicensePlate = "AI2345QR",
                    Color = "Purple",
                    VIN = "1B3CC5EB4FN123456",
                    Model = "Leaf",
                    Make = "Nissan",
                    YearOfManufacture = "2017",
                    ImageUrl = "https://cdn3.riastatic.com/photosnew/auto/photo/nissan_leaf__556711965fx.jpg",
                    Latitude = 50.481,
                    Longitude = 30.500
                },
                new Car
                {
                    Id = 10,
                    LicensePlate = "AJ8901ST",
                    Color = "Brown",
                    VIN = "19XFC2F56HE123456",
                    Model = "Civic",
                    Make = "Honda",
                    YearOfManufacture = "2022",
                    ImageUrl = "https://i.ytimg.com/vi/35vNn2jEpK8/hqdefault.jpg",
                    Latitude = 50.450,
                    Longitude = 30.515
                }
            };

            modelBuilder.Entity<Car>().HasData(cars);

            var sensors = new List<Sensor>
            {
                new Sensor
                {
                    Id = 1,
                    CarId = 1,
                    SensorTypeId = 1,
                    Value = 60,
                    Description = null,
                    TimeStamp = DateTime.Now.AddMinutes(-10)
                },
                new Sensor
                {
                    Id = 2,
                    CarId = 1,
                    SensorTypeId = 2,
                    Value = 75,
                    Description = null,
                    TimeStamp = DateTime.Now.AddMinutes(-20)
                },
                new Sensor
                {
                    Id = 3,
                    CarId = 1,
                    SensorTypeId = 3,
                    Value = 32,
                    Description = null,
                    TimeStamp = DateTime.Now.AddMinutes(-5)
                },
                new Sensor
                {
                    Id = 4,
                    CarId = 1,
                    SensorTypeId = 4,
                    Value = 75,
                    Description = null,
                    TimeStamp = DateTime.Now.AddMinutes(-15)
                },
                new Sensor
                {
                    Id = 5,
                    CarId = 1,
                    SensorTypeId = 30,
                    Value = 88000,
                    Description = null,
                    TimeStamp = DateTime.Now.AddMinutes(-15)
                }
            };
            modelBuilder.Entity<Sensor>().HasData(sensors);

            var errors = new List<Error>
            {
                // Speed Sensor
                new() { Id = 1, Code = "P0500", Description = "Vehicle Speed Sensor Malfunction", SensorTypeId = 1 },
                new() { Id = 2, Code = "P0501", Description = "Vehicle Speed Sensor Range/Performance Problem", SensorTypeId = 1 },
                new() { Id = 3, Code = "P0502", Description = "Vehicle Speed Sensor Low Input", SensorTypeId = 1 },
                new() { Id = 4, Code = "P0503", Description = "Vehicle Speed Sensor High Input", SensorTypeId = 1 },
                new() { Id = 5, Code = "P0720", Description = "Output Speed Sensor Circuit Malfunction", SensorTypeId = 1 },

                // Fuel Level Sensor
                new() { Id = 6, Code = "P0460", Description = "Fuel Level Sensor Circuit Malfunction", SensorTypeId = 2 },
                new() { Id = 7, Code = "P0461", Description = "Fuel Level Sensor Circuit Range/Performance", SensorTypeId = 2 },
                new() { Id = 8, Code = "P0462", Description = "Fuel Level Sensor Circuit Low Input", SensorTypeId = 2 },
                new() { Id = 9, Code = "P0463", Description = "Fuel Level Sensor Circuit High Input", SensorTypeId = 2 },
                new() { Id = 10, Code = "P0464", Description = "Fuel Level Sensor Circuit Intermittent", SensorTypeId = 2 },

                // Coolant Temperature Sensor
                new() { Id = 11, Code = "P0125", Description = "Insufficient Coolant Temperature for Closed Loop Fuel Control", SensorTypeId = 3 },
                new() { Id = 12, Code = "P0115", Description = "Coolant Temperature Sensor Circuit Malfunction", SensorTypeId = 3 },
                new() { Id = 13, Code = "P0117", Description = "Coolant Temperature Sensor Circuit Low Input", SensorTypeId = 3 },
                new() { Id = 14, Code = "P0118", Description = "Coolant Temperature Sensor Circuit High Input", SensorTypeId = 3 },
                new() { Id = 15, Code = "P0128", Description = "Coolant Temperature Below Thermostat Regulating Temperature", SensorTypeId = 3 },

                // Oil Temperature Sensor
                new() { Id = 16, Code = "P0520", Description = "Engine Oil Pressure Sensor Circuit Malfunction", SensorTypeId = 4 },
                new() { Id = 17, Code = "P0521", Description = "Engine Oil Pressure Sensor Range/Performance", SensorTypeId = 4 },
                new() { Id = 18, Code = "P0522", Description = "Engine Oil Pressure Sensor Low Input", SensorTypeId = 4 },
                new() { Id = 19, Code = "P0523", Description = "Engine Oil Pressure Sensor High Input", SensorTypeId = 4 },
                new() { Id = 20, Code = "P0524", Description = "Engine Oil Pressure Low", SensorTypeId = 4 },

                // Oil Pressure Sensor
                new() { Id = 21, Code = "P0520", Description = "Engine Oil Pressure Sensor Circuit Malfunction", SensorTypeId = 5 },
                new() { Id = 22, Code = "P0521", Description = "Engine Oil Pressure Sensor Range/Performance", SensorTypeId = 5 },
                new() { Id = 23, Code = "P0522", Description = "Engine Oil Pressure Sensor Low Input", SensorTypeId = 5 },
                new() { Id = 24, Code = "P0523", Description = "Engine Oil Pressure Sensor High Input", SensorTypeId = 5 },
                new() { Id = 25, Code = "P0524", Description = "Engine Oil Pressure Low", SensorTypeId = 5 },

                // Oxygen Sensor
                new() { Id = 26, Code = "P0130", Description = "Oxygen Sensor Circuit Malfunction (Bank 1, Sensor 1)", SensorTypeId = 6 },
                new() { Id = 27, Code = "P0133", Description = "Oxygen Sensor Circuit Slow Response (Bank 1, Sensor 1)", SensorTypeId = 6 },
                new() { Id = 28, Code = "P0140", Description = "Oxygen Sensor Circuit No Activity Detected (Bank 1, Sensor 2)", SensorTypeId = 6 },
                new() { Id = 29, Code = "P0141", Description = "Oxygen Sensor Heater Circuit Malfunction (Bank 1, Sensor 2)", SensorTypeId = 6 },
                new() { Id = 30, Code = "P0135", Description = "Oxygen Sensor Heater Circuit Malfunction (Bank 1, Sensor 1)", SensorTypeId = 6 },

                // Throttle Position Sensor
                new() { Id = 31, Code = "P0120", Description = "Throttle Position Sensor Circuit Malfunction", SensorTypeId = 7 },
                new() { Id = 32, Code = "P0121", Description = "Throttle Position Sensor Circuit Range/Performance", SensorTypeId = 7 },
                new() { Id = 33, Code = "P0122", Description = "Throttle Position Sensor Circuit Low Input", SensorTypeId = 7 },
                new() { Id = 34, Code = "P0123", Description = "Throttle Position Sensor Circuit High Input", SensorTypeId = 7 },
                new() { Id = 35, Code = "P0220", Description = "Throttle Position Sensor 2 Circuit Malfunction", SensorTypeId = 7 },

                // Mass Air Flow Sensor
                new() { Id = 36, Code = "P0100", Description = "Mass or Volume Air Flow Sensor Circuit Malfunction", SensorTypeId = 8 },
                new() { Id = 37, Code = "P0101", Description = "Mass or Volume Air Flow Sensor Range/Performance Problem", SensorTypeId = 8 },
                new() { Id = 38, Code = "P0102", Description = "Mass or Volume Air Flow Sensor Low Input", SensorTypeId = 8 },
                new() { Id = 39, Code = "P0103", Description = "Mass or Volume Air Flow Sensor High Input", SensorTypeId = 8 },
                new() { Id = 40, Code = "P0104", Description = "Mass or Volume Air Flow Sensor Intermittent", SensorTypeId = 8 },

                // Manifold Absolute Pressure Sensor
                new() { Id = 41, Code = "P0106", Description = "Manifold Absolute Pressure/Barometric Pressure Circuit Range/Performance", SensorTypeId = 9 },
                new() { Id = 42, Code = "P0107", Description = "Manifold Absolute Pressure/Barometric Pressure Circuit Low Input", SensorTypeId = 9 },
                new() { Id = 43, Code = "P0108", Description = "Manifold Absolute Pressure/Barometric Pressure Circuit High Input", SensorTypeId = 9 },
                new() { Id = 44, Code = "P0109", Description = "Manifold Absolute Pressure/Barometric Pressure Circuit Intermittent", SensorTypeId = 9 },
                new() { Id = 45, Code = "P0110", Description = "Manifold Absolute Pressure Sensor Circuit Malfunction", SensorTypeId = 9 },

                // Crankshaft Position Sensor
                new() { Id = 46, Code = "P0335", Description = "Crankshaft Position Sensor A Circuit Malfunction", SensorTypeId = 10 },
                new() { Id = 47, Code = "P0336", Description = "Crankshaft Position Sensor A Circuit Range/Performance", SensorTypeId = 10 },
                new() { Id = 48, Code = "P0337", Description = "Crankshaft Position Sensor A Circuit Low Input", SensorTypeId = 10 },
                new() { Id = 49, Code = "P0338", Description = "Crankshaft Position Sensor A Circuit High Input", SensorTypeId = 10 },
                new() { Id = 50, Code = "P0340", Description = "Camshaft Position Sensor Circuit Malfunction", SensorTypeId = 10 },

                // Camshaft Position Sensor
                new() { Id = 51, Code = "P0340", Description = "Camshaft Position Sensor Circuit Malfunction", SensorTypeId = 11 },
                new() { Id = 52, Code = "P0341", Description = "Camshaft Position Sensor Circuit Range/Performance", SensorTypeId = 11 },
                new() { Id = 53, Code = "P0342", Description = "Camshaft Position Sensor Circuit Low Input", SensorTypeId = 11 },
                new() { Id = 54, Code = "P0343", Description = "Camshaft Position Sensor Circuit High Input", SensorTypeId = 11 },
                new() { Id = 55, Code = "P0365", Description = "Camshaft Position Sensor B Circuit Malfunction", SensorTypeId = 11 },

                // Knock Sensor
                new() { Id = 56, Code = "P0325", Description = "Knock Sensor Circuit Malfunction (Bank 1)", SensorTypeId = 12 },
                new() { Id = 57, Code = "P0326", Description = "Knock Sensor Circuit Range/Performance (Bank 1)", SensorTypeId = 12 },
                new() { Id = 58, Code = "P0327", Description = "Knock Sensor Circuit Low Input (Bank 1)", SensorTypeId = 12 },
                new() { Id = 59, Code = "P0328", Description = "Knock Sensor Circuit High Input (Bank 1)", SensorTypeId = 12 },
                new() { Id = 60, Code = "P0330", Description = "Knock Sensor Circuit Malfunction (Bank 2)", SensorTypeId = 12 },

                // Intake Air Temperature Sensor
                new() { Id = 61, Code = "P0110", Description = "Intake Air Temperature Sensor Circuit Malfunction", SensorTypeId = 13 },
                new() { Id = 62, Code = "P0111", Description = "Intake Air Temperature Sensor Range/Performance Problem", SensorTypeId = 13 },
                new() { Id = 63, Code = "P0112", Description = "Intake Air Temperature Sensor Circuit Low Input", SensorTypeId = 13 },
                new() { Id = 64, Code = "P0113", Description = "Intake Air Temperature Sensor Circuit High Input", SensorTypeId = 13 },
                new() { Id = 65, Code = "P0114", Description = "Intake Air Temperature Sensor Circuit Intermittent", SensorTypeId = 13 },

                // Steering Angle Sensor
                new() { Id = 66, Code = "C0030", Description = "Steering Angle Sensor Circuit Malfunction", SensorTypeId = 14 },
                new() { Id = 67, Code = "C0031", Description = "Steering Angle Sensor Circuit Range/Performance", SensorTypeId = 14 },
                new() { Id = 68, Code = "C0032", Description = "Steering Angle Sensor Circuit Low Input", SensorTypeId = 14 },
                new() { Id = 69, Code = "C0033", Description = "Steering Angle Sensor Circuit High Input", SensorTypeId = 14 },
                new() { Id = 70, Code = "C0040", Description = "Steering Angle Sensor Circuit Malfunction", SensorTypeId = 14 },

                // Rain Sensor
                new() { Id = 71, Code = "B1481", Description = "Rain Sensor Circuit Malfunction", SensorTypeId = 15 },
                new() { Id = 72, Code = "B1482", Description = "Rain Sensor Circuit Range/Performance", SensorTypeId = 15 },
                new() { Id = 73, Code = "B1483", Description = "Rain Sensor Circuit Low Input", SensorTypeId = 15 },
                new() { Id = 74, Code = "B1484", Description = "Rain Sensor Circuit High Input", SensorTypeId = 15 },
                new() { Id = 75, Code = "B1490", Description = "Rain Sensor Malfunction", SensorTypeId = 15 },

                // Light Sensor
                new() { Id = 76, Code = "B0110", Description = "Light Sensor Circuit Malfunction", SensorTypeId = 16 },
                new() { Id = 77, Code = "B0111", Description = "Light Sensor Circuit Range/Performance", SensorTypeId = 16 },
                new() { Id = 78, Code = "B0112", Description = "Light Sensor Circuit Low Input", SensorTypeId = 16 },
                new() { Id = 79, Code = "B0113", Description = "Light Sensor Circuit High Input", SensorTypeId = 16 },
                new() { Id = 80, Code = "B0114", Description = "Light Sensor Circuit Intermittent", SensorTypeId = 16 },

                // Tire Pressure Sensor
                new() { Id = 81, Code = "C1100", Description = "Tire Pressure Sensor Circuit Malfunction", SensorTypeId = 17 },
                new() { Id = 82, Code = "C1101", Description = "Tire Pressure Sensor Circuit Range/Performance", SensorTypeId = 17 },
                new() { Id = 83, Code = "C1102", Description = "Tire Pressure Sensor Low Input", SensorTypeId = 17 },
                new() { Id = 84, Code = "C1103", Description = "Tire Pressure Sensor High Input", SensorTypeId = 17 },
                new() { Id = 85, Code = "C1104", Description = "Tire Pressure Sensor Malfunction", SensorTypeId = 17 },
            };
            modelBuilder.Entity<Error>().HasData(errors);

            var errorLogs = new List<ErrorLog>
            {
                new() { Id = 1, TimeStamp = DateTime.Now.AddMinutes(-30), ErrorId = 1, SensorId = 1 },
                new() { Id = 2, TimeStamp = DateTime.Now.AddMinutes(-25), ErrorId = 6, SensorId = 2 },
                new() { Id = 3, TimeStamp = DateTime.Now.AddMinutes(-20), ErrorId = 7, SensorId = 2 },
                new() { Id = 4, TimeStamp = DateTime.Now.AddMinutes(-20), ErrorId = 8, SensorId = 2 },
                new() { Id = 5, TimeStamp = DateTime.Now.AddMinutes(-20), ErrorId = 12, SensorId = 3 }
            };
            modelBuilder.Entity<ErrorLog>().HasData(errorLogs);
        }
    }
}
