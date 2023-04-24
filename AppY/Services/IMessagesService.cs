using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppY.Services
{
    public interface IMessagesService
    {
        bool IsConnected { get; }
        IModel GetNewChannel();
        void SubscribeForStream(IModel channel, string streamName, EventHandler<BasicDeliverEventArgs> OnMessageArrived);
        void PublishToStream(IModel channel, string streamName, byte[] body);
    }
}
