using MiAppBlazorWasm.Api.Service;
using MiAppBlazorWasm.Model.Dtos;
using MiAppBlazorWasm.Model.Models;
using MiAppBlazorWasm.Model.Models.ApiExterna;
using Microsoft.AspNetCore.Mvc;

namespace MiAppBlazorWasm.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly List<WeatherForecast> _forecasts = new();
    private static int _nextId = 1;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;

        // Inicializar datos de ejemplo solo si no hay datos
        if (_forecasts.Count == 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                _forecasts.Add(new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    Condition = (WeatherCondition)Random.Shared.Next(1, 8),
                    Description = $"Pronóstico para el día {i}"
                });
            }
            _nextId = 6;
        }
    }

    // NUEVO ENDPOINT: Buscar clima por ciudad (API REAL)
    [HttpGet("current/{city}")]
    public async Task<ActionResult<ApiResponse<WeatherResponse>>> GetCurrentWeatherAsync(string city, [FromQuery] string? country = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(ApiResponse.ErrorResult<WeatherResponse>("El nombre de la ciudad es requerido"));

            var locationRequest = new LocationRequest
            {
                City = city.Trim(),
                Country = country?.Trim()
            };

            var weatherData = await _weatherService.GetWeatherAsync(locationRequest);

            return Ok(ApiResponse.SuccessResult(weatherData, "Datos del clima obtenidos exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult<WeatherResponse>(
                "Error al obtener datos del clima", new List<string> { ex.Message }));
        }
    }

    // NUEVO ENDPOINT: Buscar múltiples ciudades
    [HttpPost("current/batch")]
    public async Task<ActionResult<ApiResponse<List<WeatherResponse>>>> GetMultipleCitiesWeatherAsync([FromBody] List<LocationRequest> locations)
    {
        try
        {
            if (locations == null || !locations.Any())
                return BadRequest(ApiResponse.ErrorResult<List<WeatherResponse>>("Se requiere al menos una ubicación"));

            var weatherResults = new List<WeatherResponse>();
            var errors = new List<string>();

            foreach (var location in locations)
            {
                try
                {
                    var weatherData = await _weatherService.GetWeatherAsync(location);
                    weatherResults.Add(weatherData);
                }
                catch (Exception ex)
                {
                    errors.Add($"Error para {location.City}: {ex.Message}");
                }
            }

            if (weatherResults.Any())
            {
                var message = errors.Any()
                    ? $"Se obtuvieron {weatherResults.Count} de {locations.Count} ubicaciones. Algunos errores ocurrieron."
                    : "Todos los datos del clima obtenidos exitosamente";

                return Ok(ApiResponse.SuccessResult(weatherResults, message));
            }
            else
            {
                return StatusCode(500, ApiResponse.ErrorResult<List<WeatherResponse>>(
                    "No se pudieron obtener datos para ninguna ubicación", errors));
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult<List<WeatherResponse>>(
                "Error interno del servidor", new List<string> { ex.Message }));
        }
    }

    // ENDPOINTS EXISTENTES PARA CRUD DE PRONÓSTICOS LOCALES
    [HttpGet]
    public ActionResult<ApiResponse<List<WeatherForecastResponse>>> Get()
    {
        try
        {
            var response = _forecasts.Select((forecast, index) => new WeatherForecastResponse
            {
                Id = index + 1,
                Date = forecast.Date,
                TemperatureC = forecast.TemperatureC,
                TemperatureF = forecast.TemperatureF,
                Summary = forecast.Summary ?? "",
                Description = forecast.Description,
                ConditionName = forecast.Condition.GetDescription(),
                ConditionIcon = forecast.Condition.GetIcon(),
                CreatedAt = DateTime.Now.AddDays(-Random.Shared.Next(1, 30))
            }).ToList();

            return Ok(ApiResponse.SuccessResult(response, "Pronósticos obtenidos exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult<List<WeatherForecastResponse>>(
                "Error interno del servidor", new List<string> { ex.Message }));
        }
    }

    [HttpGet("{id}")]
    public ActionResult<ApiResponse<WeatherForecastResponse>> Get(int id)
    {
        if (id < 1 || id > _forecasts.Count)
            return NotFound(ApiResponse.ErrorResult<WeatherForecastResponse>("Pronóstico no encontrado"));

        var forecast = _forecasts[id - 1];
        var response = new WeatherForecastResponse
        {
            Id = id,
            Date = forecast.Date,
            TemperatureC = forecast.TemperatureC,
            TemperatureF = forecast.TemperatureF,
            Summary = forecast.Summary ?? "",
            Description = forecast.Description,
            ConditionName = forecast.Condition.GetDescription(),
            ConditionIcon = forecast.Condition.GetIcon(),
            CreatedAt = DateTime.Now.AddDays(-Random.Shared.Next(1, 30))
        };

        return Ok(ApiResponse.SuccessResult(response, "Pronóstico encontrado"));
    }

    [HttpPost]
    public ActionResult<ApiResponse<WeatherForecastResponse>> Post([FromBody] CreateWeatherForecastRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.ErrorResult<WeatherForecastResponse>(
                "Datos de entrada inválidos", errors));
        }

        var newForecast = new WeatherForecast
        {
            Date = request.Date,
            TemperatureC = request.TemperatureC,
            Summary = request.Summary,
            Description = request.Description,
            Condition = (WeatherCondition)request.ConditionId
        };

        _forecasts.Add(newForecast);

        var response = new WeatherForecastResponse
        {
            Id = _nextId++,
            Date = newForecast.Date,
            TemperatureC = newForecast.TemperatureC,
            TemperatureF = newForecast.TemperatureF,
            Summary = newForecast.Summary ?? "",
            Description = newForecast.Description,
            ConditionName = newForecast.Condition.GetDescription(),
            ConditionIcon = newForecast.Condition.GetIcon(),
            CreatedAt = DateTime.Now
        };

        return CreatedAtAction(nameof(Get), new { id = response.Id },
            ApiResponse.SuccessResult(response, "Pronóstico creado exitosamente"));
    }

    [HttpPut("{id}")]
    public ActionResult<ApiResponse<WeatherForecastResponse>> Put(int id, [FromBody] UpdateWeatherForecastRequest request)
    {
        if (id < 1 || id > _forecasts.Count)
            return NotFound(ApiResponse.ErrorResult<WeatherForecastResponse>("Pronóstico no encontrado"));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.ErrorResult<WeatherForecastResponse>(
                "Datos de entrada inválidos", errors));
        }

        var forecast = _forecasts[id - 1];
        forecast.Date = request.Date;
        forecast.TemperatureC = request.TemperatureC;
        forecast.Summary = request.Summary;
        forecast.Description = request.Description;
        forecast.Condition = (WeatherCondition)request.ConditionId;

        var response = new WeatherForecastResponse
        {
            Id = id,
            Date = forecast.Date,
            TemperatureC = forecast.TemperatureC,
            TemperatureF = forecast.TemperatureF,
            Summary = forecast.Summary ?? "",
            Description = forecast.Description,
            ConditionName = forecast.Condition.GetDescription(),
            ConditionIcon = forecast.Condition.GetIcon(),
            CreatedAt = DateTime.Now
        };

        return Ok(ApiResponse.SuccessResult(response, "Pronóstico actualizado exitosamente"));
    }

    [HttpDelete("{id}")]
    public ActionResult<ApiResponse<bool>> Delete(int id)
    {
        if (id < 1 || id > _forecasts.Count)
            return NotFound(ApiResponse.ErrorResult<bool>("Pronóstico no encontrado"));

        _forecasts.RemoveAt(id - 1);
        return Ok(ApiResponse.SuccessResult(true, "Pronóstico eliminado exitosamente"));
    }
}