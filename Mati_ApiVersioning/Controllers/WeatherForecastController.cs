using Microsoft.AspNetCore.Mvc;

namespace Mati_ApiVersioning.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [MapToApiVersion("1.0")]
        [HttpGet(Name = "GetWeatherForecast")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]
        [ProducesResponseType(404)]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}


namespace Mati_ApiVersioning.Controllers.v2
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0", Deprecated = false)]
    public class WeatherForecast2Controller : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecast2Controller> _logger;

        public WeatherForecast2Controller(ILogger<WeatherForecast2Controller> logger)
        {
            _logger = logger;
        }

        [MapToApiVersion("2.0")]
        [HttpGet(Name = "GetWeatherForecast2")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]
        [ProducesResponseType(404)]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}