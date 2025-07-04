using MiAppBlazorWasm.Server.Context;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//crar variable para la cadena de conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("Connection");

//fdgistrar servicio opara la conexion a la base de datos
builder.Services.AddDbContext<MyAppDBcontext>(options =>
    options.UseSqlServer(connectionString));

// Configurar CORS para el cliente Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7001") // Puerto del cliente Blazor
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar compresión de respuesta
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseResponseCompression();

app.UseCors("BlazorWasmPolicy");

// Servir archivos estáticos del cliente Blazor
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Configurar fallback para Blazor WebAssembly
app.MapFallbackToFile("index.html");

app.Run();