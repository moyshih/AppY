namespace AppY.Models
{
    public abstract class Vehicle
    {
        public Vehicle(string type, string color, string plateNumber, DateTime timestamp)
        {
            Type = type;
            Color = color;
            PlateNumber = plateNumber;
            Timestamp = timestamp;
        }

        public string Type { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
