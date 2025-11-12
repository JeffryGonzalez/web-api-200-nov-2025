
namespace HelpDesk.Api.HttpClients;

public interface ILookupSoftwareFromTheSoftwareApi
{
    Task<SoftwareCatalogItem?> ValidateSoftwareItemFromCatalogAsync(Guid softwareId);
}