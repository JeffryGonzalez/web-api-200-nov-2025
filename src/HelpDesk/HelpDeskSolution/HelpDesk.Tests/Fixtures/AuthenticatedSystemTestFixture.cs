using Alba;
using Alba.Security;
using Testcontainers.PostgreSql;

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

        }, new AuthenticationStub().WithName(AuthenticatedSub));

        // using var scope  = Host.Services.CreateScope();
        // var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
        // session.Store(/*some data*/);
        //
        // await session.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        await _container.DisposeAsync();
    }
}
