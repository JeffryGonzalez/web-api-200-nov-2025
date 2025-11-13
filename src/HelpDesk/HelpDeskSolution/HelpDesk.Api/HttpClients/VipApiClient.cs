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
            return false; // maybe throw?
        }

        var request = new VipRequestMessage()
        {
            UserSubject = userSub.Name
        };
        var response = await client.PostAsJsonAsync($"/vips/{userSub.Name}", request);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return true;
            case HttpStatusCode.NotFound:
                return false;
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

