namespace AppY.Services
{
    /// <summary>
    /// App config data with default values for case there isn't an appsettings file
    /// </summary>
    public class ConfigData
    {
        public IList<string> VehicleTypes { get; set; } = new List<string> {
            "Standard",
            "Cruiser",
            "Sport",
            "Touring",
            "DualSport",
            "DirtBikes",
            "Scooters",
            "Electric"
    };
        public string HostName { get; set; } = "localhost";
        public string ConsumeQueueName { get; set; } = "carQueue";
        public string PublishQueueName { get; set; } = "motorcycleQueue";
        public string PublishUiQueueName { get; set; } = "carUiQueue";
        public int PublishRateInMilliseconds { get; set; } = 1000;
    }
}
