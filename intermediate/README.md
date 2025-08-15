# START HERE — Intermediate: Tic-Tac-Toe (Engine + CLI, .NET 8)

## Prerequisites
- **.NET 8 SDK** installed and verified with `dotnet --version`

## Project Setup
```bash
cd classic-games/intermediate
dotnet build ./TicTacToe.Core/TicTacToe.Core.csproj
dotnet build ./TicTacToe.Cli/TicTacToe.Cli.csproj
dotnet build ./TicTacToe.Tests/TicTacToe.Tests.csproj
# Tests will fail until you complete the prompts below
```


## Before You Prompt: Explore the Project
Spend 5–10 minutes getting the lay of the land.

1. Build and attempt tests / run:
   ```bash
   dotnet build ./TicTacToe.Core/TicTacToe.Core.csproj
   dotnet build ./TicTacToe.Cli/TicTacToe.Cli.csproj
   dotnet build ./TicTacToe.Tests/TicTacToe.Tests.csproj
   dotnet test  ./TicTacToe.Tests/TicTacToe.Tests.csproj       # failing at first is expected
   dotnet run   --project ./TicTacToe.Cli/TicTacToe.Cli.csproj # will likely fail when hitting NotImplemented
   ```

2. Use **Cursor** to orient yourself:
   - Ask Cursor for a **high‑level architecture summary** of `Board`, `GameStatus`, `IBotStrategy`, and `HeuristicBot`.
   - Request a **diagram or bullet flow** of a typical turn: human move → validate → status check → bot move.
   - Have Cursor **enumerate each `// PROMPT:`** in order and restate what you’ll need to implement.


## Using Cursor
Follow the **Prompt Roadmap** below. Implement the engine first, then the bot, then wire up the CLI.

## Prompt Roadmap (do these in this order)
- [x] 1. `TicTacToe.Core/Board.cs` line **14** — PROMPT: Ask Cursor to implement immutable Apply(Move) that returns a NEW Board with the move applied,
- [x] 2. `TicTacToe.Core/Board.cs` line **21** — PROMPT: Ask Cursor to implement GetStatus() that checks rows, cols, diags, and draw.
- [x] 3. `TicTacToe.Core/Board.cs` line **27** — PROMPT: Ask Cursor to implement GetEmptyCells() helper returning IEnumerable<(int r,int c)>.
- [x] 4. `TicTacToe.Cli/Program.cs` line **9** — PROMPT: Ask Cursor to implement Render(board) that draws the 3x3 grid with coordinates.
- [x] 5. `TicTacToe.Cli/Program.cs` line **17** — PROMPT: Ask Cursor to parse input, validate, and apply move; handle errors gracefully.
- [x] 6. `TicTacToe.Cli/Program.cs` line **22** — PROMPT: Ask Cursor to extract GameLoop into a class and add undo/redo as stretch goals.
- [x] 7. `TicTacToe.Core/HeuristicBot.cs` line **5** — PROMPT: Ask Cursor to implement a simple heuristic:
- [x] 8. `TicTacToe.Core/IBotStrategy.cs` line **5** — PROMPT: Ask Cursor to implement ChooseMove(Board board) that selects a legal move for the current player.
- [x] 9. `TicTacToe.Tests/BoardTests.cs` line **18** — PROMPT: Ask Cursor to make Apply(...) work immutably and then make this pass.

## Build, Run, Test
```bash
dotnet test  ./TicTacToe.Tests/TicTacToe.Tests.csproj
dotnet run   --project ./TicTacToe.Cli/TicTacToe.Cli.csproj
```

## Acceptance Criteria
- Immutable engine (`Apply`, `GetStatus`) with tests passing — DONE
  - `Apply` is immutable, validates moves, and flips `CurrentPlayer`. `GetStatus` evaluates rows/cols/diagonals and draw.
  - All tests pass (2/2).
- Heuristic bot plays legal moves (win/block/center/corners/edges) — DONE
  - Chooses winning move if available; otherwise blocks; otherwise prefers center → corners → edges.
- CLI renders board and handles invalid input without crashing — DONE
  - Grid rendered with coordinates; input parsing validates format/range; graceful error messages; 'q' to quit.
- (Stretch) Extract a `GameLoop` class and add undo/redo — DONE
  - `GameLoop` class manages turns; supports undo ('u') and redo ('r') with history stacks.

## Closing: Ideate Enhancements with Cursor
When your acceptance criteria are green, take 5–10 minutes to brainstorm **stretch features**.

## Stretch Features (Ideas)
Small
- Add board coordinate hints in prompts (e.g., "Try center: 2,2").
- Colorize output (X red, O blue) using ANSI codes with a Windows-safe fallback.
- Add a "new game" command ('n') to reset without restarting the app.
- Show last move highlight indicator on the grid.

Medium
- Pluggable difficulty: random bot, heuristic bot (current), minimax bot.
- Add move numbering and a printable move list.
- Save/load game state to a JSON file.
- Configurable first player (human or bot) via CLI flag.

