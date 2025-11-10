using Software.Api.Vendors.Models;
using Software.Tests.Fixtures;

namespace Software.Tests.Vendors.SystemTests;

[Trait("Category", "SystemsTest")]
[Collection("AuthenticatedSystemTestFixture")]
public class GettingVendorList(AuthenticatedSystemTestFixture fixture)
{
    [Fact]
    public async Task GetVendorList()
    {
        var getResponse = await fixture.Host.Scenario(api =>
        {
            api.Get.Url("/vendors");

        });

        var getBody = getResponse.ReadAsJson<IList<VendorSummaryItem>>();
        
        Assert.NotNull(getBody);
      
        var vendor1 = getBody.First(v => v.Id == fixture.SeededVendor1.Id);
        Assert.NotNull(vendor1);
        Assert.Equal(fixture.SeededVendor1.Name, vendor1.Name);
    }
    
}