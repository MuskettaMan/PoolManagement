# Changelog

All notable changes to thing package will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) and this package adheres to [Semantic Versioning](https://semver.org/)

## [v2.0.0] - 2021-08-14

### Added

- **MAJOR** - Namespace directives for all the scripts under `Musketta.Poolmanagement`.
- **MINOR** - New component icon for `GameObjectPoolBehaviour`.
- **MINOR** - Documentation MD under `/Documentation~/DOCS.md`.
- **MINOR** - Samples for the `ComponentObjectPoolBehaviour` and `PoolableComponentObjectPoolBehaviour`.

### Changed

- **MAJOR** - In the `ObjectPoolBehaviour` the `RequestObject` and `ReturnObject` methods are now `virtual`.

### Removed

- **PATCH** - Cleaned up unnecessary `using` statements.

### Fixed

- **PATCH** - Added `using` statement in editor create file to fix the initial compile error when creating an editor file.
- **PATCH** - Fixed the null reference exception when opening the 'Create Editor Script' context menu.
- **PATCH** - Fixed the import of the namespace for the generic argument.

## [v2.0.1] - 2021-08-15

*Nothing changed, incrementing version to figure out Github Actions.*

## [v2.0.2] - 2021-08-15

### Fixed

- **PATCH** - Updated the registry in the Github action file to correctly publish.

## [v2.0.3] - 2021-08-15

### Fixed

- **PATCH** - Updated the registry in the Github action file to correctly publish.
