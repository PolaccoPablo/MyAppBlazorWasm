//using MiAppBlazorWasm.Api.Service;
//using MiAppBlazorWasm.Model.Models.ApiExterna;
//using Microsoft.AspNetCore.Mvc;

//namespace MiAppBlazorWasm.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class WeatherController : ControllerBase
//    {
//        private readonly IWeatherService _weatherService;
//        private readonly ILogger<WeatherController> _logger;

//        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
//        {
//            _weatherService = weatherService;
//            _logger = logger;
//        }

//        [HttpPost("current")]
//        public async Task<ActionResult<WeatherResponse>> GetCurrentWeather([FromBody] LocationRequest request)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                _logger.LogInformation("Obteniendo clima para: {City}", request.City);

//                var weather = await _weatherService.GetWeatherAsync(request);
//                return Ok(weather);
//            }
//            catch (HttpRequestException ex)
//            {
//                _logger.LogError(ex, "Error de conexión al obtener clima para {City}", request.City);
//                return StatusCode(503, new { message = "Servicio de clima no disponible temporalmente" });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener clima para {City}", request.City);
//                return StatusCode(500, new { message = "Error interno del servidor" });
//            }
//        }

//        [HttpGet("current/{city}")]
//        public async Task<ActionResult<WeatherResponse>> GetCurrentWeatherByCity(string city, [FromQuery] string? country = null)
//        {
//            try
//            {
//                var request = new LocationRequest
//                {
//                    City = city,
//                    Country = country
//                };

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                _logger.LogInformation("Obteniendo clima para: {City}", city);

//                var weather = await _weatherService.GetWeatherAsync(request);
//                return Ok(weather);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener clima para {City}", city);
//                return StatusCode(500, new { message = "Error interno del servidor" });
//            }
//        }
//    }
//}
