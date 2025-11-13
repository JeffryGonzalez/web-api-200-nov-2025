using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace HelpDesk.SystemIntegrationTests.Tests;
public class IntegrationTest1
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    // Instructions:
    // 1. Add a project reference to the target AppHost project, e.g.:
    //
    //    <ItemGroup>
    //        <ProjectReference Include="../MyAspireApp.AppHost/MyAspireApp.AppHost.csproj" />
    //    </ItemGroup>
    //
    // 2. Uncomment the following example test and update 'Projects.MyAspireApp_AppHost' to match your AppHost project:
    //
    [Fact(Skip ="Not Doing this stuff")]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HelpDeskHost>(cancellationToken);
        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        // Act
        using var httpClient = app.CreateHttpClient("helpdesk-api");
        await app.ResourceNotifications.WaitForResourceHealthyAsync("helpdesk-api", cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);



        var response = await httpClient.GetAsync("/demos/still-open");
        var content = await response.Content.ReadAsStringAsync();
        // probably don't to deserialize into the types owned by the helpdesk-api.

        //var responseBody = await response.Content.ReadFromJsonAsync<>

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
