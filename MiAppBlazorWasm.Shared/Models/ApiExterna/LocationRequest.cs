using System.ComponentModel.DataAnnotations;

namespace MiAppBlazorWasm.Model.Models.ApiExterna
{
    public class LocationRequest
    {
        [Required(ErrorMessage = "La ciudad es requerida")]
        public string City { get; set; } = string.Empty;

        public string? Country { get; set; }
    }


}
