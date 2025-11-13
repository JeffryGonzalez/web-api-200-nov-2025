using HelpDesk.Vips.Api.Management.Models;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Vips.Api.Management;

// [Authorize(Role="HelpDeskVipManagers")]
[ApiController]
public class ManagementController(IDocumentSession session, TimeProvider clock) : ControllerBase
{
    [HttpGet("/management/vips")]
    public async Task<ActionResult<IList<VipReadModel>>> GetAllVips()
    {
        var vips = await session.Query<VipReadModel>()
            .ToListAsync();
        return Ok(vips);
    }

    [HttpGet("/management/inactive-vips")]
    public async Task<ActionResult<IList<VipInactiveReadModel>>> GetAllInactiveVips()
    {
        var response = await session.Query<VipInactiveReadModel>().ToListAsync();
        return Ok(response);
    }

    [HttpPost("/management/vips")]
    public async Task<ActionResult<VipReadModel>> AddVipAsync([FromBody] VipCreateModel request)
    {

       var id = Guid.NewGuid();
         session.Events.Append(id, new VipAdded(id, request));
        await session.SaveChangesAsync();
        var entity = new VipReadModel
        {
            Id = id,
            Created = clock.GetUtcNow(),
            Reason = request.Reason,
            UserSubject = request.UserSubject,
        };
       

        return Ok(entity);
    }

    [HttpGet("/management/vips/{id:guid}")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 30)]
    // Distributed Caching in .NET - make the HTTP client, and even database calls cacheable
    public async Task<ActionResult<VipReadModel>> GetVipAsync(Guid id)
    {
        var response = await session.LoadAsync<VipReadModel>(id);
        if(response is not null)
        {
            return Ok(response);
        }
        return NotFound();
    }

    [HttpDelete("/management/vips/{id:guid}")]
    public async Task<ActionResult> DeleteVipAsync(Guid id, [FromBody] VipReadModel request)
    {
        var savedVip = await session.LoadAsync<VipReadModel>(id);
        if(savedVip is null)
        {
            return NoContent(); // you wanted it gone, it is. You are welcome.
        }
        if (savedVip.Version != request.Version)
        {
            return Conflict(new { Message = "That Vip Has Been Updated. Reload" });
        }
        session.Events.Append(id, new VipDeactivated(savedVip));
        await session.SaveChangesAsync();
        return NoContent();
    }
    
}