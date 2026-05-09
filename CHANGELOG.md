# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` should be
derived from stable entries in this file and rewritten for users when preparing
future stable releases.

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
- Updated package-facing compatibility documentation for the current Lethal
  Company v81.5 dependency baseline.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - The v81.5 test environment used BepInExPack v5.4.2305.
- Older Lethal Company versions are no longer claimed as tested by the current
  v0.2.0 release notes; Lethal Company v73 compatibility is recorded in the
  historical `v0.1.x` release entries below.

## v0.1.2 - 2025-11-24 UTC

### Fixed

- Updated README.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.1 - 2025-11-23 UTC

### Added

- Initial Thunderstore release.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.
