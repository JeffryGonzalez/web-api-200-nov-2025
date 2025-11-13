using HelpDesk.Common.Vips;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Vips.Api.HelpDesk;

// [Authorize(Role="HelpDeskApi")]
public class VipController : ControllerBase
{
    [HttpPost("/vip-check")]
    public async Task<ActionResult> CheckForVipAsync(VipRequestMessage request, [FromServices] IDocumentSession session)
    {
        return Ok(new VipResponseMessage() { IsVip = true, UserSubject = request.UserSubject });
    }
}