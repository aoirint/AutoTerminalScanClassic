This changelog is the user-facing release notes for Thunderstore.

For internal implementation details and developer-facing release history, see
the [GitHub changelog][github-changelog].

If you find a release-note error, encounter a bug, or want to report another
project issue, see [CONTRIBUTING.md][contributing], then report it in
[GitHub Issues][github-issues].

## v0.2.0 - 2026-07-18 UTC

This release rebuilds AutoTerminalScanClassic for Lethal Company v81 and
includes internal improvements.

No gameplay changes are introduced.

### Changed

- Added the Lethal Company v81 compatibility label to the Thunderstore
  package.
- Updated the package icon's editable source from GIMP XCF to SVG. The package
  icon may have minor rendering differences.
- Refactored internal configuration handling.
- Improved `Debug.ValidationLogging` diagnostics for unexpected errors in the
  mod's game-event callbacks.

### Notes

- Compatibility: Lethal Company v81 (2026-04-17 UTC, Manifest ID:
  `6423525044216269478`).

## v0.1.2 - 2025-11-24 UTC

- Updated README.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.1 - 2025-11-23 UTC

- Initial release on Thunderstore.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

[contributing]: https://github.com/aoirint/AutoTerminalScanClassic/blob/main/CONTRIBUTING.md
[github-changelog]: https://github.com/aoirint/AutoTerminalScanClassic/blob/main/CHANGELOG.md
[github-issues]: https://github.com/aoirint/AutoTerminalScanClassic/issues
