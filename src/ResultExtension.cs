using System.Net;
using FluentResults;

namespace IATec.Shared.Bff.FluentResult;

/// <summary>
/// Extension methods for FluentResults to help construct IATec BFF responses with HTTP status codes.
/// </summary>
public static class ResultExtension
{
    private static readonly string[] StatusCodeListOrder = ["503", "500", "429", "400", "404", "403", "401"];

    /// <summary>
    /// Builds a <see cref="Result"/> representing a 404 Not Found error.
    /// </summary>
    /// <returns>A failed <see cref="Result"/> containing a 404 error.</returns>
    public static Result BuildNotFoundError()
    {
        var error = new Error(string.Empty, new Error("404"));
        return Result.Fail(error);
    }

    /// <summary>
    /// Builds a <see cref="Result"/> representing a 503 Service Unavailable error.
    /// </summary>
    /// <returns>A failed <see cref="Result"/> containing a 503 error.</returns>
    public static Result BuildServiceUnavailableError()
    {
        var error = new Error(string.Empty, new Error("503"));
        return Result.Fail(error);
    }

    /// <summary>
    /// Builds a <see cref="Error"/> representing a 400 Bad Request using the provided message.
    /// </summary>
    /// <param name="message">The error message describing what went wrong.</param>
    /// <returns>An <see cref="Error"/> with the message and a 400 status code.</returns>
    public static Error BuildBadRequestError(this string message)
    {
        return new Error(message, new Error("400"));
    }

    /// <summary>
    /// Builds a <see cref="Error"/> representing a 500 Internal Server Error using the provided message.
    /// </summary>
    /// <param name="message">The error message describing what went wrong.</param>
    /// <returns>An <see cref="Error"/> with the message and a 500 status code.</returns>
    public static Error BuildInternalServerError(this string message)
    {
        return new Error(message, new Error("500"));
    }

    /// <summary>
    /// Builds a custom <see cref="Error"/> with the given message and optional HTTP status code.
    /// When <paramref name="statusCode"/> is null or empty, the error is created without a nested status-code reason.
    /// </summary>
    /// <param name="message">The error message describing what went wrong.</param>
    /// <param name="statusCode">The optional HTTP status code to associate with the error.</param>
    /// <returns>An <see cref="Error"/> containing the message and, when provided, the status code.</returns>
    public static Error BuildError(string message, string? statusCode)
    {
        if (string.IsNullOrWhiteSpace(statusCode))
            return new Error(message);

        return new Error(message, new Error(statusCode));
    }

    /// <summary>
    /// Builds a final HTTP response tuple for a generic <see cref="Result{T}"/>.
    /// If the result contains errors, they are ordered by priority HTTP status codes
    /// and the highest-priority status code is returned alongside the failed result.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The FluentResult instance to evaluate.</param>
    /// <returns>
    /// A tuple containing the HTTP status code and the processed <see cref="Result{T}"/>.
    /// </returns>
    public static (int, Result<T>) BuildResultResponse<T>(this Result<T> result)
    {
        if (result.Errors.Count == 0)
            return ((int)HttpStatusCode.OK, result);

        var (errorList, httpStatusCode) = BuildErrorListOrderingFromHttpStatusCode(result);
        AddErrorListWithoutStatusCode(result, errorList);

        return (httpStatusCode, Result.Fail(errorList));
    }

    /// <summary>
    /// Builds a final HTTP response tuple for a non-generic <see cref="Result"/>.
    /// If the result contains errors, they are ordered by priority HTTP status codes
    /// and the highest-priority status code is returned alongside the failed result.
    /// </summary>
    /// <param name="result">The FluentResult instance to evaluate.</param>
    /// <returns>
    /// A tuple containing the HTTP status code and the processed <see cref="Result"/>.
    /// </returns>
    public static (int, Result) BuildResultResponse(this Result result)
    {
        if (result.Errors.Count == 0)
            return ((int)HttpStatusCode.OK, result);

        var (errorList, httpStatusCode) = BuildErrorListOrderingFromHttpStatusCode(result);
        AddErrorListWithoutStatusCode(result, errorList);
        return (httpStatusCode, Result.Fail(errorList));
    }

    /// <summary>
    /// Orders the errors inside a <see cref="ResultBase"/> according to a predefined
    /// HTTP status code priority list and determines the dominant HTTP status code.
    /// </summary>
    /// <param name="result">The result whose errors will be evaluated.</param>
    /// <returns>
    /// A tuple with the ordered error list and the highest-priority HTTP status code found.
    /// </returns>
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

    /// <summary>
    /// Appends errors that do not have any associated HTTP status code
    /// (or have empty reasons) to the provided error list.
    /// </summary>
    /// <param name="result">The result containing the original errors.</param>
    /// <param name="errorList">The list to which status-code-less errors will be added.</param>
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
