using Eskon_APIs.Errors;
using Eskon_APIs.Extensions;
using Eskon_APIs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eskon_APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Member,Admin")]
public class SavedListController : ControllerBase
{
    private readonly ISavedListService _savedListService;

    public SavedListController(ISavedListService savedListService)
    {
        _savedListService = savedListService;
    }

    // -------------------------------
    // POST: api/savedlist/{houseId}
    // -------------------------------
    [HttpPost("{houseId:int}")]
    public async Task<IActionResult> Save(int houseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.SaveAsync(userId, houseId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(new { message = "House added to saved list." });
    }

    // -------------------------------
    // DELETE: api/savedlist/{houseId}
    // -------------------------------
    [HttpDelete("{houseId:int}")]
    public async Task<IActionResult> Unsave(int houseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.UnsaveAsync(userId, houseId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(new { message = "House removed from saved list." });
    }

    // -------------------------------
    // GET: api/savedlist/{houseId}
    // -------------------------------
    [HttpGet("{houseId:int}")]
    public async Task<IActionResult> IsSaved(int houseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.IsSavedAsync(userId, houseId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(new { isSaved = result.Value });
    }

    // -------------------------------
    // GET: api/savedlist
    // -------------------------------
    [HttpGet]
    public async Task<IActionResult> GetSavedList(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Problem(SavedListErrors.UserNotFound);

        var result = await _savedListService.GetSavedHousesAsync(userId, cancellationToken);

        if (result.IsFailure)
            return Problem(result.Error);

        return Ok(result.Value);
    }

    // -------------------------------
    // Unified Problem()
    // -------------------------------
    private ObjectResult Problem(Error error)
    {
        return Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: error.StatusCode
        );
    }
}
