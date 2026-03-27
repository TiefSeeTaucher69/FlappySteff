# Flappy Steff – Projektkontext für Claude

## Was ist das?
PC Flappy-Bird-Klon in Unity (URP, Unity 6). Einzelspieler. Deutsche UI und Kommentare.
GitHub: https://github.com/TiefSeeTaucher69/FlappySteff

## Szenen-Reihenfolge
BootScene → FirstOpen (nur erstes Mal) → MainMenu → GameScene / ItemShop / SettingsScene / EscapeScene

## Skins
| Name | Status | PlayerPrefs-Key |
|------|--------|----------------|
| `benjo-bird` | **Default (kostenlos, immer owned)** | — |
| `ginger-bird` | Kaufbar (25 Cannabis) | `HasSkin_ginger-bird` |
| `tom-bird` | Kaufbar (25 Cannabis) | `HasSkin_tom-bird` |
| `bennet-bird` | Kaufbar (25 Cannabis) | `HasSkin_bennet-bird` |
| `jan-bird` | Kaufbar (25 Cannabis) | `HasSkin_jan-bird` |

Sprites liegen in `Assets/Resources/Skins/`. Werden per `Resources.Load<Sprite>("Skins/<name>")` geladen.
Aktiver Skin: PlayerPrefs `ActiveSkin` (Default: `"benjo-bird"`)

## Items / Power-ups
| Name | PlayerPrefs-Key | Kosten |
|------|----------------|--------|
| Invincible | `HasInvincibleItem` | 50 Cannabis |
| Shrink | `HasShrinkItem` | 50 Cannabis |
| Laser | `HasLaserItem` | 50 Cannabis |

Aktives Item: PlayerPrefs `ActiveItem` ("Invincible" / "Shrink" / "Laser" / "")

## Trails
| Name | PlayerPrefs-Key | Kosten |
|------|----------------|--------|
| Red | `HasTrailRed` | 20 Cannabis |
| Purple | `HasTrailPurple` | 20 Cannabis |
| Blue | `HasTrailBlue` | 20 Cannabis |

Aktiver Trail: PlayerPrefs `ActiveTrail` ("TrailRed" / "TrailPurple" / "TrailBlue" / "")

## Wichtige PlayerPrefs-Keys
| Key | Typ | Bedeutung |
|-----|-----|-----------|
| `Username` | string | Spielername (auch als Unity Auth Display Name gesetzt) |
| `Highscore` | int | Bester Score |
| `CannabisStash` | int | In-Game Währung |
| `TotalScore` | int | Kumulierter Gesamtscore |
| `lastRewardDate` | string | Datum der letzten Daily Reward (yyyy-MM-dd) |
| `WeeklyMissions` | string (JSON) | Aktive Wochenmissionen |
| `WeeklyMissionStartTime` | string | Startdatum der aktuellen Missionswoche (binary long) |
| `VSyncEnabled` | int | 0=aus, 1=an |
| `FPSCap` | int | 0=30, 1=60, 2=120, 3=240, 4=unlimitiert |
| `ResolutionIndex` | int | Index in Screen.resolutions |

## Backend: Unity Gaming Services
Migriert von eigenem Raspberry-Pi-Server (api.benjo.online) auf Unity Services.

| Service | Zweck |
|---------|-------|
| Unity Authentication | Anonymous Sign-In, Display Name = Username |
| Unity Leaderboards | Score-Submission und -Anzeige |

**Leaderboard-ID:** `FlappySteffLeaderboard`

Initialisierung: in `BootScene/UpdateCheckerScript.cs` (async Start). Jede Szene hat eigenen Fallback-Init-Check falls direkt im Editor gestartet.

Kein Echtzeit-Multiplayer. Netcode for GameObjects ist installiert aber ungenutzt.

## Wochenmissionen
Deterministischer Seed basierend auf ISO-Kalenderwoche + Jahr (z.B. `202614`).
Alle Spieler bekommen dieselben 3 Missionen pro Woche. Vollständig offline/lokal.

## Architektur-Entscheidungen
- `WeeklyMissionManager` ist DontDestroyOnLoad Singleton
- `CursorManager` ist DontDestroyOnLoad Singleton (versteckt Cursor im Gameplay, zeigt ihn bei Pause/Tod/Menü)
- Alle Käufe/Fortschritte in PlayerPrefs (kein Cloud Save)
- Auto-Update über GitHub Releases API (lädt .exe Installer herunter)
- Spieler-Eingabe: Tastatur + Maus + Xbox Controller (JoystickButton*)
