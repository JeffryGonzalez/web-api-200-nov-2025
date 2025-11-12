using Marten;
using Wolverine;

namespace HelpDesk.Api.Employee.Handlers;




public class EmployeeIssueHandler // some magic here - this class MUST end with the word "Handler"
{
    public async Task<OutgoingMessages> Handle(ProcessEmployeeIssue command, ILogger<EmployeeIssueHandler> logger, IMessageContext messageBus, IDocumentSession session)
    {
        session.Events.Append(command.EmployeeIssue.Id, new EmployeeSubmittedIssue(command.EmployeeIssue));
        await session.SaveChangesAsync();
        logger.LogInformation("Handling the issue - {description}", command.EmployeeIssue.Description);
        return
        [
            new CheckForVipStatus(command.EmployeeIssue.Id, command.EmployeeIssue.SubmittedBy),
            new CheckForSupportedSoftware(command.EmployeeIssue.Id, command.EmployeeIssue.Software.Id!.Value)
        ];
      
    }
}



