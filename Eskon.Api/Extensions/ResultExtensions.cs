using Eskon.Domain.Abstraction;

namespace Eskon.Api.Extensions;
public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        // if (result.IsSuccess)
        //     throw new InvalidOperationException("Cannot create a problem from a successful result.");
        //
        // var problem = Results.Problem(statusCode: result.Error.StatusCode);
        // var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
        //
        // problemDetails!.Extensions = new Dictionary<string, object?>
        //  {
        //      {
        //          "errors", new[]{ 
        //              result.Error.Code,
        //                 result.Error.Description
        //          }
        //      }
        //  };
        // return new ObjectResult(problemDetails);
        
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot create a problem from a successful result.");

        
        var statusCode = result.Error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.BadRequest => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = result.Error.Code,
            Detail = result.Error.Description
        };

        
        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }
}
