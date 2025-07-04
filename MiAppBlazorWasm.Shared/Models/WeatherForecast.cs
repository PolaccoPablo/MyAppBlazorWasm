using System.ComponentModel.DataAnnotations;

namespace MiAppBlazorWasm.Shared.Models;

public class WeatherForecast
{
    [Key]
    public int Id { get; set; }
    public DateOnly Date { get; set; }

    [Range(-50, 60, ErrorMessage = "La temperatura debe estar entre -50 y 60 grados Celsius")]
    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [Required(ErrorMessage = "El resumen es requerido")]
    [StringLength(50, ErrorMessage = "El resumen no puede exceder 50 caracteres")]
    public string? Summary { get; set; }

    public string? Description { get; set; }

    public WeatherCondition Condition { get; set; }

    public string? Location { get; set; }
}