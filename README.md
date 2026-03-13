# 倭瓜大冒险 (Squash Adventure) - Unity 2D 动作生存游戏

![Unity](https://img.shields.io/badge/Unity-2022.3+-black?logo=unity) ![C#](https://img.shields.io/badge/Language-C%23-blue?logo=csharp)

## 📝 项目简介
本项目是一款基于 Unity 2D 开发的横版动作生存游戏。玩家操控“倭瓜”角色，通过跳跃避开障碍物并击败敌人。项目重点展示了 Unity 核心组件的应用、UI 交互逻辑以及基础的性能优化方案。

---

## 🚀 核心技术要点 (Technical Highlights)

### 1. 物理控制与交互
* **UI 穿透防御**：利用 `UnityEngine.EventSystems` 实现了点击 UI 按钮时不触发角色跳跃的逻辑，优化了操作体验。
* **自定义物理判定**：结合 BoxCollider2D 与 Rigidbody2D，并针对高速运动进行了 Continuous 碰撞检测优化，防止穿模。

### 2. 性能与架构优化
* **对象池 (Object Pooling)**：针对频繁生成的障碍物和敌人实现了对象池管理，减少了内存抖动和 GC（垃圾回收）压力。
* **单例模式 (Singleton)**：构建了全局 `GameManager` 与 `AudioManager`，确保计分系统与音效控制的唯一性和跨场景访问的便利性。

### 3. UI 与动效表现
* **DOTween 动画集成**：利用 DOTween 实现了结算面板的平滑弹出 (Ease.OutBack) 以及音效的淡入淡出。
* **独立时间轴动画**：通过 `.SetUpdate(true)` 解决了游戏在 `Time.timeScale = 0` (暂停) 状态下 UI 无法播放动画的问题。

---

## 🛠️ 项目结构说明
* `Assets/Scripts`: 核心逻辑代码（玩家控制、对象池、音频管理）。
* `Assets/Prefabs`: 游戏单位、特效及 UI 预制体。
* `ProjectSettings`: 物理层级 (Layers) 与输入配置。

---

## 🎮 如何运行
1. 使用 Unity 2022.3+ 版本打开项目。
2. 引擎会自动根据 `Packages` 重新配置环境。
3. 打开 `Assets/Scenes/Main` 场景并点击 Play 运行。

---

## 👨‍💻 开发者备忘
* **Git 管理**：配置了标准的 Unity `.gitignore`，过滤了 Library 和 Temp 等冗余文件，确保仓库轻量化。
* **代码规范**：遵循 C# 帕斯卡命名规范，逻辑模块化，易于扩展。
