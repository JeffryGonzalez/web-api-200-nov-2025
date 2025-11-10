using Alba;
using Software.Api.Vendors;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors.SystemTests;

/*
 * We do not allow unauthenticated (anonymous) users to create vendors
 * Note: Authenticated means they have identified themselves with the IDP and have a proper authorization header.
 */


[Collection("AnonymousSystemTestFixture")]
[Trait("Category", "SystemsTest")]
public class Authentication(AnonymousSystemTestFixture fixture)
{
    private readonly IAlbaHost _host = fixture.Host;
    [Fact]
    public async Task UnauthenticatedGetsA401WhenPostingVendor()
    {
        // We are using an intentionally constructed bad request to ensure we don't get a 400 instead of the expected 401
        var badRequest = new VendorCreateModel { Name = "", Contact = null!, Url = "" };

        await _host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors");
            api.StatusCodeShouldBe(401);
        });
    }
    [Fact]
    public async Task UnauthenticatedGetsA401GettingVendors()
    {
      
        await _host.Scenario(api =>
        {
            api.Get.Url("/vendors");
            api.StatusCodeShouldBe(401);
        });
    }
    [Fact]
    public async Task UnauthenticatedGetsA401GettingAVendor()
    {
      
        await _host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{Guid.NewGuid()}");
            api.StatusCodeShouldBe(401);
        });
    }
}