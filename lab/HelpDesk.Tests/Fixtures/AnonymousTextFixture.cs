using Alba;

namespace HelpDesk.Tests.Fixtures;

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
