using System.Net;
using FluentResults;

namespace IATec.Shared.Bff.FluentResult;

public static class ResultExtension
{
    private static readonly string[] StatusCodeListOrder = ["503", "500", "429", "400", "404", "403", "401"];

    public static Result BuildNotFoundError()
    {
        var error = new Error(string.Empty, new Error("404"));
        return Result.Fail(error);
    }

    public static Error BuildBadRequestError(this string message)
    {
        return new Error(message, new Error("400"));
    }

    public static Error BuildInternalServerError(this string message)
    {
        return new Error(message, new Error("500"));
    }

    public static Error BuildError(string message, string? statusCode)
    {
        return new Error(message, new Error(statusCode));
    }

    public static (int, Result<T>) BuildResultResponse<T>(this Result<T> result)
    {
        if (result.Errors.Count == 0)
            return ((int)HttpStatusCode.OK, result);

        var (errorList, httpStatusCode) = BuildErrorListOrderingFromHttpStatusCode(result);
        AddErrorListWithoutStatusCode(result, errorList);

        return (httpStatusCode, Result.Fail(errorList));
    }

    public static (int, Result) BuildResultResponse(this Result result)
    {
        if (result.Errors.Count == 0)
            return ((int)HttpStatusCode.OK, result);

        var (errorList, httpStatusCode) = BuildErrorListOrderingFromHttpStatusCode(result);
        AddErrorListWithoutStatusCode(result, errorList);
        return (httpStatusCode, Result.Fail(errorList));
    }

    private static (List<IError>, int) BuildErrorListOrderingFromHttpStatusCode(ResultBase result)
    {
        var httpStatusCode = (int)HttpStatusCode.BadRequest;
        var errorList = new List<IError>();
        foreach (var statusCode in StatusCodeListOrder)
        {
            var errors = result.Errors
                .Where(r => r.Reasons
                    .Select(m => m.Message)
                    .Contains(statusCode))
                .ToList();

            if (errors.Count <= 0)
                continue;

            //First error found
            if (errorList.Count == 0)
                httpStatusCode = int.Parse(statusCode);

            errorList.AddRange(errors);
        }
        return (errorList, httpStatusCode);
    }

    private static void AddErrorListWithoutStatusCode(ResultBase result, List<IError> errorList)
    {
        var errorsWithoutStatusCode = result.Errors
            .Where(r => r.Reasons.Select(m => m.Message).Contains(null)
                        || r.Reasons.Count == 0)
            .ToList();

        if (errorsWithoutStatusCode.Count > 0)
            errorList.AddRange(errorsWithoutStatusCode);
    }
}