using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Contracts;
using Software.Api.CatalogItems.Entities;
using Software.Api.CatalogItems.Models;

namespace Software.Api.CatalogItems.Endpoints;

public static class AddCatalogItem
{
    public static async Task<
        Results<
            CreatedAtRoute<CatalogItemResponse>,
            NotFound<string>
        >
    > Handle(
        Guid vendorId,
        CatalogItemCreateRequest request,
        ICheckForVendors vendorChecker,
        IDocumentSession session,
        CancellationToken token
    )
    {
        if (!await vendorChecker.DoesVendorExistAsync(vendorId, token))
            return TypedResults.NotFound("That vendor doesn't exist");
        var entityToSave = new CatalogItemEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Version = request.Version,
            VendorId = vendorId,
        };
        session.Store(entityToSave);
        await session.SaveChangesAsync(token);

        var response = new CatalogItemResponse
        {
            Id = entityToSave.Id,
            Name = entityToSave.Name,
            Description = entityToSave.Description,
            Version = entityToSave.Version,
            VendorId = vendorId,
        };
        return TypedResults.CreatedAtRoute(response, "get-catalog-item", new { vendorId, id = entityToSave.Id });

    }
}