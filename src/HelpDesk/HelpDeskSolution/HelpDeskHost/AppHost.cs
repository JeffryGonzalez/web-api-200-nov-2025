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

var postgres = builder.AddPostgres("postgres")
    .WithImage("postgres:17.5-bullseye")
    .WithContainerName("local-postgres")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin(b =>
    {
        b.WithLifetime(ContainerLifetime.Persistent);
    });

var issuesDb = postgres.AddDatabase("issues");



builder.AddProject<Projects.HelpDesk_Api>("helpdesk-api")
    .WithExternalHttpEndpoints()
    .WithReference(softwareApiMocks)
    .WithReference(issuesDb);

builder.Build().Run();