Large
- Implement a full minimax/alpha-beta bot with depth-limited search and scoring.
- Port CLI to a simple web UI using ASP.NET Core Minimal APIs or Razor Pages.
- Add multiplayer over network (simple TCP or SignalR) with lobby/join flow.
- Integrate CI to run tests on push and publish a global tool to `dotnet tool`.

Use one or more of these prompts in Cursor:
- *“Suggest 10 practical feature improvements for this project, grouped by effort (S/M/L). Explain the user benefit of each.”*
### Small (S)
- **Input UX polish**: Add inline hints, forgiving separators (e.g., "2 3"), and a help command ('h').
  - Benefit: Fewer input errors; smoother first-time experience.
- **Turn status + last-move highlight**: Show current player and mark the last move on the board.
  - Benefit: Easier to follow the game state at a glance.
- **Configurable start and symbols**: Flags to choose who starts and swap X/O characters.
  - Benefit: Lets players tailor the game to preference and practice from different positions.
- **Quick commands**: 'n' for new game, 's' to show score, 'r' to redo, 'u' to undo, 'q' to quit, 'h' help.
  - Benefit: Faster control without restarting the app.

### Medium (M)
- **Save/load games (JSON)**: Persist and resume games; add autosave-on-exit.
  - Benefit: Continue play later or share interesting positions.
- **Difficulty levels**: Random, heuristic (current), shallow minimax (depth 2–3).
  - Benefit: Scales challenge for beginners through intermediate players.
- **Hints/coaching**: Provide a suggested move with a brief rationale.
  - Benefit: Teaches strategy and helps users improve.
- **Local 2‑player mode**: Pass‑and‑play toggle (human vs human).
  - Benefit: Simple multiplayer without networking.

### Large (L)
- **Smarter AI (minimax + alpha‑beta)**: Deeper search with evaluation function and optional time/depth limits.
  - Benefit: Stronger, configurable opponent for advanced users.
- **Web UI and/or online multiplayer**: Minimal web front‑end; SignalR lobby for invites and matchmaking.
  - Benefit: Play from browser and versus friends remotely.


- *“Identify weak spots in the current design and propose refactors that improve testability or clarity.”*
-
## Design Weak Spots and Refactors
- **Board: status logic is duplicated/hard-coded**
  - Refactor: Extract a reusable line enumerator and winner check.
    - Add `IEnumerable<(Cell,Cell,Cell)> EnumerateLines()` that yields 8 lines.
    - Implement `GetStatus()` by iterating these lines.
  - Benefit: Clearer logic, easier tests for rows/cols/diags, fewer bugs.

- **Board: lack of safe simulation helpers for bots/tests**
  - Refactor: Add non-throwing and utility APIs.
    - `bool TryApply(Move move, out Board next, out string? error)`.
    - `Board Clone()` to centralize copying logic.
  - Benefit: Cleaner bot/search code; simpler tests around illegal moves.

- **Bot: win/block check reimplements win detection**
  - Refactor: Centralize “would this move win” in engine.
    - `static bool BoardWouldWin(Board b, int r, int c, Cell player)` using the same line logic.
  - Benefit: Consistency with engine rules; easier to reason about and test.

- **Bot: simulation depends on `CurrentPlayer`**
  - Refactor: Provide a pure hypothetical evaluation.
    - `static GameStatus EvaluateHypothetical(Board b, int r, int c, Cell player)`.
  - Benefit: Simple, deterministic block/win checks.

- **CLI: rendering and parsing are hard to unit test**
  - Refactor: Extract to dedicated components.
    - `BoardRenderer.RenderToString(Board, (int r,int c)? lastMove = null)`.
    - `MoveParser.TryParse(string input, out (int r,int c))`.
  - Benefit: Enables fast, isolated tests for UI logic; reduces coupling.

- **CLI: `GameLoop` mixes I/O with decisions**
  - Refactor: Invert dependencies.
    - `IConsoleAdapter` injected into `GameLoop` for I/O.
    - Inject `IBotStrategy` and an `IGameHistory` abstraction.
  - Benefit: Deterministic, mockable loop; easier to extend (GUI/web).

- **Undo/redo: history semantics not uniform**
  - Refactor: Centralize history policy.
    - Clear redo on any new apply (human or bot).
    - Track a parallel `Stack<Move>`; recompute board from move list if needed.
  - Benefit: Predictable UX; smaller memory; simpler serialization.

- **Tests: minimal coverage for engine and none for CLI/bot**
  - Refactor: Add unit tests for:
    - `GetStatus` (all lines, early win, draw), `Apply` invariants, `GetEmptyCells`.
    - Bot behavior (win, block, priority order).
    - `MoveParser` and `BoardRenderer` once extracted.
  - Benefit: Confidence against regressions; documents behavior.

- **API clarity: constructing targeted positions**
  - Refactor: Add an internal constructor `Board(Cell[,] cells, Cell currentPlayer)` with defensive copy.
  - Benefit: Easier targeted test setups without long apply chains.

