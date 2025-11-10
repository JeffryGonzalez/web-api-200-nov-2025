

namespace Software.Api.Vendors.Models;

public record VendorDetailsModel(Guid Id, string Name, string Url, PointOfContact Contact, string CreatedBy, DateTimeOffset CreatedOn);