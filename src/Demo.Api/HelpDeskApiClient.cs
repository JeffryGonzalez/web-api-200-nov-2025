namespace Demo.Api;

public class HelpDeskApiClient(HttpClient client)
{
    public async Task<bool> IsIssueResolvedAsync(Guid issueId)
    {
       var response = await client.GetAsync("/issues/" + issueId);
        // blah blah
        response.EnsureSuccessStatusCode(); 
        return true;
    }
}
