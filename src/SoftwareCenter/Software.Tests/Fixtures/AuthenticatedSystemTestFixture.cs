using Alba.Security;
using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Software.Api.Vendors;
using Software.Api.Vendors.Data;
using Software.Api.Vendors.Models;
using Testcontainers.PostgreSql;

namespace Software.Tests.Fixtures;

using Alba;



public class AuthenticatedSystemTestFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; private set; } = null!;
    public readonly string AuthenticatedSub = "test@company.com";

    public readonly VendorEntity SeededVendor1 = new VendorEntity()
    {
        Id = Guid.NewGuid(),
        Name = "Microsoft",
        Url = "https://microsoft.com",
        CreatedBy = "test@test.com",
        CreatedOn = new DateTimeOffset(1969,04,20,23,59,00, TimeSpan.FromHours(-4)),
        Contact = new PointOfContact()
        {
            Name = "bob@microsoft.com",
            Phone = "555-1212"
        }
    };
    public readonly VendorEntity SeededVendor2 = new VendorEntity()
    {
        Id = Guid.NewGuid(),
        Name = "Jetbrains",
        Url = "https://Jetbrains.com",
        CreatedBy = "test@test.com",
        CreatedOn = new DateTimeOffset(1969,04,20,23,59,00, TimeSpan.FromHours(-4)),
        Contact = new PointOfContact()
        {
            Name = "shirley@jetbrains.com"
        }
    };
    private PostgreSqlContainer _container = null!;
    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17.5-bullseye")
            .WithDatabase("software")
            .Build();
        await _container.StartAsync();
        Host = await AlbaHost.For<Program>((config) =>
        {
           // config.UseSetting("ConnectionStrings:Software", _container.GetConnectionString());
          
        }, new AuthenticationStub().WithName(AuthenticatedSub));
       
        using var scope  = Host.Services.CreateScope();
        var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
        session.Store(SeededVendor1);
        session.Store(SeededVendor2);
        await session.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }
}

public class AnonymousSystemTestFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; private set; } = null!;
    public async Task InitializeAsync()
    {
        Host = await AlbaHost.For<Program>();
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }
}

[CollectionDefinition("AuthenticatedSystemTestFixture")]
public class SystemTestFixtureCollection : ICollectionFixture<AuthenticatedSystemTestFixture>;


[CollectionDefinition("AnonymousSystemTestFixture")]
public class AnonymousTestFixtureCollection : ICollectionFixture<AnonymousSystemTestFixture>;