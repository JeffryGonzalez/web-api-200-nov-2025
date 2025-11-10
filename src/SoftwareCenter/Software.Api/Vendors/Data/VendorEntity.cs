using Software.Api.Vendors.Models;

namespace Software.Api.Vendors.Data;

public class VendorEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public PointOfContact Contact { get; set; } = new PointOfContact();
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }

    public VendorDetailsModel MapToDetails()
    {
        return new VendorDetailsModel(Id, Name, Url, Contact!, CreatedBy, CreatedOn);
    }
}