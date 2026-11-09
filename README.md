# Pac-Man (Unity)

A basic Pac-Man recreation made as a university project.

## Requirements

- Unity **2021.3 LTS** (project was created with 2021.3.25f1; any later 2021.3 patch works)

## Opening the project

1. Open Unity Hub → **Add** → select this folder.
2. Open `Assets/Scenes/MainGame.unity` and press Play.

## Controls

| Key | Action |
| --- | --- |
| Arrow keys / WASD | Move |
| Esc / P | Pause / resume |
| R | Restart (while paused or after game over) |
| Enter / Space | Play again after game over |
| Q | Quit (while paused or after game over) |

## Building a playable version

**From the editor:** File → Build Settings → pick your platform → **Build**. `MainGame` is already
in the scene list.

**Automatically with GitHub Actions:** `.github/workflows/build.yml` builds Windows, macOS and
Linux versions using [GameCI](https://game.ci). Add these repository secrets first
(Settings → Secrets and variables → Actions):

- `UNITY_LICENSE` – contents of your Unity `.ulf` license file
  ([how to get one](https://game.ci/docs/github/activation))
- `UNITY_EMAIL` and `UNITY_PASSWORD` – your Unity account

Then run the **Build game** workflow (Actions tab → Build game → Run workflow) and download the
builds from the run's *Artifacts* section.

## Gameplay fixes

- Losing all three lives now ends the game with a Game Over screen (score + high score) instead
  of only printing to the console; you can restart from there.
- Short invincibility after losing a life, so one ghost touch can't take several lives at once.
- Eaten ghosts (eyes returning home) no longer hurt you or give points again.
- Power pellets no longer affect ghosts that are still in the ghost house or already eaten.
- Fixed an exception on start in `PlayerGhostInteraction`, and ghost respawn timers piling up after
  several deaths (which threw an out-of-range exception).
- Fixed Pink's "aim ahead of Pac-Man" targeting, which read the wrong rotation value.
- Snappier controls (raw input, no diagonal movement), pause menu, Esc/Q to quit.
- The window now starts fullscreen-windowed and is resizable, and the HUD scales with resolution
  (it was fixed to 1920×1080 before).
- Removed the empty, unused `Game Over` scene.

## Maintenance notes

- Removed deprecated / unused packages: `com.unity.ide.vscode` (deprecated by Unity; VS Code
  support now comes from `com.unity.ide.visualstudio`), `com.unity.collab-proxy`
  (Unity Version Control, not used – the project is on Git) and `com.unity.visualscripting` (unused).
- Updated `com.unity.ide.rider`, `com.unity.ide.visualstudio`, `com.unity.test-framework`
  and `com.unity.textmeshpro` to their latest 2021.3-compatible versions.
- Removed 188 "missing script" components left on the pellets by a deleted script.
- `SampleScene` was an exact duplicate of `MainGame`; it was removed and `MainGame` is now the
  scene in the build settings.
