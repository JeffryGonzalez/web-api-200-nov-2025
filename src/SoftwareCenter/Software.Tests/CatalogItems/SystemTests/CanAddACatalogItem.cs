using System.Security.Claims;
using Software.Api.CatalogItems.Models;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.CatalogItems.SystemTests;

[Collection("AuthenticatedSystemTestFixture")]
[Trait("Category", "SystemsTest")]
public class CanAddACatalogItem(AuthenticatedSystemTestFixture fixture)
{
    [Fact]
    public async Task MembersOfTheSoftwareCenterCanAddACatalogItem()
    {
        var catalogItemToPost = new CatalogItemCreateRequest()
        {
            Name = "Visual Studio Code",
            Description = "An Editor For Programmers",
            Version = "119.1.0"
        };
        var vendorId = fixture.SeededVendor1.Id;

        var postResponse = await fixture.Host.Scenario(api =>
        {
            api.Post.Json(catalogItemToPost).ToUrl($"/vendors/{vendorId}/catalog-items");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.StatusCodeShouldBe(201);
        });

        var location = postResponse.Context.Response.Headers.Location.First();
        var path = new Uri(location!).AbsolutePath;
        var postBody = postResponse.ReadAsJson<VendorDetailsModel>();
        Assert.NotNull(postBody);

        var getResponse = await fixture.Host.Scenario(api =>
            {
                api.Get.Url(path);
            }
        );
        
        var getBody = getResponse.ReadAsJson<VendorDetailsModel>();
        Assert.NotNull(getBody);
        Assert.Equal(postBody, getBody);
    }

    [Fact]
    public async Task AddingToBadVendorGivesFourOhFour()
    {
        var catalogItemToPost = new CatalogItemCreateRequest()
        {
            Name = "Visual Studio Code",
            Description = "An Editor For Programmers",
            Version = "119.1.0"
        };
        
        var postResponse = await fixture.Host.Scenario(api =>
        {
            api.Post.Json(catalogItemToPost).ToUrl($"/vendors/{Guid.NewGuid()}/catalog-items");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.StatusCodeShouldBe(404);
        });

        var postResponseBody = await postResponse.ReadAsTextAsync();
        Assert.NotNull(postResponseBody);
        Assert.Equal("\"That vendor doesn't exist\"", postResponseBody);
    }
}