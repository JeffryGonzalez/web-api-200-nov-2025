using Demo.Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults(); // hey microsoft, give me the "starter kit" for OTEL, resiliency (retries, circuit breakers, etc)

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<HelpDeskApiClient>(client =>
{
    client.BaseAddress = new Uri(""); // get this from configuration, etc.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
