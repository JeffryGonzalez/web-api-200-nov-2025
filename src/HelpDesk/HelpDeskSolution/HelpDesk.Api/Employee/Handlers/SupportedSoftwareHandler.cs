using HelpDesk.Api.HttpClients;
using Marten;

namespace HelpDesk.Api.Employee.Handlers;

public class SupportedSoftwareHandler
{
    public async Task Handle(CheckForSupportedSoftware command, 
        SoftwareCenterApiClient softwareApi,
        IDocumentSession session)
    {
        var response = await softwareApi.ValidateSoftwareItemFromCatalogAsync(command.SoftwareId);
        if (response is not null)
        {
            session.Events.Append(command.IssueId, new SupportedSoftwareReported(response));
            
        }
        else
        {
            session.Events.Append(command.IssueId, new UnsupportedSoftwareReported());
        }
        // write the code to call the software api with the command.SoftwareId
        // if it is there, return the title and vendor of the software,
        // if it isn't log that this is not supported.
        // In the same database transaction that is writing this event, the projection is updated too.
        await session.SaveChangesAsync();

    }
}