using Eskon.Api.Extensions;
using Eskon.Application.Contracts.Review;
using Eskon.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eskon.Api.Controllers;

/// <summary>
/// Manages review operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    /// <summary>
    /// Gets all reviews written by the current authenticated user.
    /// </summary>
    /// <response code="200">Returns the user's reviews.</response>
    /// <response code="401">Unauthorized.</response>
    [HttpGet("my-reviews")]
    [Authorize]
    public async Task<IActionResult> GetMyReviews(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await reviewService.GetMyReviewsAsync(userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Gets all reviews for a specific house.
    /// </summary>
    /// <response code="200">Returns the house reviews.</response>
    /// <response code="404">House not found.</response>
    [HttpGet("house/{houseId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHouseReviews(int houseId, CancellationToken cancellationToken)
    {
        var result = await reviewService.GetHouseReviewsAsync(houseId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Creates a review for a house.
    /// </summary>
    /// <response code="201">Review created successfully.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="409">Already reviewed or own house.</response>
    [HttpPost("house/{houseId:int}")]
    [Authorize]
    public async Task<IActionResult> Create(int houseId, [FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await reviewService.CreateAsync(userId, houseId, request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetHouseReviews), new { houseId }, result.Value)
            : result.ToProblem();
    }

    /// <summary>
    /// Updates an existing review.
    /// </summary>
    /// <response code="204">Updated successfully.</response>
    /// <response code="403">Not the review owner.</response>
    /// <response code="404">Review not found.</response>
    [HttpPut("{reviewId:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int reviewId, [FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await reviewService.UpdateAsync(userId, reviewId, request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    /// <summary>
    /// Deletes a review.
    /// </summary>
    /// <response code="204">Deleted successfully.</response>
    /// <response code="403">Not the review owner.</response>
    /// <response code="404">Review not found.</response>
    [HttpDelete("{reviewId:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int reviewId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await reviewService.DeleteAsync(userId, reviewId, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
