# Pac-Man (Unity)

A basic Pac-Man recreation made as a university project.

## Requirements

- Unity **2021.3 LTS** (project was created with 2021.3.25f1; any later 2021.3 patch works)

## Opening the project

1. Open Unity Hub → **Add** → select this folder.
2. Open `Assets/Scenes/MainGame.unity` and press Play.

## Controls

Arrow keys / WASD to move.

## Maintenance notes

- Removed deprecated / unused packages: `com.unity.ide.vscode` (deprecated by Unity; VS Code
  support now comes from `com.unity.ide.visualstudio`), `com.unity.collab-proxy`
  (Unity Version Control, not used – the project is on Git) and `com.unity.visualscripting` (unused).
- Updated `com.unity.ide.rider`, `com.unity.ide.visualstudio`, `com.unity.test-framework`
  and `com.unity.textmeshpro` to their latest 2021.3-compatible versions.
- Removed 188 "missing script" components left on the pellets by a deleted script.
- `SampleScene` was an exact duplicate of `MainGame`; it was removed and `MainGame` is now the
  scene in the build settings.
