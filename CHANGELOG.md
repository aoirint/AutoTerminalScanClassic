# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` is derived from
stable entries in this file and rewritten for users.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

### Added

- Added a canonical developer changelog so future release preparation can keep
  maintainer-facing migration, compatibility, build, and validation context
  separate from Thunderstore-facing release notes.

### Changed

- Documented the staged CJP/SDC `v0.2.x` migration policy for development,
  quality checks, dependency updates, release automation, and Thunderstore
  publication.
- Updated package-facing compatibility metadata for the current Lethal Company
  v81.5 dependency baseline.

### Notes

- Compatibility:
    - The current migration work updates the compile and Thunderstore
      dependency baseline to the same Lethal Company v81.5 package family used
      by current CJP/SDC migration work.
    - Real-game validation has not yet been performed for AutoTerminalScanClassic
      on v81.5 as part of this staged migration.

## v0.1.2 - 2025-11-24 UTC

### Fixed

- Updated README comparison notes.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
    - This compatibility metadata was backfilled while preparing the CJP/SDC
      `v0.2.x` migration plan.

## v0.1.1 - 2025-11-23 UTC

### Added

- Initial Thunderstore release.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
    - This compatibility metadata was backfilled while preparing the CJP/SDC
      `v0.2.x` migration plan.
