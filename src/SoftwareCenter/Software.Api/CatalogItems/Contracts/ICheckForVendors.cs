namespace Software.Api.CatalogItems.Contracts;

public interface ICheckForVendors
{
    Task<bool> DoesVendorExistAsync(Guid id, CancellationToken token);
}