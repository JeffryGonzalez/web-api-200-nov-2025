using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alba;
using HelpDesk.Api;
using HelpDesk.Api.Services;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WireMock.Server;

namespace HelpDesk.Tests.Demos;
public class DemoFixture : IAsyncLifetime
{
    public WireMockServer MockServer { get; private set; } = null!;
    public IAlbaHost Host { get; private set; } = null!;
    public IServiceScope Scope { get; private set; } = null;
    private PostgreSqlContainer _container = null!;
    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17.5-bullseye")
            .WithDatabase("issues")
            .Build();
        await _container.StartAsync();
        MockServer = WireMockServer.Start();
        Host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("services:software:http:0", MockServer.Url); 
            config.UseSetting("services:vips:http:0", MockServer.Url);
            config.UseSetting("ConnectionStrings:issues", _container.GetConnectionString());
        });
       Scope = Host.Services.CreateScope();
       var session = Scope.ServiceProvider.GetRequiredService<IDocumentSession>();
       session.Store(new UserIdentity()
       {
           Id =Guid.Parse("2F227AD9-B448-4616-9057-05E7763716EA"),
           Name = "bob@company.com"
       });
       session.Store(new UserIdentity()
       {
           Id = Guid.Parse("171D52A7-A55F-4AAF-8CDA-F09D406C7DF4"),
           Name = "sue@company.com"
       });
       await session.SaveChangesAsync();
    }
    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        MockServer.Stop();
        Scope.Dispose();
        await _container.StopAsync();
        await _container.DisposeAsync();
    }

 
}


[CollectionDefinition("WireMockFixture")]
public class WireMockFixture : ICollectionFixture<DemoFixture>;