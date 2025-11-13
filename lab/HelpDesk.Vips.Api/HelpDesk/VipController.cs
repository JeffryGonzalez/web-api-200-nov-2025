using HelpDesk.Common.Vips;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Vips.Api.HelpDesk;

// [Authorize(Role="HelpDeskApi")]
[ApiController]
public class VipController : ControllerBase
{
    [HttpPost("/vip-check")]
    public async Task<ActionResult<VipResponseMessage>> CheckForVipAsync(VipRequestMessage request, [FromServices] IDocumentSession session)
    {
        return Ok(new VipResponseMessage() { IsVip = true, UserSubject = request.UserSubject });
    }
}