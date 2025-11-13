

// Test Plan - I'm going to want at least two tests here
// One that shows what my API does when the other API returns a null (no software)
// One that shows what my API does when the other API does return software



using Alba;
using HelpDesk.Api.HttpClients;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace HelpDesk.Tests.Demos;
public class GettingSoftwareGrayBox
{
    // Pretty good confidence, but not great.

    [Fact]
    public async Task WhenSoftwareIsNotSupported()
    {
        var softwareId = Guid.Parse("f81dbfab-2a30-4e76-98e4-d1a67799731e");
        var host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("services:software:http:0", "http://never-used");
            // configure test services are used to replace real services during tests
            config.ConfigureTestServices(sp =>
            {
                var fakeSoftwareApi = Substitute.For<ILookupSoftwareFromTheSoftwareApi>();
                fakeSoftwareApi.ValidateSoftwareItemFromCatalogAsync(softwareId)
                .Returns(Task.FromResult<SoftwareCatalogItem?>(null)); // think that'll do a null.
                sp.AddScoped<ILookupSoftwareFromTheSoftwareApi>((_) => fakeSoftwareApi);
            });
        });

        var response = await host.Scenario(api =>
        {
            api.Get.Url("/demos/software/" + softwareId);
            api.StatusCodeShouldBe(200);
        });

        var body = response.ReadAsText();
        Assert.NotNull(body);
        Assert.Equal("{\"message\":\"Sorry, no Software with that id\"}", body);
    }
    [Fact]
    public async Task WhenSoftwareIsSupported()
    {
        
        var softwareId = Guid.Parse("f81dbfab-2a30-4e76-98e4-d1a67799731e");

        var expectedResponse = new SoftwareCatalogItem
        {
            Id = softwareId,
            Title = "Visual Studio 2026",
            Vendor = "Microsoft"
        };
        var host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("services:software:http:0", "http://never-used");
            // configure test services are used to replace real services during tests
            config.ConfigureTestServices(sp =>
            {
                var fakeSoftwareApi = Substitute.For<ILookupSoftwareFromTheSoftwareApi>();
                fakeSoftwareApi.ValidateSoftwareItemFromCatalogAsync(softwareId)
                .Returns(Task.FromResult<SoftwareCatalogItem?>(expectedResponse)); // think that'll do a null.
                sp.AddScoped<ILookupSoftwareFromTheSoftwareApi>((_) => fakeSoftwareApi);
            });
        });

        var response = await host.Scenario(api =>
        {
            api.Get.Url("/demos/software/" + softwareId);
            api.StatusCodeShouldBe(200);
        });

        var body = response.ReadAsJson<SoftwareCatalogItem>();
        Assert.NotNull(body);
        Assert.Equal(expectedResponse.Id, body.Id);
        Assert.Equal(expectedResponse.Title, body.Title);
        // etc. etc.
    }

}
