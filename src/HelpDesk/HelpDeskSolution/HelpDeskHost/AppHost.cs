var builder = DistributedApplication.CreateBuilder(args);


// this will start a docker container running postgres:17.5
var postgres = builder.AddPostgres("postgres")
       .WithImage("postgres:17.5-bullseye")
       .WithPgAdmin();

// I'm going to need a database in that called "issues"
var issuesDb = postgres.AddDatabase("issues");


builder.AddProject<Projects.HelpDesk_Api>("helpdesk-api")
    .WaitFor(issuesDb)
    .WithReference(issuesDb);
    

builder.Build().Run();
