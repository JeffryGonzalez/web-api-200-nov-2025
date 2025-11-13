using System.Text.Json;
using System.Text.Json.Serialization;

using Marten;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.Marten;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.
builder.UseWolverine(options =>
{
    options.Policies.UseDurableLocalQueues();
    options.Durability.Mode = DurabilityMode.Solo; // Default is just slower to start
});
builder.Services.AddControllers() .AddJsonOptions(options =>
{
    // it has the Json CamelCase Naming Policy added by default.
    //options.JsonSerializerOptions.Converters.Add(JsonNamingPolicy)
    // Allows you to send enum values as string and serializes them as strings.
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    // in Json, if a property doesn't exist, it's the same as returning it with a null value
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    // one more option
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System); 
builder.AddNpgsqlDataSource("vips");
builder.Services.AddMarten(opts =>
{

}).IntegrateWithWolverine()
.UseNpgsqlDataSource()
.UseLightweightSessions();


var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Environment Variable on your machine called ASPNETCORE_ENVIRONMENT
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}




app.Run();
