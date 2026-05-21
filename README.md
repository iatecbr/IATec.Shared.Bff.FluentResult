# IATec.Shared.Bff.FluentResult

A .NET library that extends [FluentResults](https://github.com/altmann/FluentResults) to streamline building HTTP-friendly responses in Backend-for-Frontends (BFFs) across IATec projects.

## Features

- **Pre-built HTTP error helpers**: Quickly create `Result` and `Error` objects representing common HTTP status codes (400, 404, 500, 503, etc.).
- **Automatic status-code extraction**: Given a `Result` or `Result<T>` with multiple errors, determines the most appropriate HTTP status code to return based on a configurable priority list.
- **Seamless FluentResults integration**: Works on top of the existing `FluentResults` package without replacing it.

## Supported Error Builders

| Method | HTTP Code | Description |
|--------|-----------|-------------|
| `BuildNotFoundError()` | 404 | Creates a failed `Result` representing Not Found. |
| `BuildServiceUnavailableError()` | 503 | Creates a failed `Result` representing Service Unavailable. |
| `"message".BuildBadRequestError()` | 400 | Creates an `Error` representing Bad Request. |
| `"message".BuildInternalServerError()` | 500 | Creates an `Error` representing Internal Server Error. |
| `BuildError(message, statusCode)` | Custom | Creates an `Error` with an arbitrary status code. |

## Response Builder

Use `BuildResultResponse<T>()` or `BuildResultResponse()` to evaluate a `Result` and automatically derive the dominant HTTP status code:

```csharp
var result = Result.Fail("Not Found".BuildNotFoundError());
(int statusCode, Result response) = result.BuildResultResponse();
// statusCode == 404
```

When multiple errors are present, the library applies the following priority order to choose the HTTP status code:

`503 → 500 → 429 → 400 → 404 → 403 → 401`

## Requirements

- .NET 9.0 or .NET 10.0
- Package reference: `FluentResults` (v4.0.0)

## Installation

```bash
dotnet add package IATec.Shared.Bff.FluentResult
```

## Changelog

See the [CHANGELOG](CHANGELOG.md) for a history of notable changes and releases.

## Contribution

This project is maintained by the **IATec Platform Team**. Contributions follow the internal IATec open-source guidelines.

## License

© IATec Solutions
