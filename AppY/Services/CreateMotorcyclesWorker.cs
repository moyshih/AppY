using AppY.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Drawing;
using System.Text;

namespace AppY.Services
{
    public class CreateMotorcyclesWorker : BackgroundService
    {
        private readonly ConfigData _configData;
        private readonly IMessagesService _messagesService;
        private IModel _channel;

        public CreateMotorcyclesWorker(IMessagesService messagesService, ConfigData configData)
        {
            _messagesService = messagesService;
            _configData = configData;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Use using for disposing the channel at the end
            using (var channel = _messagesService.GetNewChannel())
            {
                _channel = channel;
                IList<Vehicle> vehicles = new List<Vehicle>();

                var rnd = new Random();
                KnownColor[] colorNames = (KnownColor[])Enum.GetValues(typeof(KnownColor));

                // Create and publish vehicle every {AppConfig.PublishRateInMiliseconds}
                while (!stoppingToken.IsCancellationRequested)
                {
                    Vehicle vehicle = getRandomVehicle(rnd, colorNames);
                    vehicles.Add(vehicle);
                    PublishVehicle(vehicle);

                    await Task.Delay(_configData.PublishRateInMilliseconds, stoppingToken);
                }
            }
        }

        private void PublishVehicle(Vehicle vehicle)
        {
            // Serialize the car object to JSON
            var carJson = JsonConvert.SerializeObject(vehicle);
            var carBytes = Encoding.UTF8.GetBytes(carJson);

            _messagesService.PublishToStream(_channel, _configData.PublishQueueName, carBytes);
        }

        private Vehicle getRandomVehicle(Random rnd, IEnumerable<KnownColor> colorNames)
        {
            string type = _configData.VehicleTypes.ElementAt(rnd.Next(_configData.VehicleTypes.Count()));
            string color = colorNames.ElementAt(rnd.Next(colorNames.Count())).ToString();
            string plateNumber = rnd.Next(1000000, 99999999).ToString();
            DateTime timestamp = DateTime.Now;

            Vehicle vehicle = new Motorcycle(type, color, plateNumber, timestamp);

            return vehicle;
        }
    }
}