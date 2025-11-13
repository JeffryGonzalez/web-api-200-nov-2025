using System.Net;
using HelpDesk.Api.Services;
using Marten;

namespace HelpDesk.Api.HttpClients;

public class VipApiClient(HttpClient client, IDocumentSession session)
{
    public async Task<bool> CheckIfEmployeeIsVipAsync(Guid employeeId)
    {
        var userSub = await session.Query<UserIdentity>()
            .Where(u => u.Id == employeeId)
            .SingleOrDefaultAsync();
        if (userSub == null)
        {
            return false; // maybe throw? dispatch an event?
        }

        var request = new VipRequestMessage()
        {
            UserSubject = userSub.Name
        };
        var response = await client.PostAsJsonAsync($"/vip-check", request);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var body = await response.Content.ReadFromJsonAsync<VipResponseMessage>();
                return body is not null && body.IsVip;
            
            default:
                response.EnsureSuccessStatusCode();
                return false;
        }
    }
}

public record VipRequestMessage
{
    public required string UserSubject { get; init; }
}

public record VipResponseMessage
{
    public required string UserSubject { get; init; }
    public required bool IsVip { get; init; }
}

