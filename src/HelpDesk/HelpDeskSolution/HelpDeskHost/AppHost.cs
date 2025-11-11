var builder = DistributedApplication.CreateBuilder(args);


var mappingPath = Path.Combine("..", "wiremock-mappings");
if(!Directory.Exists(mappingPath))
{
    throw new Exception("Can't Find Mapping File");
}
var softwareApiMocks = builder.AddWireMock("software")
    .WithMappingsPath(mappingPath)
    .WithReadStaticMappings()
    .WithWatchStaticMappings();

// this will start a docker container running postgres:17.5
//var postgres = builder.AddPostgres("postgres")
//       .WithImage("postgres:17.5-bullseye")
//       .WithPgAdmin();

//// I'm going to need a database in that called "issues"
//var issuesDb = postgres.AddDatabase("issues");


builder.AddProject<Projects.HelpDesk_Api>("helpdesk-api")
    .WithExternalHttpEndpoints()
    .WithReference(softwareApiMocks);
    

builder.Build().Run();
