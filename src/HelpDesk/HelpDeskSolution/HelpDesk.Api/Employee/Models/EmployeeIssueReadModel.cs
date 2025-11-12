using HelpDesk.Api.Employee.Data;
using HelpDesk.Api.HttpClients;

namespace HelpDesk.Api.Employee.Models;

public record EmployeeIssueReadModel
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Description { get; init; } = string.Empty;
    public IssueImpact Impact { get; init; }
    public IssueImpactRadius ImpactRadius { get; init; }

    public SoftwareCatalogItem Software { get; set; } = new();
    
    public string? SoftwareMessage { get; set; } = null;
    public string? VipStatus { get; set; } = null;
    public bool SoftwareChecked { get; set; }
    public bool VipStatusChecked { get; set; }

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

            if (VipStatusChecked)
            {
                startingPriority += 500;
            }
            
            return startingPriority;
        }
    }

    public Guid SubmittedBy { get; init; } 

    public IssueContactMechanism ContactMechanisms { get; init; } = new();
    public IssueContactPreferences ContactPreferences { get; init; }

    public IssueStatus Status { get; set; } = IssueStatus.AwaitingVerification;

   

    private static int GetPriority(EmployeeIssueReadModel model)
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
   
}