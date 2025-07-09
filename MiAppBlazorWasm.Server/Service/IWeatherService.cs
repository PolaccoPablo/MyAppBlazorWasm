using MiAppBlazorWasm.Model.Models.ApiExterna;

namespace MiAppBlazorWasm.Api.Service
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(LocationRequest location);
    }
}
