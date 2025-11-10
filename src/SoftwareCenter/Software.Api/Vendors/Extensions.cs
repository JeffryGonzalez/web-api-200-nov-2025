using FluentValidation;
using Software.Api.Vendors.Data;
using Software.Api.Vendors.Models;

namespace Software.Api.Vendors;

public static class Extensions
{
    public static IServiceCollection AddVendors(this IServiceCollection services)
    {
        services.AddScoped<ICreateVendors, MartenVendorData>();
        services.AddScoped<ILookupVendors, MartenVendorData>();
        services.AddScoped<IValidator<VendorCreateModel>, VendorCreateModelValidator>();

        return services;
    }
}
