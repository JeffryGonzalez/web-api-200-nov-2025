using Marten;

namespace HelpDesk.Api.Employee.Handlers;

public class VipStatusHandler
{
    public async Task Handle(CheckForVipStatus command, IDocumentSession session)
    {
        // Write the code here (after break) to check if this person is a VIP or not.
        // TODO: This is our last bit of bad fake classroom code here.
        // if they are, then log that this issue is for a vip, otherwise, log that they aren't.
        session.Events.Append(command.IssueId, new VipIssueReported());
        
        await session.SaveChangesAsync();
       
    }
}