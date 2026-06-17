# Contributing

Keep this package as a narrow integration between Object Loading and API.

## Design Rules

- Keep core Object Loading independent from API.
- Do not depend on Session.
- Do not add backend URL resolving.
- Do not add backend DTOs.
- Do not add glTF, Addressables, caching, or material remapping.
- Keep auth token passing explicit.
- Add tests for mapper behavior and error handling.

## Validation

Before publishing changes:

1. Run EditMode tests for `Deucarian.ObjectLoading.APIIntegration.Tests`.
2. Validate the package in a Unity project that has Object Loading and API available.
