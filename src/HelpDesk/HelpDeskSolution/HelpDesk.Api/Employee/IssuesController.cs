using HelpDesk.Api.Employee.Data;
using HelpDesk.Api.Employee.Handlers;
using HelpDesk.Api.Employee.Models;
using HelpDesk.Api.HttpClients;
using HelpDesk.Api.Services;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace HelpDesk.Api.Employee;

public class IssuesController : ControllerBase
{
    //[Authorize] // requires an authorization header with a bearer JWT or return 401
    [HttpPost("/employee/issues")]
    public async Task<ActionResult> ReportAnIssue(
        [FromBody] IssueCreateModel request,
        [FromServices] TimeProvider clock,
        [FromServices] IManageUserIdentity userIdentity,
        [FromServices] IssueCreateModelValidator validator,
        [FromServices] IMessageContext messageBus
 
        // this is an adjustable clock for testing.
        )
    {
       
        var validationResults = await validator.ValidateAsync( request );
        if(!validationResults.IsValid)
        {
            //return BadRequest(); // a 400 - this is what I usually do, but..
            return BadRequest(validationResults.ToDictionary());
        }

       

        var response = new EmployeeIssueReadModel
        {
            Id = Guid.NewGuid(),
            ContactMechanisms = request.ContactMechanisms,
            ContactPreferences = request.ContactPreferences,
            Description = request.Description,
            Impact = request.Impact,
            ImpactRadius = request.ImpactRadius,
            Software = new SoftwareCatalogItem()
            {
                Id = request.SoftwareId
            },
            SubmittedBy = await userIdentity.GetUserIdFromRequestingContextAsync()
        };

        // hand this off to a "background worker" to handle. This controller is busy enough.
        // BUT - it must be durable - it has to save it as is, in case there is a failure, etc.
        // Jimmy Bogard - AutoMapper, Mediatr
        await messageBus.PublishAsync(new ProcessEmployeeIssue(response));
        
        return Created($"/employee/issues/{response.Id}", response);
    }

    [HttpGet("/employee/issues/{id:guid}")]
    public async Task<ActionResult> GetIssueAsync(Guid id,[FromServices] IDocumentSession session)
    {
        var response = await session.Events.AggregateStreamAsync<EmployeeIssueReadModel>(id);
        // Todo: AuthZ - should only be able to retrieve your own issue
        if(response is null)
        {
            return NotFound("Nope - nothing");
        } else
        {
            return Ok(response);
        }
    }

    [HttpGet("/employee/issues/")]
    public async Task<ActionResult> GetIssuesAsync([FromServices] IDocumentSession session,
        [FromServices] IManageUserIdentity userIdentity)
    {
        var userId = await userIdentity.GetUserIdFromRequestingContextAsync();
        var issues = await session.Query<EmployeeIssueReadModel>()
            .Where(i => i.SubmittedBy == userId)
            .ToListAsync();
        return Ok(issues);
    }

    [HttpGet("/issues-awaiting-tech-assignment")]
    public async Task<ActionResult> GetIssueAwaitingTechAssignmentAsync([FromServices] IDocumentSession session)
    {
        var issues = await session.Query<EmployeeIssueReadModel>()
            .Where(issue => issue.Status == IssueStatus.AwaitingTechAssignment)
            .OrderBy(issue => issue.AssignedPriority)
            .ToListAsync();
        return Ok(issues);
    }
}
