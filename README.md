# HEIC 转档器

把 iPhone 拍摄的 HEIC / HEIF 照片批次转换成 PNG 或 JPG 的 Windows 小工具。

- 开发环境：.NET Framework 4.8 + WinForms
- 执行环境：Windows 10 / Windows 11（系统已内置 .NET Framework 4.8，不需要另外安装 HEIF / HEVC 扩展）
- HEIC 解码：[Magick.NET](https://github.com/dlemstra/Magick.NET)（内置 libheif）

## 功能

- 加入文件、加入资料夹（含子资料夹），或直接把文件/资料夹拖进清单
- 输出格式可选：
  - **PNG（无损）**：像素与原图完全相同，10 位 HEIC 会输出 16 位 PNG；文件会比 HEIC 大很多
  - **JPG（有损）**：品质 1～100 可调（默认 100），使用 4:4:4 色彩取样，尽量减少损失
- 自选输出资料夹，同名文件不会被覆盖（自动改名为 `名称 (1).png`）
- 依方向信息自动转正
- 移除 EXIF / XMP / IPTC（含 GPS 位置），只保留 ICC 色彩描述档（iPhone 多为 Display P3，去掉会偏色）
- 转换在背景执行，界面不会卡住；可随时取消，单张失败不影响其他文件
- 记住上次的输出资料夹、格式与品质（`%AppData%\HeicConverter\settings.ini`）

## 编译

1. 用 Visual Studio 2022（需安装「.NET 桌面开发」工作负载）打开 `HeicConverter.sln`
2. 选择 `Release`，执行「生成解决方案」，NuGet 套件会自动还原
3. 输出在 `src\HeicConverter\bin\Release\net48\`

也可以用命令行：

```
dotnet build HeicConverter.sln -c Release
```

## 发布

把 `bin\Release\net48\` 整个资料夹复制给使用者即可，需包含：

- `HeicConverter.exe`、`HeicConverter.exe.config`
- `Magick.NET-Q16-AnyCPU.dll`、`Magick.NET.Core.dll`
- `Magick.Native-Q16-x64.dll`（以及 `x86`、`arm64` 版本，依电脑架构自动选用）

## 项目结构

| 文件 | 说明 |
| --- | --- |
| `HeicImageConverter.cs` | 转换核心逻辑（与界面无关） |
| `ConvertOptions.cs` | 输出格式与转换参数 |
| `MainForm.cs` | 主窗口：清单管理、批次转换流程 |
| `AppSettings.cs` | 记住使用者设定 |
