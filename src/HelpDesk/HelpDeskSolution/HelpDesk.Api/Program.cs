using Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();


var connectionString = builder.Configuration.GetConnectionString("issues") ?? throw new Exception("No Connection String Found In Environment");

builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString);
}).UseLightweightSessions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");

    });
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
}

app.UseAuthentication(); 
app.UseAuthorization();

app.Run();

public partial class Program;