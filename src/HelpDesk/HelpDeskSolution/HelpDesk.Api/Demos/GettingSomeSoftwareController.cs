using HelpDesk.Api.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Demos;

public class GettingSomeSoftwareController(ILookupSoftwareFromTheSoftwareApi softwareCenterApi) : ControllerBase
{
    // GET /demos/software/tacos
    [HttpGet("/demos/software/{id:guid}")]
    public async Task<ActionResult> GetInfoAboutSoftware(Guid id)
    {
        var response = await softwareCenterApi.ValidateSoftwareItemFromCatalogAsync(id);
        if(response is null)
        {
            return Ok(new { message = "Sorry, no Software with that id" });
        } else
        {
            return Ok(response);
        }
    }
}

