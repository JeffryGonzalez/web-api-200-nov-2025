using Riok.Mapperly.Abstractions;
using Software.Api.CatalogItems.Entities;

namespace Software.Api.CatalogItems.Models;

[Mapper]
public static partial class CatalogItemMappers
{
    public static partial IQueryable<CatalogItemResponse> ProjectTo(this IQueryable<CatalogItemEntity> entity);
}