# Development Operations

## Dependency updates

Run from the repository root in PowerShell.
NuGet applies a seven-day cooldown to automatic version selection.
Follow this cooldown unless a trusted security advisory warrants an earlier update.

For a plugin dependency update:

```powershell
dotnet restore AutoTerminalScanClassic/AutoTerminalScanClassic.csproj `
  --configfile AutoTerminalScanClassic/nuget.config --force-evaluate --use-lock-file
dotnet restore AutoTerminalScanClassic/AutoTerminalScanClassic.csproj `
  --configfile AutoTerminalScanClassic/nuget.config --locked-mode
```
