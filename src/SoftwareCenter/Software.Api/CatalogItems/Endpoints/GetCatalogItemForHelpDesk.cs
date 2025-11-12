using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Entities;
using Software.Api.CatalogItems.Models;
using Software.Api.Vendors.Data;

namespace Software.Api.CatalogItems.Endpoints;

public class GetCatalogItemForHelpDesk
{


    public static async Task<
                Results<
                    Ok<HelpDeskCatalogItem>,
                    NotFound>
            >
            Handle(Guid id,  IDocumentSession session, CancellationToken token)
    {
        var response = await session.Query<CatalogItemEntity>().Where(c => c.Id == id)
           
            .FirstOrDefaultAsync(token);

        if (response == null)
        {
            return TypedResults.NotFound();
        }
        if (response != null)
        {
            var vendor = await session.Query<VendorEntity>()
                .Where(v => v.Id == response.VendorId)
                .SingleOrDefaultAsync(token);

            if (vendor == null) { return TypedResults.NotFound(); }

            var responseMessage = new HelpDeskCatalogItem
            {
                Title = response.Name,
                Vendor = vendor.Name

            };
            return TypedResults.Ok(responseMessage);
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}


public class HelpDeskCatalogItem
{
    public string Title { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
}
