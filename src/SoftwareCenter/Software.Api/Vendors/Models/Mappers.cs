using Software.Api.Vendors.Data;

namespace Software.Api.Vendors.Models;

using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class VendorMappers
{
    public static partial IQueryable<VendorSummaryItem> ProjectToSummary(this IQueryable<VendorEntity> q);


    [MapperIgnoreSource(nameof(VendorEntity.Contact))]
    [MapperIgnoreSource(nameof(VendorEntity.CreatedBy))]
    [MapperIgnoreSource(nameof(VendorEntity.CreatedOn))]
    [MapperIgnoreSource(nameof(VendorEntity.Url))]
    public static partial VendorSummaryItem MapFromEntity(this VendorEntity entity);


    public static partial VendorDetailsModel MapToResponse(this VendorEntity entity);
}

