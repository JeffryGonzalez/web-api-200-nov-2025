namespace HelpDesk.Api.Employee.Models;


/*{
    "sofwareId": "some-id-from-the-software-team",
    "description": "long form description of the issue",
    "impact": "Inconvenience",
    "impactRadius": "Personal",
    "contactMechanisms": {
        "email": "jeff@company.com",
        "phone": "555-1212"
    },
    "contactPreference": "Email"
}*/
// if VIP adds 500 to the priority
public enum IssueImpact {  Inconvenience, WorkStoppage } // WorkStoppage adds "50" to the priority
public enum IssueImpactRadius {  Personal, Customer} // Customer Impact adds 100 to the priority
public enum IssueContactPreferences { Email, Phone }

public record IssueCreateModel
{
    public Guid SoftwareId { get; init; }
    public string Description { get; init; } = string.Empty;
    public IssueImpact Impact { get; init; }  
    public IssueImpactRadius ImpactRadius { get; init; }

    public IssueContactMechanism ContactMechanisms { get; init; } = new();
    public IssueContactPreferences ContactPreferences { get; init; }

}

public record IssueContactMechanism
{
    public string? Email { get; init; }
    public string? Phone { get; init; }
}