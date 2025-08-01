using System.ComponentModel.DataAnnotations;

namespace MiAppBlazorWasm.Model.Dtos
{

    public class CreateWeatherForecastRequest
    {
        [Required(ErrorMessage = "La fecha es requerida")]
        public DateOnly Date { get; set; }

        [Range(-50, 60, ErrorMessage = "La temperatura debe estar entre -50 y 60 grados Celsius")]
        public int TemperatureC { get; set; }

        [Required(ErrorMessage = "El resumen es requerido")]
        [StringLength(50, ErrorMessage = "El resumen no puede exceder 50 caracteres")]
        public string Summary { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ConditionId { get; set; }
    }

    public class UpdateWeatherForecastRequest : CreateWeatherForecastRequest
    {
        [Required]
        public int Id { get; set; }
    }

    public class WeatherForecastResponse
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public string ConditionIcon { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public static class ApiResponse
    {
        public static ApiResponse<T> SuccessResult<T>(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResult<T>(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}