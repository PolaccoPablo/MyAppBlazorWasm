using MiAppBlazorWasm.Shared.Models;
using MiAppBlazorWasm.Shared.Dtos;

using System.Net.Http.Json;
using System.Text.Json;

namespace MiAppBlazorWasm.Client.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }


    public async Task<ApiResponse<List<WeatherForecastResponse>>?> GetWeatherForecastsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/weather");
            var jsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<List<WeatherForecastResponse>>>(jsonString, _jsonOptions);
            }
            else
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResponse<List<WeatherForecastResponse>>>(jsonString, _jsonOptions);
                return errorResponse;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener pronósticos: {ex.Message}");
            return ApiResponse.ErrorResult<List<WeatherForecastResponse>>(
                "Error de conexión", new List<string> { ex.Message });
        }
    }


    public async Task<ApiResponse<WeatherForecastResponse>?> GetWeatherForecastAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/weather/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ApiResponse<WeatherForecastResponse>>(jsonString, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener pronóstico {id}: {ex.Message}");
            return ApiResponse.ErrorResult<WeatherForecastResponse>(
                "Error de conexión", new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<WeatherForecastResponse>?> CreateWeatherForecastAsync(CreateWeatherForecastRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/weather", request);
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ApiResponse<WeatherForecastResponse>>(jsonString, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear pronóstico: {ex.Message}");
            return ApiResponse.ErrorResult<WeatherForecastResponse>(
                "Error de conexión", new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<WeatherForecastResponse>?> UpdateWeatherForecastAsync(int id, UpdateWeatherForecastRequest request)
    {
        try
        {
            request.Id = id;
            var response = await _httpClient.PutAsJsonAsync($"api/weather/{id}", request);
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ApiResponse<WeatherForecastResponse>>(jsonString, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar pronóstico: {ex.Message}");
            return ApiResponse.ErrorResult<WeatherForecastResponse>(
                "Error de conexión", new List<string> { ex.Message });
        }
    }

    public async Task<ApiResponse<bool>?> DeleteWeatherForecastAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/weather/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ApiResponse<bool>>(jsonString, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar pronóstico: {ex.Message}");
            return ApiResponse.ErrorResult<bool>(
                "Error de conexión", new List<string> { ex.Message });
        }
    }
}



