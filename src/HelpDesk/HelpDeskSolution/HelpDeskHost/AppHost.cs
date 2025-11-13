

using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);


var mappingPath = Path.Combine("..", "wiremock-mappings");
if (!Directory.Exists(mappingPath))
{
    throw new Exception("Can't Find Mapping File");
}

var softwareApiMocks = builder.AddWireMock("software")
      .WithMappingsPath(mappingPath)
    .WithReadStaticMappings()
    .WithWatchStaticMappings();


// The software API is external. I dont want to have to have that running, etc.
// so I'm just going to have it return 404 for every call for right now.


var softwareCenter = builder.AddExternalService("software-center", "http://localhost:1337");
   
   
            

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
var vipsDatabase = postgres.AddDatabase("vips");


var vipApi = builder.AddProject<Projects.HelpDesk_Vips_Api>("vip-api")
    .WithExternalHttpEndpoints()
    .WithReference(vipsDatabase);
    

var issuesApi = builder.AddProject<Projects.HelpDesk_Api>("helpdesk-api")
    .WithExternalHttpEndpoints()
    .WithReference(softwareApiMocks)
    .WithReference(vipApi)
    //.WithEnvironment("services__vip__http__0", softwareApiMocks.GetEndpoint("http"))
    .WithReference(issuesDb).WaitFor(issuesDb);

builder.AddYarp("gateway") // create one url that will "proxy" to these services
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute(issuesApi); // go here as the "root"
        yarp.AddRoute("/catalog/{**catchAll}", softwareCenter)
         .WithTransformPathRemovePrefix("/catalog");

        yarp.AddRoute("/vips/{**catchAll}", vipApi) // but if the path starts with "vips" go here
            .WithTransformPathRemovePrefix("/vips") // but remove that prefix before sending it.
        ;
    });


builder.Build().Run();
