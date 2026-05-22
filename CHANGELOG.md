# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [1.3.0] - 2026-05-21

### ADDED
- Added comprehensive XML documentation summaries (`<summary>`) to all public and private classes and methods in `ResultExtension` for improved IntelliSense support.
- Added `CHANGELOG.md` to track project changes.
- Added link to `CHANGELOG.md` in `README.md`.

### CHANGED
- Rewrote `README.md` with a complete usage guide, feature descriptions and practical code examples.

## [1.2.0] - 2026-01-09

### ADDED
- Added multi-target framework support for `.NET 10.0` alongside existing `.NET 9.0`.

## [1.1.0] - 2025-09-12

### ADDED
- Added `BuildServiceUnavailableError()` method to support HTTP 503 Service Unavailable responses.

## [1.0.0] - 2025-08-11

### ADDED
- Initial stable release of `IATec.Shared.Bff.FluentResult`.
- Added `ResultExtension` static class providing helper methods for FluentResults integration.
- Added `BuildNotFoundError()` method for HTTP 404 responses.
- Added `BuildBadRequestError()` method for HTTP 400 responses.
- Added `BuildInternalServerError()` method for HTTP 500 responses.
- Added `BuildError()` generic method for custom status code responses.
- Added `BuildResultResponse<T>()` and `BuildResultResponse()` methods to automatically map FluentErrors to appropriate HTTP status codes.
- Implemented internal prioritization logic for multiple errors based on HTTP status codes (503, 500, 429, 400, 404, 403, 401).

## [0.1.0] - 2025-06-16

### ADDED
- Project initialization, repository setup and core DLL structure.
- Integration with the `FluentResults` library (v3.x / v4.0.0).

[1.3.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/compare/v1.2.0...v1.3.0
[1.2.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/iatecbr/IATec.Shared.Bff.FluentResult/releases/tag/v1.0.0
