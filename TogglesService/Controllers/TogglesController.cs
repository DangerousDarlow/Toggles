using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace TogglesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TogglesController : ControllerBase
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly ConnectionFactory _factory;

        public TogglesController()
        {
            _factory = new ConnectionFactory();
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("toggle-updates", "fanout");
        }

        [HttpPost("[action]")]
        public void SetToggle(Toggle toggle)
        {
            var json = JsonConvert.SerializeObject(toggle);
            var bytes = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish("toggle-updates", "", null, bytes);
        }

        public class Toggle
        {
            public string Name { get; set; }

            public bool Enabled { get; set; }
        }
    }
}