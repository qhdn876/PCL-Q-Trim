# PCL-Q-Trim

A stripped-down third-party fork of Plain Craft Launcher (PCL) 2.14.0.0, capable of downloading and launching Minecraft

> **Unofficial.** This is an independent derivative work made by a third party. It is not
> affiliated with, endorsed by, or supported by the PCL author (龙腾猫跃 / Meloong). For the
> original software, please visit https://github.com/Meloong-Git/PCL.

## What this is

PCL is a Minecraft launcher for Windows. This fork keeps the parts that actually launch and
download the game, and removes the parts that sit around them: onboarding pages, help centre,
personalisation, feedback channel and anonymous telemetry.
The original author is currently unable to offer downloads following a report by some stupid group referring to itself as "Boring Parents"; this fork remains available for download.
## What still works

- **Install and download Minecraft**: vanilla releases and snapshots, plus Forge, NeoForge,
  Fabric, Quilt and OptiFine
- **Download content**: modpacks, mods, resource packs, shaders and worlds, with mirror support
- **Launch and play**: Microsoft and third-party account login, Java runtime management,
  per-version isolation, memory and JVM argument settings
- **Manage instances**: version list, mod enable/disable, saves and screenshots

## What was removed

| Removed | Notes |
| --- | --- |
| Personalisation page | Background, title bar and theme settings |
| Help centre and help detail pages | Including the in-app help loader |
| Feedback channel | Error dialogs no longer offer to submit a report |
| Anonymous telemetry | No usage data leaves the machine |

Local diagnostics logging is retained, so crash reports still contain environment details
that help with troubleshooting.

## Download

`dist/PCL-Q-Trim.exe` is a single-file build. Windows 10 or later, with .NET Framework 4.8
(preinstalled on most systems).

Double-click and run. No installer.

## Build

Requirements: Visual Studio 2022 Build Tools with the .NET desktop workload, and MSBuild.

```powershell
msbuild "Plain Craft Launcher 2\Plain Craft Launcher 2.vbproj" -p:Configuration=Release
```

`MeloongCore` is a separate repository and is not tracked here. Clone it into the repository
root before building:

```powershell
git clone https://github.com/Meloong-Git/MeloongCore.git
```

## License and attribution

Distributed under the **PCL Distribution License** and the **PCL Repository Fair Use Guide**.
See [LICENCE](LICENCE) for the full text.

This project is a derivative work under the "heavy use" terms of that guide. It keeps the
original license file, credits the original author in the About page, and retains the
purchase-reminder flow required of forks that can launch Minecraft.

Original author: 龙腾猫跃 (Meloong)
- Project: https://github.com/Meloong-Git/PCL
- Support: https://meloong.com/afd/a/LTCat

---

## 中文说明

本项目是 **Plain Craft Launcher (PCL) 2.14.0.0** 的第三方精简修改版，
保留完整的 Minecraft 下载与启动能力：原版游戏、Forge / NeoForge / Fabric / Quilt / OptiFine、
整合包、Mod、资源包、光影、世界下载，Microsoft 账号与第三方登录，Java 管理，版本隔离。

移除了个性化设置页、帮助页、反馈通道与匿名数据上报。本地诊断日志保留，便于排查崩溃。

`dist/PCL-Q-Trim.exe` 是编译好的单文件版本，双击即可运行，需要 .NET Framework 4.8。

编译前需要把 MeloongCore 克隆到仓库根目录，它不是本仓库的子模块。

许可与署名见 [LICENCE](LICENCE)，原作者龙腾猫跃。
