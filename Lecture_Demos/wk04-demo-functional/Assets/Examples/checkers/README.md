# Checkers: one script vs. one script per job

Two scenes, identical gameplay, opposite architecture. Open them side by side and
play both - you cannot tell them apart from the Game view, which is the whole point.

| Scene | Script(s) | Hierarchy |
|---|---|---|
| `Scenes/checkers-simple.unity` | `Monolithic/CheckersMonolith.cs` (1 file) | one GameObject, one component |
| `Scenes/checkers-complicated.unity` | `Modular/*.cs` (13 files) | one GameObject, ten components |

Nothing needs to be authored by hand: the board (cubes) and the pieces
(cylinders) are created from primitives at runtime. Press Play and drag.

## The game

- 8x8 board, red at the bottom three rows, black at the top three.
- Drag a piece with the left mouse button; release it over a square.
- Illegal drops snap back to where the piece came from.
- Diagonal slide forward one square, or jump an opponent to remove it.
- Reach the far row to be crowned - kings move and jump backwards too.
- A piece that can jump again keeps the turn.
- Capture every enemy piece to win. The status line is top-left.

## What each modular script owns

| Script | Concern |
|---|---|
| `CheckersTypes.cs` | shared vocabulary: `PlayerColor`, `Move` |
| `PrimitiveFactory.cs` | making a coloured cube/cylinder |
| `BoardSettings.cs` | board geometry: sizes, colours, cell <-> world |
| `BoardBuilder.cs` | spawning the checkerboard |
| `PieceSpawner.cs` | the opening position |
| `Piece.cs` | one piece: owner, square, crown, how it shows itself |
| `BoardState.cs` | which piece is on which square |
| `MoveRules.cs` | is this move legal? can it jump again? should it be crowned? |
| `MoveExecutor.cs` | committing an approved move |
| `TurnManager.cs` | whose turn, multi-jump lock, win check, status text |
| `PieceDragger.cs` | the mouse, and nothing else |
| `MoveCoordinator.cs` | wiring the above into a game loop |
| `TurnHud.cs` | drawing the status line |

## Talking points for the lecture

- **Both are ~the same amount of code.** Splitting up did not add work, it moved it.
- **Try changing one thing in each version.** e.g. "kings may not jump backwards",
  or "board is 10x10", or "pieces fade out instead of vanishing". In the modular
  version you know which file to open before you start reading.
- **Try deleting one thing.** Delete `TurnHud` from the complicated scene and the
  game still runs. Delete the status text from the monolith and you are editing
  the same file that holds the rules.
- **Reuse.** `MoveRules` has no mouse, no rendering and no `Update()`, so it can be
  driven by an AI, a network message or a unit test. The monolith's rules only
  exist inside a mouse-release.
- **The cost is real too:** ten components on one GameObject, indirection through
  an event, and you have to know where to look. The monolith is one file you can
  read top to bottom - which is why it is the tempting choice on day one and the
  expensive one on day thirty.

## Notes

- The project uses the **Input System package** (not the old `Input` class), so both
  versions read the mouse via `Mouse.current`.
- Everything is generated at runtime, so there are no prefabs to keep in sync. If you
  want prefab pieces instead, replace `PrimitiveFactory.Create` in `PieceSpawner`
  (modular) or `CreatePiece` (monolith) with `Instantiate`.
