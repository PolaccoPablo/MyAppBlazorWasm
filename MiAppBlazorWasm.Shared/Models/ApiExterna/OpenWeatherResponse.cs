using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiAppBlazorWasm.Model.Models.ApiExterna
{
    public class OpenWeatherResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public MainWeather Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();

        [JsonPropertyName("wind")]
        public WindInfo Wind { get; set; } = new();

        [JsonPropertyName("sys")]
        public SysInfo Sys { get; set; } = new();
    }
}
