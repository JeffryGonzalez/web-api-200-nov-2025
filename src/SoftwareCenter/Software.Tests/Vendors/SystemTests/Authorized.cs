using System.Security.Claims;
using Alba;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors.SystemTests;

[Collection("AuthenticatedSystemTestFixture")]
[Trait("Category", "SystemsTest")]
public class Authorized(AuthenticatedSystemTestFixture fixture)
{
    private readonly IAlbaHost _host = fixture.Host;
    [Fact]
    public async Task MembersOfTheSoftwareCenterNotManagers()
    {
        var badRequest = new VendorCreateModel { Name = "", Contact = null!, Url = "" };
        await _host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.StatusCodeShouldBe(403);
        });

    }
    [Fact]
    public async Task AuthenticatedUsersNotSoftwareCenterNotManagers()
    {
        var badRequest = new VendorCreateModel { Name = "", Contact = null!, Url = "" };
        await _host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors");
            api.StatusCodeShouldBe(403);
        });

    }

    [Fact]
    public async Task AuthorizedUsersCanGetVendorList()
    {
        await _host.Scenario(api =>
        {
            api.Get.Url("/vendors");
            api.StatusCodeShouldBe(200);
        });
    }
    [Fact]
    public async Task AuthorizedUsersCanGetAVendor()
    {
        await _host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{fixture.SeededVendor1.Id}");
            api.StatusCodeShouldBe(200);
        });
        
        await _host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{Guid.NewGuid()}");
            api.StatusCodeShouldBe(404);
        });
        
    }


}