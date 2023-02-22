using Azure.Messaging.ServiceBus;

namespace AkvelonTest
{
    public class MessageBroker : IMessageBroker
    {
        private readonly ServiceBusSender _sender;

        public MessageBroker(ServiceBusSender sender)
        {
            _sender = sender;
        }

        public async Task SendMessageAsync(string message)
        {
            await _sender.SendMessageAsync(new ServiceBusMessage(message));
        }
    }
}
