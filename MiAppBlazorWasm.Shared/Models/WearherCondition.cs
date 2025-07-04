namespace MiAppBlazorWasm.Shared.Models;

public enum WeatherCondition
{ //Esto despues podemos pasarlo a una entidad ID descripcion y icono, para que sea mas facil de mantener y traducir
    Sunny = 1,
    Cloudy = 2,
    Rainy = 3,
    Snowy = 4,
    Stormy = 5,
    Foggy = 6,
    Windy = 7
}

public static class WeatherConditionExtensions
{
    public static string GetDescription(this WeatherCondition condition)
    {
        return condition switch
        {
            WeatherCondition.Sunny => "Soleado",
            WeatherCondition.Cloudy => "Nublado",
            WeatherCondition.Rainy => "Lluvioso",
            WeatherCondition.Snowy => "Nevando",
            WeatherCondition.Stormy => "Tormentoso",
            WeatherCondition.Foggy => "Neblinoso",
            WeatherCondition.Windy => "Ventoso",
            _ => "Desconocido"
        };
    }

    public static string GetIcon(this WeatherCondition condition)
    {
        return condition switch
        {
            WeatherCondition.Sunny => "☀️",
            WeatherCondition.Cloudy => "☁️",
            WeatherCondition.Rainy => "🌧️",
            WeatherCondition.Snowy => "❄️",
            WeatherCondition.Stormy => "⛈️",
            WeatherCondition.Foggy => "🌫️",
            WeatherCondition.Windy => "💨",
            _ => "❓"
        };
    }
}