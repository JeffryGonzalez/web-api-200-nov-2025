

using Alba;
using HelpDesk.Api.HttpClients;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Microsoft.Extensions.Time.Testing;

namespace HelpDesk.Tests.Demos;
[Collection("WireMockFixture")]
public class GettingSoftwareMockedApi(DemoFixture fixture)
{
    [Fact]
    public async Task GettingSoftwareThatIsThere()
    {
       

   
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
        fixture.MockServer.Given(
            Request.Create()
            .UsingMethod("GET")
            .WithPath($"/help-desk/catalog-items/{softwareId}")) // this was the bug - duh.
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new SoftwareCenterResponse
                {
                    Title = expectedResponse.Title,
                    Vendor = expectedResponse.Vendor,
                })
            );



      
        var response = await fixture.Host.Scenario(api =>
        {
            api.Get.Url(getPath);
            api.StatusCodeShouldBe(200);
        });

       
        var body = response.ReadAsJson<SoftwareCatalogItem>();
        Assert.NotNull(body);
        Assert.Equal(expectedResponse.Title, body.Title);
        Assert.Equal(expectedResponse.Vendor, body.Vendor);
        Assert.Equal(expectedResponse.Id, body.Id);
        Assert.True(body.RetrievedAt.HasValue);
        //Assert.Equal<DateTimeOffset?>(requestTime, expectedResponse.RetrievedAt);
        var diff = body.RetrievedAt.Value - requestTime;
        Assert.True(diff.Seconds <= 5); // "Fudge" Value

    }
    [Fact]
    public async Task GettingSoftwareThatIsNotThere()
    {

        fixture.MockServer.ResetMappings(); // got luck here, but they can bleed through.
        var requestTime = DateTimeOffset.Now;
        var softwareId = Guid.Parse("f81dbfab-2a30-4e76-98e4-d1a67799731e");

      
        var getPath = "/demos/software/" + softwareId;


        var response = await fixture.Host.Scenario(api =>
        {
            api.Get.Url(getPath);
            api.StatusCodeShouldBe(200);
        });


        var body = response.ReadAsText();
        Assert.NotNull(body);
        Assert.Equal("{\"message\":\"Sorry, no Software with that id\"}", body);


    }
}
