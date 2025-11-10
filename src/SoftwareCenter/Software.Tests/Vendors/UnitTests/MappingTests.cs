
using Software.Api.Vendors;
using Software.Api.Vendors.Models;

namespace Software.Tests.Vendors.UnitTests;

public class MappingTests
{

    [Fact]
    [Trait("Category", "Unit")]

    public void CanMapVendorCreateToEntity()
    {
        var model = new VendorCreateModel
        {
            Name = "Company",
            Url = "https://company.xyz",
            Contact = new PointOfContact { Name = "Joe", Email = "joe@aol.com", Phone = "555-1212" }
        };

        var id = Guid.NewGuid();
        var mapped = model.MapToEntity(id, "jeff");

        Assert.NotNull(mapped);
        Assert.Equal(id, mapped.Id);
        Assert.Equal(model.Url, mapped.Url);
      
    }
}
