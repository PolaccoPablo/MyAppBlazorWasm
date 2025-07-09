using MiAppBlazorWasm.Model.Models.ApiExterna;
using System.Text.Json;

namespace MiAppBlazorWasm.Api.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["OpenWeatherMap:ApiKey"] ??
                     throw new InvalidOperationException("API Key de OpenWeatherMap no configurada");
        }

        public async Task<WeatherResponse> GetWeatherAsync(LocationRequest location)
        {
            try
            {
                var query = string.IsNullOrEmpty(location.Country)
                    ? location.City
                    : $"{location.City},{location.Country}";

                var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(query)}&appid={_apiKey}&units=metric&lang=es";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al obtener datos del clima: {response.StatusCode} - {errorContent}");
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(jsonContent);

                if (weatherData == null)
                {
                    throw new InvalidOperationException("No se pudieron deserializar los datos del clima");
                }

                return new WeatherResponse
                {
                    Location = $"{weatherData.Name}, {weatherData.Sys.Country}",
                    Temperature = Math.Round(weatherData.Main.Temp, 1),
                    FeelsLike = Math.Round(weatherData.Main.FeelsLike, 1),
                    Description = weatherData.Weather.FirstOrDefault()?.Description ?? "No disponible",
                    Humidity = weatherData.Main.Humidity,
                    WindSpeed = Math.Round(weatherData.Wind.Speed * 3.6, 1), // Convertir m/s a km/h
                    Icon = weatherData.Weather.FirstOrDefault()?.Icon ?? "",
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión con la API del clima: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error al procesar la respuesta de la API: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}", ex);
            }
        }
    }
}

