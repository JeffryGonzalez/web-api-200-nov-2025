using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Software.Api.Vendors.Models;

namespace Software.Api.Vendors;


[Authorize]
[ApiController] 
public class VendorsController : ControllerBase
{
    [HttpGet("/vendors")]
    public async Task<ActionResult> GetAllVendors(
        [FromServices] ILookupVendors vendorLookup,
        CancellationToken token)
    {

        var vendors  = await vendorLookup.GetAllVendorsAsync(token);
        return Ok(vendors);
    }

    [Authorize(Policy = "SoftwareCenterManager")]
    [HttpPost("/vendors")] // POST to a collection resource. 
    public async Task<ActionResult<VendorDetailsModel>> AddAVendorAsync(
        [FromBody] VendorCreateModel request,
        [FromServices] ICreateVendors vendorCreator,
        [FromServices] IValidator<VendorCreateModel> validator
        )
    {
 
        var validations = await validator.ValidateAsync(request);

        if (!validations.IsValid)
        {
            return BadRequest(validations.Errors); // send a 400 with some error information in it.
        }


        var response = await vendorCreator.CreateVendorAsync(request);

        return Created($"/vendors/{response.Id}", response);
    }


    [HttpGet("/vendors/{id:guid}")]
    public async Task<ActionResult> GetVendorByIdAsync([FromRoute] Guid id,

        [FromServices] ILookupVendors vendorLookup,
        CancellationToken token )
    {

       var response = await vendorLookup.GetVendorByIdAsync(id, token);
        return response switch
        {
            null => NotFound(),
            _ => Ok(response)
        };
    }
}
