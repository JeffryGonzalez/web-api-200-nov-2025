

using HelpDesk.Api.Employee.Models;
using HelpDesk.Tests.Fixtures;

namespace HelpDesk.Tests.Employee;

[Collection("AuthenticatedSystemTestFixture")]
[Trait("Category", "SystemTest")]
public class ReportsAnIssue(AuthenticatedSystemTestFixture fixture)
{
   

    [Fact]
    public async Task ReportingAnIssue()
    {
        var issueToReport = new IssueCreateModel
        {
            SoftwareId = Guid.NewGuid(),
            Description = "Dang thing won't work!",
            Impact = IssueImpact.Inconvenience,
            ImpactRadius = IssueImpactRadius.Personal,
            ContactMechanisms = new IssueContactMechanism
            {
                Email = "user@company.com",
                Phone = "444-1212"
            },
            ContactPreferences = IssueContactPreferences.Email
        };
        await fixture.Host.Scenario(api =>
        {
            api.Post.Json(issueToReport).ToUrl("/employee/issues");
            api.StatusCodeShouldBe(201);
        });
    }
}
