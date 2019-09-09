using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Toggles
{
    public class RabbitRemoteSync : ITogglesRemoteSync
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly EventingBasicConsumer _consumer;
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;

        public RabbitRemoteSync()
        {
            _factory = new ConnectionFactory();
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("toggle-updates", "fanout");
            _queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(_queueName, "toggle-updates", "");

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var str = Encoding.UTF8.GetString(ea.Body);
                var toggle = JsonConvert.DeserializeObject<Toggle>(str);
                Trace.WriteLine($"Received toggle {toggle.Name}, enabled {toggle.Enabled}");

                ToggleUpdate?.Invoke(toggle);
            };
        }

        public void StartConsuming() => _channel.BasicConsume(_queueName, true, _consumer);

        public event ToggleUpdateEventHandler ToggleUpdate;
    }
}