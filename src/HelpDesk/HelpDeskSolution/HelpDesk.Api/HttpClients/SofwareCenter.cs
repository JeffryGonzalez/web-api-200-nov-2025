namespace HelpDesk.Api.HttpClients;

// Typed HttpClients 
public class SoftwareCenter(HttpClient client, TimeProvider clock)
{
    public async Task<SoftwareCatalogItem?> ValidateSoftwareItemFromCatalogAsync(Guid softwareId)
    {
        // Todo: think about doing a consumer/provider pattern - later.
        var response = await client.GetAsync("/catalog-items/" + softwareId);

        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var responseDate = response.Headers.Date;
            var returnedBody = await response.Content.ReadFromJsonAsync<SoftwareCatalogItem>();
            if (returnedBody is null) return null;
            returnedBody.RetrievedAt = responseDate ?? clock.GetUtcNow();
            return returnedBody;
        }

        if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }

        return null;

    }
}

public record SoftwareCatalogItem
{
    public DateTimeOffset RetrievedAt { get; set; }
    public required string Title { get; init; } = string.Empty;
    public required string Vendor { get; init; } = string.Empty;
}