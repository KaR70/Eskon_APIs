using Eskon_APIs.Contracts.Amenity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eskon_APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AmenitiesController : ControllerBase
{
    private readonly IAmenityService _amenityService;

    public AmenitiesController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var amenties = await _amenityService.GetAllAsync(cancellationToken);

        return Ok(amenties);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var amenityResult = await _amenityService.GetAsync(id, cancellationToken);
        
        return amenityResult.IsSuccess ? Ok(amenityResult.Value) : 
            Problem(statusCode: StatusCodes.Status404NotFound, title: amenityResult.Error.Code, detail: amenityResult.Error.Description);
    }

    [HttpPost("")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add([FromBody] AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var addedAmenity = await _amenityService.AddAsync(amenityRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = addedAmenity.AmenityId }, addedAmenity);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _amenityService.DeleteAsync(id, cancellationToken);

        return deleteResult.IsSuccess ? NoContent() : 
            Problem(statusCode: StatusCodes.Status404NotFound, title: deleteResult.Error.Code, detail: deleteResult.Error.Description);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AmenityRequest amenityRequest, CancellationToken cancellationToken = default)
    {
        var updateResult = await _amenityService.UpdateAsync(id, amenityRequest, cancellationToken);

        return updateResult.IsSuccess ? NoContent() : 
            Problem(statusCode: StatusCodes.Status404NotFound, title: updateResult.Error.Code, detail: updateResult.Error.Description);
    }
}
