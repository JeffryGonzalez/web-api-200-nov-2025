using HelpDesk.Api.Employee.Data;
using HelpDesk.Api.Employee.Models;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Employee;

public class IssuesController : ControllerBase
{
    [Authorize] // requires an authorization header with a bearer JWT or return 401
    [HttpPost("/employee/issues")]
    public async Task<ActionResult> ReportAnIssue(
        [FromBody] IssueCreateModel request,
        [FromServices] IDocumentSession session,
        [FromServices] TimeProvider clock // this is an adjustable clock for testing.
        )
    {
        // Transaction List - Fowler
        // 0. Only employees that are identified by our IDP can do this.
        //    -- their identity should be in a JWT bearer token in then authorization header.
        //    -- if they have not been identified, we should send a 401 response (Unauthorized)
        // 1. look at the data they sent in the body of the request
        // 2. Validate it - have some rules, enforce them, if it is bad, send them a 400 Bad Request
        // 3. We have to check to see if this is supported software
        // 4. Assign it to a tech
        // 5. See if it is a VIP, etc.
        // if a request takes longer than about ~100ms on my local machine, it's too much.
        var entityToSave = new IssueEntity
        {
            Id = Guid.NewGuid(),
            Status = IssueStatus.AwaitingVerification,
            SubmittedAt = clock.GetUtcNow(), // Fix
            SubmittedBy = User.Identity.Name, // we will do something else with this tomorrow.
            SubmittedIssue = new SubmittedIssue
            {
                SoftwareId = request.SoftwareId,
                ContactMechanisms = request.ContactMechanisms,
                ContactPreferences = request.ContactPreferences,
                Description = request.Description,
                Impact = request.Impact,
                ImpactRadius = request.ImpactRadius

            }
        };
        session.Store(entityToSave); // whatever code you have to write to store it.
        await session.SaveChangesAsync();

        return Created();
    }
}
