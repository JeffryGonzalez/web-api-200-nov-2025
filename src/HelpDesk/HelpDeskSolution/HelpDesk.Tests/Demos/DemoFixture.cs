using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alba;
using WireMock.Server;

namespace HelpDesk.Tests.Demos;
public class DemoFixture : IAsyncLifetime
{
    public WireMockServer MockServer { get; private set; } = null!;
    public IAlbaHost Host { get; private set; } = null!;
    public async Task InitializeAsync()
    {
        MockServer = WireMockServer.Start();
        Host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("services:software:http:0", MockServer.Url); 
        });
    }
    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        MockServer.Stop();
    }

 
}


[CollectionDefinition("WireMockFixture")]
public class WireMockFixture : ICollectionFixture<DemoFixture>;