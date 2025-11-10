namespace Software.Api.Vendors.Models;

public record VendorSummaryItem
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}