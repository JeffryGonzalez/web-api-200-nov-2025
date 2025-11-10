using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Contracts;
using Software.Api.CatalogItems.Entities;
using Software.Api.CatalogItems.Models;

namespace Software.Api.CatalogItems.Endpoints;

public static class GetCatalogItems
{
    public static async Task<
        Results<
            Ok<IReadOnlyList<CatalogItemResponse>>,
            NotFound<string>>
    > Handle(
        Guid vendorId,
        IDocumentSession session,
        ICheckForVendors vendorChecker,
        CancellationToken token)
    {
        if (!await vendorChecker.DoesVendorExistAsync(vendorId, token))
            return TypedResults.NotFound("No Vendor With That Id");
        var results = await session.Query<CatalogItemEntity>()
            .Where(x => x.VendorId == vendorId)
            .ProjectTo()
            .ToListAsync(token);
        return TypedResults.Ok(results);

    }
}