# CustomResolution
**自定义分辨率 Mod（MelonLoader）**

---

## 📌 简介 | Description

**中文**  
CustomResolution 是一款面向 Unity 游戏（默认适配《A Dance of Fire and Ice》）的分辨率增强 Mod。  
支持高 DPI 缩放、非原生分辨率强制应用，以及四种全屏模式一键切换。

**English**  
CustomResolution is a resolution enhancement mod for Unity games (default support for *A Dance of Fire and Ice*).  
It supports High-DPI scaling, forced non-native resolutions, and one-click switching between four fullscreen modes.

---

## ✨ 功能特性 | Features

| 功能 | Feature |
|----|----|
| ✅ 实时读取显示器原生分辨率 | ✅ Real-time native display resolution detection |
| ✅ 高 DPI 自动缩放 UI | ✅ Automatic UI scaling for High-DPI |
| ✅ 自定义任意分辨率（直接应用） | ✅ Custom resolution (apply directly) |
| ✅ 四种全屏模式单按钮切换 | ✅ One-button toggle for 4 fullscreen modes |
| ✅ 防止窗口超出屏幕 | ✅ Prevent window out-of-screen |
| ✅ 配置自动保存 | ✅ Auto-save configuration |
| ✅ Alt+Enter 拦截保护 | ✅ Block Alt+Enter behavior |

---

## 🖥️ 全屏模式说明 | Fullscreen Modes

| 模式 | 说明 | Mode | Description |
|----|----|----|----|
| 窗口模式 | 普通窗口 | Windowed | Normal window |
| 无边框窗口化 | 伪全屏 | Borderless | Borderless fullscreen |
| 强制全屏（拉伸） | 独占 + 拉伸 | Exclusive Stretch | Exclusive + stretched |
| 强制全屏（等比例） | 独占 + 原生比例 | Exclusive Aspect | Exclusive + native aspect |

---

## 📦 安装方法 | Installation

**中文**
1. 安装 https://melonloader.dev/
2. 将 `CustomResolution.dll` 放入 `Mods` 文件夹
3. 启动游戏
4. 按 **F1** 打开设置窗口

**English**
1. Install https://melonloader.dev/
2. Place `CustomResolution.dll` into the `Mods` folder
3. Launch the game
4. Press **F1** to open settings

---

## 🎮 使用方法 | Usage

| 操作 | Action |
|----|----|
| F1 | 打开 / 关闭窗口 | Open / Close UI |
| 选择分辨率 | 应用预设分辨率 | Select resolution |
| 自定义分辨率 | 输入并直接应用 | Enter & apply custom res |
| 切换全屏模式 | 单按钮循环 | Cycle fullscreen modes |
| 应用 | 保存并生效 | Save & apply |

---

## ⚙️ 配置文件 | Configuration

路径 | Path  
```
A Dance of Fire and Ice/UserData/MelonPreferences.cfg
```

示例 | Example
```ini
[CustomResolution]
Width=1920
Height=1080
FullScreenMode=1
UIScale=1.25
```

---

## ⚠️ 注意事项 | Notes

- 自定义分辨率不会写入列表，仅本次生效  
  *Custom resolutions are applied directly and not saved to list.*
- 强制全屏可能受显卡驱动影响  
  *Exclusive fullscreen may depend on GPU drivers.*
- 高 DPI 下 UI 会自动缩放  
  *UI auto-scales under High-DPI.*
- 目前仅支持简体中文
  *Simplified Chinese Only.*
  
---

## 🛠️ 技术信息 | Technical Info

| 项目 | Info |
|----|----|
| 框架 | MelonLoader |
| 补丁 | Harmony |
| UI | Unity IMGUI |
| 版本 | **0.0.1** |
| 作者 | MMXCawa |

---

## 📜 许可证 | License

MIT License

---

## 🙏 致谢 | Credits

- https://github.com/LavaGang/MelonLoader
- https://github.com/pardeike/Harmony

---
