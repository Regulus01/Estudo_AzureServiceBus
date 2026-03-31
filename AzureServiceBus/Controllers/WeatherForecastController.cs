using AzureServiceBus.Infrastructure.MessageBus;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AzureServiceBus.Controllers
{
    [ApiController]
    [Route("WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBusService _bus;

        public WeatherForecastController(IBusService bus)
        {
            _bus = bus;
        }

        [HttpPost()]
        public async Task<IActionResult> PostAsync()
        {
            var message = new
            {
                id = 1,
                nome = "jose"
            };

            await _bus.SendMessageAsync("topic.1", message);
           
            return Created();
        }
    }
}
