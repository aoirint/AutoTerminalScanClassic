# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` should be
derived from stable entries in this file and rewritten for users when preparing
future stable releases.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

## v0.2.0-alpha.1 - 2026-05-09 UTC

### Added

- Added a canonical developer changelog so future release preparation can keep
  maintainer-facing migration, compatibility, build, and validation context
  separate from Thunderstore-facing release notes.
- Added opt-in structured validation logging for release-candidate checks.
  Validation logging is disabled by default and avoids player names, lobby
  identifiers, account identifiers, machine names, profile paths, access or
  session tokens, and raw Unity object details.
- Added Harmony callback diagnostics that log compact exception type metadata
  without allowing diagnostic failures to break base-game callbacks.

### Changed

- Documented repository maintenance guidance for development, quality checks,
  dependency updates, release preparation, and Thunderstore package notes.
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
- Release channel:
    - `v0.2.0-alpha.1` is a GitHub-only prerelease artifact and is not
      published to Thunderstore.
    - `assets/manifest.json` intentionally keeps `version_number` at `0.0.0`
      in the committed package metadata. The release workflow keeps prerelease
      identity in the project version, artifact name, and Git tag while using
      the placeholder manifest version for prerelease and edge artifacts.
    - Thunderstore publication remains gated to stable `latest` releases by
      `.github/workflows/build.yml`.
    - Pull requests now run the Build workflow to create validation artifacts
      before merge, while GitHub Release creation remains limited to
      main-branch pushes.
    - The prerelease build uses the CI-only `BepInExPluginVersion=0.0.0`
      fallback so BepInEx 5 receives loader-compatible plugin metadata without
      committing that fallback to `AutoTerminalScanClassic.csproj`.
- Validation:
    - Real-game alpha validation is tracked separately in #11.
    - The alpha artifact still needs runtime confirmation that BepInEx does not
      emit an AutoTerminalScanClassic invalid-version warning.

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
