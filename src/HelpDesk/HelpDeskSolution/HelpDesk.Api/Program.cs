using System.Text.Json.Serialization;
using HelpDesk.Api.Employee.BackgroundWorker;
using HelpDesk.Api.Services;
using Marten;


var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddAuthentication().AddJwtBearer(); // Authentication - Finding out who someone is
builder.Services.AddControllers() // this is optional, we don't have to use controllers. 
    .AddJsonOptions(options =>
    {
        // Allows you to send enum values as string and serializes them as strings.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // in Json, if a property doesn't exist, it's the same as returning it with a null value
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    });

builder.Services.AddHttpContextAccessor(); // In a service we create, we can access the HTTP context.
builder.Services.AddOpenApi();
builder.Services.AddSingleton(_ => TimeProvider.System); // this is for the "clock"
builder.Services.AddScoped<IManageUserIdentity,UserIdentityManager>();
// builder.Services.AddHostedService<IssueProcessor>();

// your database stuff will vary. You might use SQL Server, DB2, MongoDb, whatever.
// The IDocumentSession that we can use in our controllers, services, etc.
builder.AddNpgsqlDataSource("issues");
var connectionString = builder.Configuration.GetConnectionString("issues") ?? throw new Exception("No Connection String Found In Environment");
Console.WriteLine(connectionString);
builder.Services.AddMarten(opts =>
{
    //opts.Connection(connectionString); // Come back to this.
   })
    .UseNpgsqlDataSource()
    .UseLightweightSessions();

// above this line is configuration of the services that make up our API
var app = builder.Build();
// after this, you can't change the services - but you do configure "middleware"

if (app.Environment.IsDevelopment()) // Environment Variable on your machine called ASPNETCORE_ENVIRONMENT
{
    app.MapOpenApi(); // makes it so we are generating an OAS 3.0 Specification ("swagger doc")
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");

    });
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
}

app.UseAuthentication();  // use authentication middleware - look at incoming requests

app.UseAuthorization(); // setting policies about who can do what.
// go look at all my classes, look for the [HttpX] attributes, and create the routing table.
app.MapControllers();

app.Run();

public partial class Program; // tomorrow, .net 10 comes out! you don't have to do this any more!