using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Employee;

public class IssuesController : ControllerBase
{
    [HttpPost("/employee/issues")]
    public async Task<ActionResult> ReportAnIssue()
    {
        return Created();
    }
}
