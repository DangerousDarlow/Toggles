using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace TogglesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TogglesController : ControllerBase
    {
        private ConnectionFactory _factory;
        private IConnection _connection;

        public TogglesController()
        {
            _factory = new ConnectionFactory();
            _connection = _factory.CreateConnection();
        }

        [HttpPost("[action]")]
        public void SetToggle(Toggle toggle)
        {
        }

        public class Toggle
        {
            public string Name { get; set; }

            public bool Enabled { get; set; }
        }
    }
}