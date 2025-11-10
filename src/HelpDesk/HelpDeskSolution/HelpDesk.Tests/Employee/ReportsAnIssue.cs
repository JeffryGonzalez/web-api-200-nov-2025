

using HelpDesk.Tests.Fixtures;

namespace HelpDesk.Tests.Employee;

[Collection("AuthenticatedSystemTestFixture")]
[Trait("Category", "SystemTest")]
public class ReportsAnIssue(AuthenticatedSystemTestFixture fixture)
{
   

    [Fact]
    public async Task ReportingAnIssue()
    {
        await fixture.Host.Scenario(api =>
        {
            api.Post.Url("/employee/issues");
            api.StatusCodeShouldBe(201);
        });
    }
}
