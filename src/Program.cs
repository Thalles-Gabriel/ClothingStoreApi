using System.Text.Json.Serialization;
using ClothingStore.API.Filters.Actions;
using ClothingStore.API.Helpers;
using ClothingStore.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new LoggingActionFilter());
}).AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddRepositories();
builder.Services.AddFilters();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
