using Alba;
using Alba.Security;
using HelpDesk.Api.Employee.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Wolverine;
using Wolverine.Tracking;

namespace HelpDesk.Tests.Fixtures;

public class AuthenticatedSystemTestFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; private set; } = null!;
    public readonly string AuthenticatedSub = "test@company.com";


    private PostgreSqlContainer _container = null!;
    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17.5-bullseye")
            .WithDatabase("issues")
            .Build();
        await _container.StartAsync();
        Host = await AlbaHost.For<Program>((config) =>
        {
            config.UseSetting("ConnectionStrings:issues", _container.GetConnectionString());
            config.UseSetting("services:software:http:0", "https://bad");
            config.UseSetting("services:vip-api:http:0", "https://bad");
        }, new AuthenticationStub().WithName(AuthenticatedSub));

        // 
    
        //await Host.InvokeMessageAndWaitAsync(new ProcessEmployeeIssue(new Api.Employee.Models.EmployeeIssueReadModel
        //{
        //    Id = Guid.NewGuid(),
        //    ContactMechanisms = new Api.Employee.Models.IssueContactMechanism
        //    {
        //        Phone = "555-1212"
        //    },
        //    ContactPreferences = Api.Employee.Models.IssueContactPreferences.Phone,
        //    Description = "Blewey!",
        //    Software = new Api.HttpClients.SoftwareCatalogItem
        //    {
        //        Id = Guid.NewGuid()
        //    }
        //}));
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        await _container.DisposeAsync();
    }
}