- **Project organization**
  - Refactor: Split into namespaces/files (e.g., `TicTacToe.Cli.UI`, `TicTacToe.Engine.Analysis`).
  - Benefit: Discoverability and maintenance; clearer boundaries.
- *“Pick one Small feature and draft a task checklist with acceptance tests. Then implement it.”*

### Task: Configurable start and symbols (CLI flags)

#### Task checklist
- [x] Define CLI flags and help
  - [ ] Add `--start {human|bot}` (alias `-s`)
  - [ ] Add `--human-mark {X|O}` (alias `-m`)
  - [ ] Add `--swap-marks` (alias `-w`) convenience toggle
  - [ ] Update `--help` to document flags and precedence
- [x] Parse flags in `intermediate/TicTacToe.Cli/Program.cs`
  - [ ] Create `CliOptions` with validated settings:
    - `StartingPlayer: Human|Bot`
    - `HumanMark: X|O`
    - `BotMark: X|O`
  - [x] Precedence: `--human-mark` overrides `--swap-marks`
- [x] Validate combinations and invalid values
  - [x] Non‑zero exit and usage hint on invalid input
- [x] Apply options to game setup
  - [ ] Initialize starting turn from `StartingPlayer`
  - [ ] Assign marks to Human/Bot from options
  - [ ] Ensure move order respects `StartingPlayer`
- [x] Ensure rendering/prompts reflect settings
  - [ ] Board render and turn messages use selected marks
  - [ ] Print one‑line configuration summary at start, e.g., `Starting: Bot | Human: O | Bot: X`
- [x] Update this README usage with examples
- [x] Add tests
  - [x] Unit tests for `CliOptions` parsing/validation
  - [x] CLI acceptance tests (scripted runs) asserting output lines

#### Acceptance tests
Assumptions: Defaults are Human starts; Human=X; Bot=O. Starting player and marks are independent. Precedence: `--human-mark` > `--swap-marks`.

- Default configuration
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --once
    ```
  - Expect: line containing `Starting: Human | Human: X | Bot: O`

- Bot starts
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --start bot --once
    ```
  - Expect: `Starting: Bot | Human: X | Bot: O`; first move output belongs to Bot

- Human mark set to O (Human starts)
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --human-mark O --once
    ```
  - Expect: `Starting: Human | Human: O | Bot: X`; first move is O (Human)

- Human mark O and Bot starts
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --human-mark O --start bot --once
    ```
  - Expect: `Starting: Bot | Human: O | Bot: X`; first move is X (Bot)

- Swap marks (Human becomes O, Bot becomes X)
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --swap-marks --once
    ```
  - Expect: `Starting: Human | Human: O | Bot: X`

- Precedence of human-mark over swap-marks
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --swap-marks --human-mark X --once
    ```
  - Expect: `Starting: Human | Human: X | Bot: O` (swap ignored by explicit human-mark)

- Invalid start value
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --start alice --once
    ```
  - Expect: non‑zero exit; output contains usage with `--start {human|bot}`

- Invalid human-mark value
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --human-mark Z --once
    ```
  - Expect: non‑zero exit; output contains usage with `--human-mark {X|O}`

- Help shows new flags
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --help
    ```
  - Expect: mentions `--start`, `--human-mark`, `--swap-marks`, and precedence note

- Board render uses chosen marks
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --swap-marks --once
    ```
  - Expect: board/state lines only show `X` and `O` consistent with `Human: O | Bot: X`; no assumption that X always moves first

- Combined flags consistent across a short game
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --start bot --human-mark O --script "pass,pass,pass" --once
    ```
  - Expect: start line `Starting: Bot | Human: O | Bot: X`; move order alternates Bot(X), Human(O), Bot(X)…

- No duplicate configuration line when flags omitted
  - Command:
    ```bash
    dotnet run --project ./TicTacToe.Cli/TicTacToe.Cli.csproj -- --once
    ```
  - Expect: exactly one configuration summary line


- *“Write a short README ‘What I’d Do Next’ section summarizing potential upgrades.”*

## What I'd Do Next

- **Polish the CLI UX**: Colorize marks (Windows-safe), show current player inline, and highlight last move.
- **Difficulty settings**: Add random and shallow minimax strategies with a `--difficulty` flag.
- **Local 2‑player mode**: Pass‑and‑play toggle to support Human vs Human without the bot.
- **Save/Load games**: Persist move history to JSON; `--load <file>` and `:w`/`:o` commands.
- **Refactor UI seams**: Extract `BoardRenderer` and `MoveParser` for easier testing and reuse.
- **Smarter AI**: Implement minimax with alpha‑beta pruning and optional depth/time limits.
- **Scriptable runs**: Add `--script` input playback and `--once`/`--seed` to aid reproducible tests.
- **Packaging/CI**: Publish as a global tool (`dotnet tool`) and add CI to run tests on push/PR.
