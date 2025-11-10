namespace Software.Api.CatalogItems.Entities;

public class CatalogItemEntity
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;

    public Guid VendorId { get; init; }
}