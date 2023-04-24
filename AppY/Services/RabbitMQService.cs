using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppY.Services
{
    public class RabbitMQService : IMessagesService, IDisposable
    {
        private readonly ConfigData _configData;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        public bool IsConnected { get; } = false;

        public RabbitMQService(ConfigData configData)
        {
            _configData = configData;

            try
            {
                _factory = new ConnectionFactory() { HostName = _configData.HostName };
                _connection = _factory.CreateConnection();
                IsConnected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't establish connection to the RabbitMQ server", e);
            }
        }

        public IModel GetNewChannel()
        {
            return _connection.CreateModel();
        }

        public void SubscribeForStream(IModel channel, string streamName, EventHandler<BasicDeliverEventArgs> OnMessageArrived)
        {
            channel.QueueDeclare(
                queue: streamName,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnMessageArrived;

            channel.BasicConsume(
                queue: streamName,
                autoAck: false,
                consumer: consumer);
        }

        public void PublishToStream(IModel channel, string streamName, byte[] body)
        {
            channel.QueueDeclare(
                queue: streamName,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            channel.BasicPublish(
                exchange: "",
                routingKey: streamName,
                basicProperties: null,
                body: body);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}