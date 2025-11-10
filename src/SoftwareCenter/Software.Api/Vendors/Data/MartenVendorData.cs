using Marten;
using Software.Api.CatalogItems.Contracts;
using Software.Api.Vendors.Models;

namespace Software.Api.Vendors.Data;

public class MartenVendorData(IDocumentSession session, IHttpContextAccessor httpContextAccessor) : ICreateVendors, ILookupVendors, ICheckForVendors
{
    public async Task<VendorDetailsModel> CreateVendorAsync(VendorCreateModel request)
    {
        var name = GetCurrentUserSub() ?? throw new Exception("Cannot be used in an unauthenticated context");
        var vendorToSave = request.MapToEntity(Guid.NewGuid(), name);

        session.Store(vendorToSave);
        await session.SaveChangesAsync();
        var response = new VendorDetailsModel(vendorToSave.Id, vendorToSave.Name, vendorToSave.Url, vendorToSave.Contact, vendorToSave.CreatedBy, vendorToSave.CreatedOn);
        return response;
    }

    public async Task<bool> DoesVendorExistAsync(Guid id, CancellationToken token)
    {
        return await session.Query<VendorEntity>().AnyAsync(v => v.Id == id, token);
    }

    public async Task<IReadOnlyList<VendorSummaryItem>> GetAllVendorsAsync(CancellationToken token)
    {
        var results = await session.Query<VendorEntity>()
          .OrderBy(r => r.CreatedOn)
          .ProjectToSummary()
            .ToListAsync(token: token);

        return results;
    }

    public async Task<VendorDetailsModel?> GetVendorByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await session.Query<VendorEntity>().Where(v => v.Id == id).SingleOrDefaultAsync(token);
        return entity?.MapToDetails();
    }
    
    private string? GetCurrentUserSub()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}

