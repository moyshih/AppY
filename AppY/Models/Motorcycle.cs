namespace AppY.Models
{
    internal class Motorcycle : Vehicle
    {
        public Motorcycle(string type, string color, string plateNumber, DateTime timestamp)
            : base(type, color, plateNumber, timestamp)
        {
        }
    }
}
