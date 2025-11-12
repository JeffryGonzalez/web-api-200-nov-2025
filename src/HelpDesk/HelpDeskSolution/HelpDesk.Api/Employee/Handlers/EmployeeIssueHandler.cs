using System.Threading.Tasks;
using HelpDesk.Api.Employee.Models;
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

public record SupportedSoftwareReported(string Title, string Vendor);
public record UnsupportedSoftwareReported();