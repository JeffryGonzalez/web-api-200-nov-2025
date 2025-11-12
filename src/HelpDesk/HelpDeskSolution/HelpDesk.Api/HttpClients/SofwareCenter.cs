namespace HelpDesk.Api.HttpClients;

// Typed HttpClients 
public class SoftwareCenter(HttpClient client, TimeProvider clock) : ILookupSoftwareFromTheSoftwareApi
{
    public async Task<SoftwareCatalogItem?> ValidateSoftwareItemFromCatalogAsync(Guid softwareId)
    {
        // Todo: think about doing a consumer/provider pattern - later.
        // [HttpGet("/catalog-items/{id:guid}"]

        var response = await client.GetAsync("/catalog-items/" + softwareId);




        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
   
            var responseDate = response.Headers.Date ?? DateTime.MinValue;
            var returnedBody = await response.Content.ReadFromJsonAsync<SoftwareCenterResponse>();
            if (returnedBody is null) return null; // todo: think about this - something bad.
            var mappedResponse = new SoftwareCatalogItem
            {
                Id = softwareId,
                Title = returnedBody.Title,
                Vendor = returnedBody.Vendor,
                RetrievedAt = responseDate
            };
            return mappedResponse;
        }

        // if we haven't already handled this, then it is some other Http Error - let resiliency handle it.
        if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
        {
            // please punch me in the nose real hard. Throw an exception if the response
            // status code is >299 (so, 405, 500, etc. or the service isn't available.
            response.EnsureSuccessStatusCode();
        }

        return null;

    }
}

// part of the contract or "pact" I'm going to have with the software center
// in this case, "they" are going to use this to fulfill this contract.
// It could ALSO be something that is already there...

public record SoftwareCenterResponse
{
    public string? Title { get; init; }
    public string? Vendor { get; init; }
}
public record SoftwareCatalogItem
{
    public Guid Id { get; set; }
    public DateTimeOffset? RetrievedAt { get; set; }
    public string? Title { get; init; } 
    public string? Vendor { get; init; } 
}

// SRM - need a get-catalog-item/{id} that returns a 404 or { title: string, vendor: string }