using System.Text.Json;
using System.Text.Json.Serialization;
using HelpDesk.Api;
using HelpDesk.Api.Employee.Models;
using HelpDesk.Api.HttpClients;
using HelpDesk.Api.Services;
using JasperFx.Events.Projections;
using Marten;
using Wolverine;
using Wolverine.Marten;


var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults(); // this adds the resiliency handers - https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience?tabs=dotnet-cli

builder.UseWolverine(options =>
{
    options.Policies.UseDurableLocalQueues();
    options.Durability.Mode = DurabilityMode.Solo; // Default is just slower to start
});

builder.Services.AddHttpClient<SoftwareCenterApiClient>(client =>
{
    var serviceAddress = builder.Configuration["services:software:http:0"] ?? throw new Exception("No SoftwareCenter is configured");
    client.BaseAddress = new Uri(serviceAddress); 
});
builder.Services.AddHttpClient<VipApiClient>(client =>
{
    //services__vip-api__http__0
    var serviceAddress = builder.Configuration["services:vip-api:http:0"] ?? throw new Exception("No SoftwareCenter is configured");
    client.BaseAddress = new Uri(serviceAddress); 
});
builder.Services.AddScoped<ILookupSoftwareFromTheSoftwareApi>(b =>
{
    // this is a provider factory 
    return b.GetRequiredService<SoftwareCenterApiClient>();
});

builder.Services.AddAuthentication().AddJwtBearer(); // Authentication - Finding out who someone is
builder.Services.AddControllers() // this is optional, we don't have to use controllers. 
    .AddJsonOptions(options =>
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

builder.Services.AddHttpContextAccessor(); // In a service we create, we can access the HTTP context.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System); // this is for the "clock"

var useFakeIdentity = builder.Environment.IsDevelopment();
// var useFakeIdentity = builder.Configuration["USE_FAKE_IDENTITY"] == "true";
if (useFakeIdentity)
{
    builder.Services.AddScoped<IManageUserIdentity, DevelopmentOnlyUserIdentityFakeProvider>();

}
else
{
    builder.Services.AddScoped<IManageUserIdentity, UserIdentityManager>();
}
// builder.Services.AddHostedService<IssueProcessor>();

// your database stuff will vary. You might use SQL Server, DB2, MongoDb, whatever.
// The IDocumentSession that we can use in our controllers, services, etc.
// Had builder.Services.AddNpgsqlDataSource("issues"); That doesn't work.
builder.AddNpgsqlDataSource("issues");
builder.Services.AddMarten(opts =>
{
    //opts.Connection(connectionString); // Come back to this.

    // If you are using "live" projections, you don't have to do this.
    // That is when you do session.Events.AggregateStreamAsync<EmployeeIssueReadModel>(id);

    opts.Projections.Add<EmployeeIssueProjection>(ProjectionLifecycle.Inline); 
    // Inline is POWERFUL but can be costly -
    // this means that the stored document is updated in the same transaction as the events being
    // published.
    // Async means that we are going to use eventual consistency. 
    // There will be a worker process (background worker) that periodically applies the events
    // to the projection. So it won't be in the same transaction or immediate.
    // That work can be spread to other nodes (processes) to scale this.
})
    
    .IntegrateWithWolverine()
    .UseNpgsqlDataSource()
    .UseLightweightSessions();

builder.Services.AddScoped<IssueCreateModelValidator>();
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



if (app.Environment.IsDevelopment())
{
    await app.SeedUsers();
}

app.Run();

public partial class Program; // tomorrow, .net 10 comes out! you don't have to do this any more!