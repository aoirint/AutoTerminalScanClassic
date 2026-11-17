# Development Operations

## Dependency updates

Run these commands from the repository root in PowerShell.

Wait seven days after a package version is published unless a trusted
security advisory warrants an earlier update.

For a plugin dependency update:

```powershell
dotnet restore AutoTerminalScanClassic/AutoTerminalScanClassic.csproj `
  --configfile AutoTerminalScanClassic/nuget.config --force-evaluate --use-lock-file
dotnet restore AutoTerminalScanClassic/AutoTerminalScanClassic.csproj `
  --configfile AutoTerminalScanClassic/nuget.config --locked-mode
```
