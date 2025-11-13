using Marten;
using Microsoft.AspNetCore.Mvc;
using Software.Api.CatalogItems.Contracts;
using Software.Api.CatalogItems.Endpoints;
using Software.Api.CatalogItems.Entities;
using Software.Api.Vendors;
using Software.Api.Vendors.Data;

namespace Software.Api.CatalogItems;




public static class Extensions
{

    public static IServiceCollection AddCatalogItems(this IServiceCollection services)
    {
        services.AddScoped<ICheckForVendors, MartenVendorData>();
        return services;
    }
    public static WebApplication MapCatalogItems(this WebApplication builder)
    {
        var helpDeskGroup = builder.MapGroup("/help-desk");
        //  .RequireAuthorization("HelpDeskOnly");
        helpDeskGroup.MapGet("/catalog-items/{id:guid}", GetCatalogItemForHelpDesk.Handle);
        // add other things in the future.

        builder.MapGet("/catalog-items", async ([FromServices] IDocumentSession session) => await session.Query<CatalogItemEntity>().ToListAsync());
        
        var group = builder.MapGroup("/vendors").RequireAuthorization(); // unless you are identified with a JWT

        group.MapGet("/{vendorId:guid}/catalog-items/{id:guid}", GetCatalogItem.Handle)
            .WithName("get-catalog-item");
    
        group.MapGet("/{vendorId:guid}/catalog-items/", GetCatalogItems.Handle);

        group.MapPost("/{vendorId:guid}/catalog-items/", AddCatalogItem.Handle).RequireAuthorization("SoftwareCenter"); // and you are SoftwareCenter
       
        return builder;
    }
}