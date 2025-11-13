

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
        var postResponse = await fixture.Host.Scenario(api =>
        {
            api.Post.Json(issueToReport).ToUrl("/employee/issues");
            api.StatusCodeShouldBe(201);
        });

        var location = postResponse.Context.Response.Headers.Location.First();

        var postBody = postResponse.ReadAsJson<EmployeeIssueReadModel>();
        Assert.NotNull(postBody);

        // It has to process the issue, it has to check the software, it has to check the vip status, etc.
        var getResponse = await fixture.Host.Scenario(api =>
        {
            api.Get.Url(location);
            api.StatusCodeShouldBe(200);
        });

        var getBody = getResponse.ReadAsJson<EmployeeIssueReadModel>();

        Assert.Equal(postBody, getBody);



        // A little hokey
    }
}
