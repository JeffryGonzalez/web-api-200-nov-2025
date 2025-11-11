using HelpDesk.Api.Employee.Models;

namespace HelpDesk.Api.Employee.Data;
/*
{
    "id": "{{issueId}}"
    "softwareId": "some-id-from-the-software-team",
    "description": "long form description of the issue",
    "impact": "WorkStoppage",
    "impactRadius": "Customer",
    "contactMechanisms": {
        "email": "jeff@company.com",
        "phone": "555-1212"
    },
    "contactPreference": "Email",
    "submittedBy": "name in token",
    "submittedAt": "date time offset of when it was submitted",
    "status": "AwaitingVerification" | "AwaitingTechAssignment" | "ElevatedToVipManager" | ... 

}*/

public enum IssueStatus {  AwaitingVerification, Verified }
public class IssueEntity
{
    public Guid Id { get; set; }
    // this is "embedded data"
    public SubmittedIssue SubmittedIssue { get; set; } = new();
    public Guid SubmittedBy { get; set; }
    public DateTimeOffset SubmittedAt { get; set; }
    public IssueStatus Status { get; set; }


}

public class SubmittedIssue
{
    public Guid SoftwareId { get; init; }
    public string Description { get; init; } = string.Empty;
    public IssueImpact Impact { get; init; }
    public IssueImpactRadius ImpactRadius { get; init; }

    public IssueContactMechanism ContactMechanisms { get; init; } = new();
    public IssueContactPreferences ContactPreferences { get; init; }
}
