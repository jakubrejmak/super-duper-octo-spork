using Microsoft.AspNetCore.Diagnostics;
using Parser.Api.Exceptions;

namespace Parser.Api.Middleware;

public class ParseExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not ParseException parseException)
            return false;

        context.Response.StatusCode = 400;

        await Results
            .Problem(
                title: "Parsing failed",
                detail: parseException.Message,
                statusCode: StatusCodes.Status400BadRequest
            )
            .ExecuteAsync(context);

        return true;
    }
}
