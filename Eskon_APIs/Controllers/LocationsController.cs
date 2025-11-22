using Eskon_APIs.Contracts.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eskon_APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var locationResult = await _locationService.GetAsync(id, cancellationToken);

        return locationResult.IsSuccess ? Ok(locationResult.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: locationResult.Error.Code, detail: locationResult.Error.Description);
    }

    [HttpPost("")]
    [Authorize(Roles = "Admin, Member")]
    public async Task<IActionResult> Add([FromBody] LocationRequest locationRequest, CancellationToken cancellationToken)
    {
        var addedLocationResult = await _locationService.AddAsync(locationRequest, cancellationToken);

        return addedLocationResult.IsSuccess 
            ? CreatedAtAction(nameof(Get), new {id = addedLocationResult.Value.LocationId}, addedLocationResult.Value)
            : Problem(statusCode: StatusCodes.Status400BadRequest, title: addedLocationResult.Error.Code, detail: addedLocationResult.Error.Description);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, LocationRequest locationRequest, CancellationToken cancellationToken)
    {
        var updateResult = await _locationService.UpdateAsync(id, locationRequest, cancellationToken);

         return updateResult.IsSuccess ? NoContent() 
            : Problem(statusCode: 404, title: updateResult.Error.Code, detail: updateResult.Error.Description);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _locationService.DeleteAsync(id, cancellationToken);
        return deleteResult.IsSuccess ? NoContent() :
            Problem(statusCode: 404, title: deleteResult.Error.Code, detail: deleteResult.Error.Description);
    }
}
