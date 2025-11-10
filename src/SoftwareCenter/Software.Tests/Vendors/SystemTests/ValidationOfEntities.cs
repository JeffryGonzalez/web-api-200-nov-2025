using System.Security.Claims;
using Alba;
using Software.Api.Vendors;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors.SystemTests;

/*
 * This test is just to prove that the entities are validated. Testing for the actual validation rules are in the
 * Unit Tests
 */

[Collection("AuthenticatedSystemTestFixture")]
[Trait("Category", "SystemsTest")]
public class ValidationOfEntities(AuthenticatedSystemTestFixture fixture)
{
    private readonly IAlbaHost _host = fixture.Host;
    [Fact]
    public async Task InvalidVendorRequestsReturnBadRequest()
    {
     

        var badRequest = new VendorCreateModel { Name = "", Contact = null!, Url = "" };

        await _host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.StatusCodeShouldBe(400);
        });

    }
}