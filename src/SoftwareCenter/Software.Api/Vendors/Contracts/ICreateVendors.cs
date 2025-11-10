
using Software.Api.Vendors.Models;

namespace Software.Api.Vendors;

public interface ICreateVendors
{
    Task<VendorDetailsModel> CreateVendorAsync(VendorCreateModel request);
}