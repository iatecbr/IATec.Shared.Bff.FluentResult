# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [1.2.0] - 2026-05-18

### ADDED
- Added support for .NET 10.0 alongside existing .NET 9.0 target framework.
- Added new `BuildServiceUnavailableError` method to support 503 Service Unavailable responses.
- Added comprehensive XML documentation summaries to all public classes and methods for improved IntelliSense support.
- Introduced `CHANGELOG.md` to track project changes.

## [1.1.0] - 2026-05-18

### ADDED
- Added custom error builder for HTTP 503 (Service Unavailable) status code.

### CHANGED
- Improved internal error ordering logic for more accurate HTTP status code extraction.

## [1.0.0] - 2026-05-18

### ADDED
- Initial stable release of `IATec.Shared.Bff.FluentResult`.
- Added `ResultExtension` static class providing helper methods for FluentResults integration.
- Added `BuildNotFoundError` method for HTTP 404 responses.
- Added `BuildBadRequestError` method for HTTP 400 responses.
- Added `BuildInternalServerError` method for HTTP 500 responses.
- Added `BuildError` generic method for custom status code responses.
- Added `BuildResultResponse<T>` and `BuildResultResponse` methods to automatically map FluentErrors to appropriate HTTP status codes.
- Implemented internal prioritization logic for multiple errors based on HTTP status codes (503, 500, 429, 400, 404, 403, 401).

## [0.1.0] - 2026-05-18

### ADDED
- Project initialization and repository setup.
- Core DLL structure and package metadata configuration.
- Integration with `FluentResults` library (v4.0.0).

[1.2.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/releases/tag/v1.0.0
