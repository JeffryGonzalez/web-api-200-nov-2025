using HelpDesk.Api.Employee.Data;
using HelpDesk.Api.Employee.Handlers;
using HelpDesk.Api.HttpClients;
using Marten.Events.Aggregation;

namespace HelpDesk.Api.Employee.Models;

public class EmployeeIssueProjection : SingleStreamProjection<EmployeeIssueReadModel, Guid>
{
    public static EmployeeIssueReadModel Create(EmployeeSubmittedIssue @event)
    {
        var issue = new EmployeeIssueReadModel
        {
            Id = @event.EmployeeIssue.Id,
            ContactMechanisms = @event.EmployeeIssue.ContactMechanisms,
            ContactPreferences = @event.EmployeeIssue.ContactPreferences,
            Status =IssueStatus.AwaitingVerification,
            Description = @event.EmployeeIssue.Description,
            Impact = @event.EmployeeIssue.Impact,
            ImpactRadius = @event.EmployeeIssue.ImpactRadius,
            Software = new SoftwareCatalogItem()
            {
                Id = @event.EmployeeIssue.Software.Id,
            },
            SubmittedBy = @event.EmployeeIssue.SubmittedBy,
        };
       
        return issue;
    }
    
    public static EmployeeIssueReadModel Apply(VipIssueReported @event, EmployeeIssueReadModel model)
    {
        return model with { VipStatus = "Is Vip", };
    }

    public static EmployeeIssueReadModel Apply(SupportedSoftwareReported @event,  EmployeeIssueReadModel model)
    {
        return model with {  Software = @event.Item, SoftwareChecked = true};
    }
    public static EmployeeIssueReadModel Apply(UnsupportedSoftwareReported @event, EmployeeIssueReadModel model)
    {
        return model with { SoftwareMessage = "Unsupported Software", VipStatusChecked = true};
    }
}