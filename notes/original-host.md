# The Host where we moved to Docker Compose, If We Need it

```csharp
var builder = DistributedApplication.CreateBuilder(args);


var mappingPath = Path.Combine("..", "wiremock-mappings");
if(!Directory.Exists(mappingPath))
{
    throw new Exception("Can't Find Mapping File");
}
//var softwareApiMocks = builder.AddWireMock("software")
//    .WithMappingsPath(mappingPath)
//    .WithReadStaticMappings()
//    .WithWatchStaticMappings();
var softwareCenter = builder.AddExternalService("software", "http://localhost:1337")
    .WithHttpHealthCheck("/openapi/v1.json");
var p1 = builder.AddParameter("use-fake-identity", false);
var externalDatabase = builder.AddConnectionString("shared-dev-database", "some remote connection string");

// this will start a docker container running postgres:17.5
//       .WithImage("postgres:17.5-bullseye")
//var postgres = builder.AddPostgres("postgres")
//       .WithPgAdmin();

//// I'm going to need a database in that called "issues"
//var issuesDb = postgres.AddDatabase("issues");


builder.AddProject<Projects.HelpDesk_Api>("helpdesk-api")
    .WithExternalHttpEndpoints()
    .WithEnvironment("USE_FAKE_IDENTITY", p1)
    .WithReference(externalDatabase)
    .WithReference(softwareCenter)
    .WaitFor(softwareCenter);
    

builder.Build().Run();
```