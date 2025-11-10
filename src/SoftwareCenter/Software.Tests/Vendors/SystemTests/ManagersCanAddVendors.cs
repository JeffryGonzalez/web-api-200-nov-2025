using System.Security.Claims;
using Alba;
using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors.SystemTests;

[Collection("AuthenticatedSystemTestFixture")]
[Trait("Category", "SystemsTest")]
public class ManagersCanAddVendors(AuthenticatedSystemTestFixture fixture)
{

    private readonly IAlbaHost _host = fixture.Host;
    [Fact]

    public async Task CanAddAVendor()
    {

        var vendorToPost = new VendorCreateModel
        {
            Name = "Microsoft",
            Contact = new PointOfContact
            {
                Name = "Satya",
                Email = "satya@microsoft.com",
                Phone = "555-1212"
            },
            Url = "https://microsoft.com"

        };
        

        var postResponse = await _host.Scenario(api =>
        {
            api.Post.Json(vendorToPost).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.StatusCodeShouldBe(201);
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        Assert.NotNull(postResponseBody);
        Assert.Equal(fixture.AuthenticatedSub, postResponseBody.CreatedBy);

        var location = postResponse.Context.Response.Headers.Location.First();
       
        var getResponse = await _host.Scenario(api =>
        {
            api.Get.Url(location!);
            api.StatusCodeShouldBeOk();
        });

        var getResponseBody = await getResponse.ReadAsJsonAsync<VendorDetailsModel>();
        Assert.NotNull(getResponse);

        Assert.Equal(postResponseBody, getResponseBody);

    }
}


