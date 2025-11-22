using Eskon_APIs.Contracts.House;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eskon_APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HouseController : ControllerBase
{
    private readonly IHouseService _houseService;
    public HouseController(IHouseService houseService)
    {
        _houseService = houseService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllHouses(CancellationToken cancellationToken)
    {
        var CurrentUserId = User.Identity?.IsAuthenticated == true
            ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            : null;

        var houses = await _houseService.GetAllAsync(CurrentUserId, cancellationToken);

        return Ok(houses);
    }

    [HttpPost("")]
    [Authorize(Roles = "Member, Admin")]
    public async Task<IActionResult> Create(CreateHouseRequest request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.CreateAsync(request, ownerId, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Get), new { id = result.Value.HouseId }, result.Value);
        }

        return Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _houseService.GetAsync(id, currentUserId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Member, Admin")]
    public async Task<IActionResult> Update(int id, UpdateHouseRequest request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.UpdateAsync(id, request, ownerId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Member, Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized();
        }

        var result = await _houseService.DeleteAsync(id, ownerId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: result.Error.StatusCode, title: result.Error.Code, detail: result.Error.Description);
    }
}
