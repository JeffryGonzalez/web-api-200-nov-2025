
using HelpDesk.Tests.Fixtures;

namespace HelpDesk.Tests.Employee;

[Collection("AnonymousSystemTestFixture")]
[Trait("Category", "SystemTest")]
public class MustBeAuthenticatedToReportAnIssue(AnonymousSystemTestFixture fixture)
{

    [Fact]
    public async Task GivesA401ForNoToken()
    {
        await fixture.Host.Scenario(api =>
        {
            api.Post.Url("/employee/issues");
            api.StatusCodeShouldBe(401);
        });
    }
    
}
