using System.Text.Json.Serialization;

namespace MiAppBlazorWasm.Model.Models.ApiExterna
{
    public class SysInfo
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }
}
