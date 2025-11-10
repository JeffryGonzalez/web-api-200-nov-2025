using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors.SystemTests;

[Trait("Category", "SystemsTest")]
[Collection("AuthenticatedSystemTestFixture")]
public class GettingAVendor(AuthenticatedSystemTestFixture fixture)
{
    [Fact]
    public async Task GettingAnExitingVendor()
    {

        var getResponse = await fixture.Host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{fixture.SeededVendor1.Id}");
        });
        
        var responseBody = getResponse.ReadAsJson<VendorDetailsModel>();
        Assert.NotNull(responseBody);
        Assert.Equal(fixture.SeededVendor1.Id, responseBody.Id);
        Assert.Equal(fixture.SeededVendor1.Name, responseBody.Name);
        Assert.Equal(fixture.SeededVendor1.CreatedBy, responseBody.CreatedBy);
        
    }
    
    [Fact]
    public async Task NonExistentReturns404()
    {

        var getResponse = await fixture.Host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{Guid.NewGuid()}");
            api.StatusCodeShouldBe(404);
        });
        
   
    }
}
