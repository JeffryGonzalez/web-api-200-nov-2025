

using Alba;
using HelpDesk.Api.HttpClients;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Microsoft.Extensions.Time.Testing;

namespace HelpDesk.Tests.Demos;
public class GettingSoftwareMockedApi
{
    [Fact]
    public async Task GettingSoftwareThatIsThere()
    {
        var mockServer = WireMockServer.Start();

   
        var requestTime = DateTimeOffset.Now;
        var softwareId = Guid.Parse("f81dbfab-2a30-4e76-98e4-d1a67799731e");

        var expectedResponse = new SoftwareCatalogItem
        {
            Id = softwareId,
            Title = "Visual Studio 2026",
            Vendor = "Microsoft",
            RetrievedAt = DateTimeOffset.UtcNow
        };
        var getPath = "/demos/software/" + softwareId;
        mockServer.Given(
            Request.Create()
            .UsingMethod("GET")
            .WithPath($"/catalog-items/{softwareId}")) // this was the bug - duh.
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new SoftwareCenterResponse
                {
                    Title = expectedResponse.Title,
                    Vendor = expectedResponse.Vendor,
                })
            );



        var host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("services:software:http:0", mockServer.Url);
        });
        var response = await host.Scenario(api =>
        {
            api.Get.Url(getPath);
            api.StatusCodeShouldBe(200);
        });

       
        var body = response.ReadAsJson<SoftwareCatalogItem>();
        Assert.NotNull(body);
        Assert.Equal(expectedResponse.Title, body.Title);
        Assert.Equal(expectedResponse.Vendor, body.Vendor);
        Assert.Equal(expectedResponse.Id, body.Id);
       // Assert.Equal<DateTimeOffset?>(requestTime, expectedResponse.RetrievedAt);
    }
}
