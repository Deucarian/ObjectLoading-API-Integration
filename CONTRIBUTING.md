# Contributing

Keep this package as a narrow bridge between Object Loading and API Helper.

## Design Rules

- Keep core Object Loading independent from API Helper.
- Do not depend on Session Helper.
- Do not add backend URL resolving.
- Do not add backend DTOs.
- Do not add glTF, Addressables, caching, or material remapping.
- Keep auth token passing explicit.
- Add tests for mapper behavior and error handling.

## Validation

Before publishing changes:

1. Run EditMode tests for `JorisHoef.ObjectLoading.APIHelperBridge.Tests`.
2. Validate the package in a Unity project that has Object Loading and API Helper available.
