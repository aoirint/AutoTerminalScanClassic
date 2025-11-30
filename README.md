# AutoTerminalScanClassic

A Lethal Company mod that scans the number of scraps on the moon and sends it to the chat at the start of each day, just like the terminal scan technique.

- [User guide](./assets/README.md)

## Development

Install .NET SDK 10.0 or later.

- <https://dotnet.microsoft.com/en-us/download/dotnet/10.0>

Install PowerShell 7.

- <https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell-on-windows>

Install Visual Studio 2022.

- <https://visualstudio.microsoft.com/en-us/vs/>

Restore NuGet packages.

```powershell
dotnet restore --locked-mode
```

Create BeplnEx profiles in `./profiles` directory.

```powershell
pwsh ./InitProfiles.ps1
```

Open `AutoTerminalScanClassic.sln` in Visual Studio.

Launch `HostAndGuest` profile to debug. Or manually run `Debug.ps1`.

```powershell
pwsh ./Debug.ps1
```

## Code format

- Language version: [C# 13.0](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13)
- Target framework: [.NET standard 2.1](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1)

```powershell
dotnet format
```

## Package management

To update the lock file after modifying your package references, run:

```powershell
dotnet restore --use-lock-file
```

## GitHub Actions management

The repository uses GitHub Actions for CI.

The version of the actions are pinned with [pinact](https://github.com/suzuki-shunsuke/pinact).

```powershell
# Pin
pinact run

# Update
pinact run --update
```

## Build

```powershell
# Debug build
DOTNET_CLI_UI_LANGUAGE=en dotnet build

# Release build
DOTNET_CLI_UI_LANGUAGE=en dotnet build --configuration Release
```

## Release

1. Replace version in `AutoTerminalScanClassic/AutoTerminalScanClassic.csproj` as semver format, e.g. `1.2.3`.
2. Commit and push the changes.
3. CI will create a GitHub Release automatically.
4. Download the release artifact from the GitHub Release page.
5. Upload the artifact to Thunderstore. **NOTE: prerelease version is not supported, e.g. `1.2.3-beta.1`.**

## Debugging

### r2modman

1. Open r2modman.
2. Open `Config editor`.
3. Open `BepInEx\config\BepInEx.cfg` in the config list.
4. Set `Logging.Console.LogLevels` to `All`.
5. Open `Settings > Import local mod`.
6. Select the DLL file from `bin/Debug/netstandard2.1/`.
7. Click `Start modded`.

### Manual

1. Install BepInEx: https://docs.bepinex.dev/articles/user_guide/installation/index.html
2. Launch `Lethal Company.exe` and exit to generate the BepInEx config files.
3. Open `C:/Program Files (x86)/Steam/steamapps/common/Lethal Company/BepInEx/config/BepInEx.cfg`.
4. Copy the DLL file into `C:/Program Files (x86)/Steam/steamapps/common/Lethal Company/BepInEx/plugins/` from `bin/Debug/netstandard2.1/`.
5. Set `Logging.Console.Enabled` to `true`.
6. Set `Logging.Console.LogLevels` to `All`.
7. Launch `Lethal Company.exe` again.
