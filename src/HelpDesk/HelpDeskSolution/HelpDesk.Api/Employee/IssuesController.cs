using HelpDesk.Api.Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Employee;

public class IssuesController : ControllerBase
{
    [Authorize] // requires an authorization header with a bearer JWT or return 401
    [HttpPost("/employee/issues")]
    public async Task<ActionResult> ReportAnIssue(
        [FromBody] IssueCreateModel request
        )
    {
        // Transaction List - Fowler
        // 0. Only employees that are identified by our IDP can do this.
        //    -- their identity should be in a JWT bearer token in then authorization header.
        //    -- if they have not been identified, we should send a 401 response (Unauthorized)
        // 1. look at the data they sent in the body of the request
        // 2. Validate it - have some rules, enforce them, if it is bad, send them a 400 Bad Request
        // 3. We have
        return Created();
    }
}
