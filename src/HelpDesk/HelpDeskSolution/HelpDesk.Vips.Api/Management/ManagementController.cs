using HelpDesk.Vips.Api.Management.Models;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Vips.Api.Management;

// [Authorize(Role="HelpDeskVipManagers")]
[ApiController]
public class ManagementController(IDocumentSession session) : ControllerBase
{
    [HttpGet("/management/vips")]
    public async Task<ActionResult<IList<VipReadModel>>> GetAllVips()
    {
        return NoContent();
    }

    [HttpGet("/management/inactive-vips")]
    public async Task<ActionResult<IList<VipInactiveReadModel>>> GetAllInactiveVips()
    {
        return NoContent();
    }

    [HttpPost("/management/vips")]
    public async Task<ActionResult<VipReadModel>> AddVipAsync([FromBody] VipCreateModel request)
    {
        return NoContent();
    }

    [HttpGet("/management/vips/{id:guid}")]
    public async Task<ActionResult<VipReadModel>> GetVipAsync(Guid id)
    {
        return NoContent();
    }

    [HttpDelete("/management/vips/{id:guid}")]
    public async Task<ActionResult> DeleteVipAsync(Guid id, [FromBody] VipCreateModel request)
    {
        return NoContent();
    }
    
}