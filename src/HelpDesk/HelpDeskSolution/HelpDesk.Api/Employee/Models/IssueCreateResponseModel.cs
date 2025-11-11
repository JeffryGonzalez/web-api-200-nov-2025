using HelpDesk.Api.Employee.Data;

namespace HelpDesk.Api.Employee.Models;

public record IssueCreateResponseModel
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Description { get; init; } = string.Empty;
    public IssueImpact Impact { get; init; }
    public IssueImpactRadius ImpactRadius { get; init; }
    public Guid SoftwareId { get; set; }

    public string? SoftwareTitle { get; set; } = null;
    public string? SoftwareVendor { get; set; } = null;
    public string? SoftwareMessage { get; set; } = null;
    public string? VipStatus { get; set; } = null;

    public int AssignedPriority
    {
        get
        {
            var startingPriority = 0;
            if (Impact == IssueImpact.WorkStoppage)
            {
                startingPriority += 50;
            }

            if (ImpactRadius == IssueImpactRadius.Customer)
            {
                startingPriority += 50;
            }

            if (VipStatus != null)
            {
                startingPriority += 500;
            }
            return startingPriority;
        }
    }

    public Guid SubmittedBy { get; init; } 

    public IssueContactMechanism ContactMechanisms { get; init; } = new();
    public IssueContactPreferences ContactPreferences { get; init; }

    public IssueStatus Status
    {
        get
        {
            if ((SoftwareTitle != null && SoftwareVendor != null) || SoftwareMessage != null && VipStatus != null)
            {
                return IssueStatus.AwaitingVerification;
            }
            else
            {
                return IssueStatus.AwaitingVerification;
            }
        }
    }

    public static IssueCreateResponseModel Create(EmployeeSubmittedIssue @event)
    {
        var issue = new IssueCreateResponseModel
        {
            Id = @event.Issue.Id,
            ContactMechanisms = @event.Issue.ContactMechanisms,
            ContactPreferences = @event.Issue.ContactPreferences,
         
            Description = @event.Issue.Description,
            Impact = @event.Issue.Impact,
            ImpactRadius = @event.Issue.ImpactRadius,
            SoftwareId = @event.Issue.SoftwareId,
            SubmittedBy = @event.Issue.SubmittedBy,
           
           
        };
       
        return issue;
    }

    private static int GetPriority(IssueCreateResponseModel model)
    {
        var startingPriority = 0;
        if (model.Impact == IssueImpact.WorkStoppage)
        {
            startingPriority += 50;
        }

        if (model.ImpactRadius == IssueImpactRadius.Customer)
        {
            startingPriority += 50;
        }

        if (model.VipStatus != null)
        {
            startingPriority += 500;
        }
        return startingPriority;
    }
   
    public static IssueCreateResponseModel Apply(VipIssueReported @event, IssueCreateResponseModel model)
    {
        return model with { VipStatus = "Is Vip", };
    }

    public static IssueCreateResponseModel Apply(SupportedSoftwareReported @event,  IssueCreateResponseModel model)
    {
        return model with {  SoftwareTitle = @event.Title, SoftwareVendor = @event.Vendor };
    }
    public static IssueCreateResponseModel Apply(UnsupportedSoftwareReported @event, IssueCreateResponseModel model)
    {
        return model with { SoftwareMessage = "Unsupported Software" };
    }
}
