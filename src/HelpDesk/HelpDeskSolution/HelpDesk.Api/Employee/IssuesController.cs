using HelpDesk.Api.Employee.Data;
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

       

        var response = new IssueCreateResponseModel
        {
            Id = Guid.NewGuid(),
            ContactMechanisms = request.ContactMechanisms,
            ContactPreferences = request.ContactPreferences,
            Description = request.Description,
            Impact = request.Impact,
            ImpactRadius = request.ImpactRadius,
            SoftwareId = request.SoftwareId,
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
        var response = await session.Events.AggregateStreamAsync<IssueCreateResponseModel>(id);
        if(response is null)
        {
            return NotFound("Nope - nothing");
        } else
        {
            return Ok(response);
        }
    }
}
