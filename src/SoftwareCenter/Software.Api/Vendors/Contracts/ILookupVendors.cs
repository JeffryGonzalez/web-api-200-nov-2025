

using Software.Api.Vendors.Models;

namespace Software.Api.Vendors;

public interface ILookupVendors
{
    Task<IReadOnlyList<VendorSummaryItem>> GetAllVendorsAsync(CancellationToken token);
    Task<VendorDetailsModel?> GetVendorByIdAsync(Guid id, CancellationToken token);
}