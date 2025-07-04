using MiAppBlazorWasm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppBlazorWasm.Server.Context
{
    public class MyAppDBcontext: DbContext
    {
        public MyAppDBcontext(DbContextOptions<MyAppDBcontext> options) : base(options)
        {
        }
        // Define DbSets for your entities here
        // public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
     }

}
