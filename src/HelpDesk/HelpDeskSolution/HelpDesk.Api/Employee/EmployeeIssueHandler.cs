using System.Threading.Tasks;
using HelpDesk.Api.Employee.Models;
using HelpDesk.Api.HttpClients;
using Marten;
using Wolverine;

namespace HelpDesk.Api.Employee;


public record ProcessEmployeeIssue(IssueCreateResponseModel Issue);

public record EmployeeSubmittedIssue(IssueCreateResponseModel Issue);
public record CheckForVipStatus(Guid IssueId, Guid EmployeeId);
public record CheckForSupportedSoftware(Guid IssueId, Guid SoftwareId);


public class EmployeeIssueHandler // some magic here - this class MUST end with the word "Handler"
{
    public async Task<OutgoingMessages> Handle(ProcessEmployeeIssue command, ILogger<EmployeeIssueHandler> logger, IMessageContext messageBus, IDocumentSession session)
    {
        session.Events.Append(command.Issue.Id, new EmployeeSubmittedIssue(command.Issue));
        await session.SaveChangesAsync();
        logger.LogInformation("Handling the issue - {description}", command.Issue.Description);
        return new OutgoingMessages
        {
            new CheckForVipStatus(command.Issue.Id, command.Issue.SubmittedBy),
            new CheckForSupportedSoftware(command.Issue.Id, command.Issue.SoftwareId)
        };
       // messageBus.SendAsync( new CheckForVipStatus(command.Issue.Id, command.Issue.SubmittedBy));
        //await messageBus.InvokeAsync(new CheckForSupportedSoftware(command.Issue.Id, command.Issue.SoftwareId));
    }
}

public record VipIssueReported();
public record NonVipIssueReported();


public class VipStatusHandler
{
    public async Task Handle(CheckForVipStatus command, IDocumentSession session)
    {
        // Write the code here (after break) to check if this person is a VIP or not.
 
        // if they are, then log that this issue is for a vip, otherwise, log that they aren't.
        session.Events.Append(command.IssueId, new VipIssueReported());
        
        await session.SaveChangesAsync();
       
    }
}

public record SupportedSoftwareReported(string Title, string Vendor);
public record UnsupportedSoftwareReported();
public class SupportedSoftwareHandler
{
    public async Task Handle(CheckForSupportedSoftware command, 
        SoftwareCenter softwareApi,
        IDocumentSession session)
    {
        var response = await softwareApi.ValidateSoftwareItemFromCatalogAsync(command.SoftwareId);
        if (response is not null)
        {
            session.Events.Append(command.IssueId, new SupportedSoftwareReported(response.Title, response.Vendor));
            
        }
        else
        {
            session.Events.Append(command.IssueId, new UnsupportedSoftwareReported());
        }
        // write the code to call the software api with the command.SoftwareId
        // if it is there, return the title and vendor of the software,
        // if it isn't log that this is not supported.
        await session.SaveChangesAsync();

    }
}