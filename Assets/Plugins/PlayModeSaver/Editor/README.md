# 💾 Play Save Mode — Unity 6 Editor Plugin

> Preserve component values modified during Play Mode. No more losing tweaks when you press Stop.

![Unity](https://img.shields.io/badge/Unity-6000%2B-black?logo=unity&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)
![Editor Only](https://img.shields.io/badge/scope-Editor%20Only-blue)
![No Dependencies](https://img.shields.io/badge/dependencies-none-brightgreen)

---

## The Problem

Unity discards all component changes made during Play Mode the moment you press **Stop**. This forces a tedious loop of:

1. Enter Play Mode → tweak values → mentally note them
2. Exit Play Mode → re-enter the values by hand
3. Repeat until it feels right

**Play Save Mode** breaks that loop entirely.

---

## Features

- **Save Now** — snapshot a component's current values at any moment during play
- **Save When Exiting** — track a component and capture its final state when you press Stop
- **Always Save** — register a component once; it auto-saves every single play session
- **Editor Window** — `Tools > Play Save Mode` shows all registered components, their status, and lets you apply or clear saves manually
- **Hierarchy Indicator** — an orange icon appears next to any GameObject with pending saves; click it to jump straight to the window
- **Undo support** — all applied changes are wrapped in a single Undo group (`Ctrl+Z` to revert)
- **Auto scene save** — the scene is saved automatically after changes are applied
- **No dependencies** — four plain C# files, no packages, no Asset Store

---

## Installation

1. Copy the four files into your project under `Assets/Editor/PlaySaveMode/`:

```
Assets/
└── Editor/
    └── PlaySaveMode/
        ├── PlaySaveModeManager.cs
        ├── PlaySaveModeMenuItems.cs
        ├── PlaySaveModeWindow.cs
        └── PlaySaveModeHierarchyIndicator.cs
```

2. Unity will recompile automatically. No further setup required.

> ⚠️ All files **must** live inside an `Editor/` folder. Unity will throw compile errors if they are placed in a runtime-compiled folder.

---

## Usage

### Save Now
> Snapshot the component's values at this exact moment.

1. Enter **Play Mode**
2. Modify any component values in the Inspector
3. Right-click the component header (or click **⋮**) → `Play Save Mode > Save Now`
4. Press **Stop** — values are applied automatically

### Save When Exiting Play Mode
> Track a component and save its final state when you stop.

1. Enter **Play Mode**
2. Right-click the component header → `Play Save Mode > Save When Exiting Play Mode`
3. Keep tweaking freely — the latest state at the moment you press Stop is saved

### Always Save When Exiting Play Mode
> Set-and-forget. Works across all future play sessions.

1. Right-click the component header (in **any** mode) → `Play Save Mode > Always Save When Exiting Play Mode`
2. A ✓ checkmark confirms it's active
3. Click again to un-register

---

## Editor Window

Open via **Tools → Play Save Mode**

| Area | Description |
|---|---|
| Status banner | Green in Play Mode, grey in Edit Mode |
| Pending Saves list | Shows GameObject name, component type, and `always` badge |
| Ping button | Highlights and selects the object in the Hierarchy |
| Remove button | Drops a component from the queue without touching the scene |
| Apply All Changes | Applies all pending saves (Edit Mode only) |
| Clear All | Removes all registrations with a confirmation prompt |

The window is fully **dockable** — drag it next to your Inspector for a permanent view while playtesting.

---

## Hierarchy Indicator

An **orange icon** appears at the right edge of any row in the Hierarchy that has at least one pending save.

- Updates live every 500ms during Play Mode
- Clicking the icon opens the Play Save Mode window directly
- Disappears automatically after changes are applied or cleared

---

## Limitations

- **Serializable fields only** — saves `[SerializeField]` and `public` fields. Private non-serialized fields are not captured.
- **Scene objects only** — Project-window prefabs are not targeted.
- **`EditorJsonUtility` based** — nested references to other scene objects serialized as instance IDs may not resolve correctly across domain reloads.

---

## File Overview

| File | Responsibility |
|---|---|
| `PlaySaveModeManager.cs` | Snapshot storage, serialization, apply-on-exit logic, Undo |
| `PlaySaveModeMenuItems.cs` | Right-click / ⋮ context menu entries on every component |
| `PlaySaveModeWindow.cs` | Dockable Editor Window (`Tools > Play Save Mode`) |
| `PlaySaveModeHierarchyIndicator.cs` | Orange indicator icon in the Hierarchy window |

---

## License

MIT — free to use, modify, and distribute.
