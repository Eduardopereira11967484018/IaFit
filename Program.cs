using IaFit.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<INutritionService, NutritionService>();
builder.Services.AddLogging(logging => logging.AddConsole()); // Adiciona logs ao console

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapFallback(context =>
{
    context.Response.StatusCode = 404;
    return context.Response.WriteAsync("Rota não encontrada.");
});

app.Run("http://localhost:5000");