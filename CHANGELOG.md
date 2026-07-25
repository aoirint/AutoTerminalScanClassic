# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` is derived from
stable release entries in this file and rewritten for users.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

## v0.2.1 - 2026-07-25 UTC

### Fixed

- Restored `SelfOnly` scan-result chat messages on Lethal Company v81. The
  game's local HUD method is private in this version, so the adapter invokes
  the exact v81 method through Harmony reflection without broadcasting the
  message to other players.

### Notes

- Compatibility: Lethal Company v81 (2026-04-17 UTC, Manifest ID:
  `6423525044216269478`).

## v0.2.0 - 2026-07-18 UTC

### Changed

- Promoted `v0.2.0-alpha.1` to stable `v0.2.0`.
- Set the project version to stable `0.2.0`:
    - `BepInEx.PluginInfoProps` derives the BepInEx plugin metadata version
      from the project version.
    - Source `assets/manifest.json` remains at the repository placeholder
      `0.0.0`; the release workflow writes the generated stable Thunderstore
      manifest version into the packaged artifact.
- Published stable user-facing release notes in `assets/CHANGELOG.md`:
    - The stable notes summarize the user-facing outcome from the prerelease
      cycle.
    - Detailed developer-facing prerelease implementation history remains in
      the `v0.2.0-alpha.1` section below.
- Added the Lethal Company v81 compatibility label to the Thunderstore
  package.
- Replaced the editable package-icon source with SVG. The packaged icon may
  have minor rendering differences.

### Fixed

- Corrected the Thunderstore Client-side category configuration to use the
  `clientside` submission key.

### Notes

- Release validation:
    - The maintainer accepted observed v0.1.2 use on v81 as sufficient
      compatibility evidence for this stable release.
    - The planned dedicated alpha-validation matrix was not repeated.
- Compatibility:
    - Compatible with Lethal Company v81 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - The v81 test environment uses BepInExPack v5.4.2305.
- Older Lethal Company versions are no longer claimed as tested by the current
  v0.2.0 release notes; Lethal Company v73 compatibility is recorded in the
  historical `v0.1.x` release entries below.

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
- Updated package-facing compatibility documentation for the current in-game
  Lethal Company v81 label.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - The v81 test environment uses BepInExPack v5.4.2305.
- This was the first `v0.2.0` alpha artifact for release-candidate validation.
- Prerelease artifacts are GitHub-only; Thunderstore publication remains
  limited to stable releases.
- Superseded by stable `v0.2.0`.

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
