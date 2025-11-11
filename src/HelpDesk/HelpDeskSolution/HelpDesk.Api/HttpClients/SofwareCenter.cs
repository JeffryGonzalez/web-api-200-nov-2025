namespace HelpDesk.Api.HttpClients;


// Typed HttpClients 
public class SoftwareCenter(HttpClient client)
{
    public async Task<SoftwareCatalogItem?> ValidateSoftwareItemFromCatalogAsync(Guid softwareId)
    {
        // Todo: think about doing a consumer/provider pattern - later.
        var response = await client.GetAsync("/catalog-items/" + softwareId);
        
       //  response.EnsureSuccessStatusCode();

        if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null; // This means they don't support this software - it isn't in the catalog.
        }

        var returnedBody = await response.Content.ReadFromJsonAsync<SoftwareCatalogItem>();
        return returnedBody;
    }
}

public record SoftwareCatalogItem
{
    public string Title { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
}