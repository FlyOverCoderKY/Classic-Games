# START HERE — Advanced: Web Tic-Tac-Toe (Razor Pages, Minimax, .NET 8)

## Prerequisites
- **.NET 8 SDK** installed and verified with `dotnet --version`

## Project Setup
```bash
cd classic-games/advanced
dotnet build ./WebTicTacToe/WebTicTacToe.csproj
dotnet run  --project ./WebTicTacToe/WebTicTacToe.csproj
```

Open the URL printed by `dotnet run` in your browser.


## Before You Prompt: Explore the Project
Take 5–10 minutes to see what renders and what’s missing.

1. Build & run the site:
   ```bash
   dotnet build ./WebTicTacToe/WebTicTacToe.csproj
   dotnet run  --project ./WebTicTacToe/WebTicTacToe.csproj
   ```
   The page will load but won’t be fully functional until you implement DI, state, and handlers.

2. Use **Cursor** to get a walkthrough:
   - Ask for a **tour of the Razor Pages flow**: `Program.cs` → DI → `Index.cshtml.cs` → `Index.cshtml`.
   - Have it **summarize the responsibilities** of `GameState` and `GameService` and how session should be used.
   - Ask Cursor to **list each `// PROMPT:`** and describe the required changes in everyday language.


## Using Cursor
Open the `advanced/WebTicTacToe` project in Cursor and follow the road map below. Implement DI and models first, then service logic with **Minimax**, then page handlers/UI.

## Prompt Roadmap (do these in this order)
- [x] 1. `WebTicTacToe/Program.cs` line **6** — Register `GameService` and helpers with DI.
- [x] 2. `WebTicTacToe/Models/GameState.cs` line **3** — Implement a serializable `GameState` that holds:
- [x] 3. `WebTicTacToe/Services/GameService.cs` line **5** — Implement `GameService` with methods:
- [x] 4. `WebTicTacToe/Pages/Index.cshtml.cs` line **13** — Expose state to the page (board/status/scoreboard)
- [x] 5. `WebTicTacToe/Pages/Index.cshtml` line **9** — Render a 3x3 grid as a form with buttons.

### Suggested Minimax seed prompt
> *"Create a Minimax algorithm for Tic-Tac-Toe (3x3) in C#. Return the best legal move for the current player. Use evaluation: win=+10, loss=-10, draw=0; consider depth to prefer faster wins and slower losses."*

## Build & Run
```bash
dotnet build ./WebTicTacToe/WebTicTacToe.csproj
dotnet run  --project ./WebTicTacToe/WebTicTacToe.csproj
```

## Acceptance Criteria
- Playable at `/`
- Bot uses **Minimax** and never loses
- Session scoreboard persists across rematches (in-memory session only)

### Acceptance Criteria Status
- ✅ Playable at `/`: `Pages/Index.cshtml` is the root Razor Page and the app maps Razor Pages in `Program.cs`, so the game is available at the site root.
- ✅ Minimax bot that never loses: The bot uses a depth-aware Minimax (win=+10−depth, loss=depth−10, draw=0) to always force at least a draw. See `Services/GameService.cs`.
- ✅ Session scoreboard persists across rematches: Score is stored in session (`IHttpContextAccessor` + `ISession`) and is not reset on rematch. See `Models/GameState.cs` (`Scoreboard`) and `Services/GameService.cs`.

### Notes on Session Persistence
- The scoreboard is per-browser session and backed by in-memory session storage; restarting the app will clear server memory.
- To reset locally without code changes: clear the site's cookies or open a private/incognito window.
- There is also a `ResetScoreboard()` method in `Services/GameService.cs` you can wire to a button/handler if you want a UI control.

## Closing: Ideate Enhancements with Cursor
When your acceptance criteria are green, take 5–10 minutes to brainstorm **stretch features**.

### Stretch Features
- **Dark mode toggle [S]**: Theme switch persisted (cookie/localStorage); ensure WCAG contrast.
- **Move history & replay [S]**: Record coordinates/turns; step through or auto-replay a finished game.
- **Accessibility polish [S]**: Full keyboard play, visible focus, ARIA live updates for status, reduced motion option.
- **Audio feedback [S]**: Click/win/draw sounds with a user toggle.
- **Reset controls [S]**: Dedicated buttons for new game and reset scoreboard with confirm.

- **Difficulty levels [M]**: Easy=random, Medium=heuristic, Hard=full Minimax; selector stored in session.
- **Undo last move [M]**: Allow one-step undo for the human; re-run solver to continue.
- **Local PvP (hot-seat) [M]**: Human vs human mode with separate scoreboard.
- **Win/turn animations [M]**: Highlight winning line; smooth turn indicator.

- **Online multiplayer (SignalR) [L]**: Room codes/matchmaking, turn sync, basic chat.
- **Persistent leaderboard [L]**: SQLite + EF Core; track wins/streaks across sessions.
- **Variant boards [L]**: 4×4 or NxN with alpha–beta pruning; responsive grid UI.


Use one or more of these prompts in Cursor:
- *“Suggest 10 practical feature improvements for this project, grouped by effort (S/M/L). Explain the user benefit of each.”*

- ### Suggested Features (S/M/L)
- **Small (S)**
  - **Quick rematch shortcut**: One-click button and `R` keybind for faster game loops.
  - **Highlight last move**: Visually mark the most recent cell to improve turn clarity.
  - **Thinking indicator**: Subtle spinner/skeleton while the bot computes to signal responsiveness.
  - **Sound toggle**: Lightweight click/win/draw sounds with a user-controlled mute for feedback without annoyance.

- **Medium (M)**
  - **Difficulty selector**: Easy (random), Medium (heuristic), Hard (Minimax) so all skill levels can enjoy the game.
  - **Move history + undo**: Review past moves and revert one step to learn and correct mistakes.
  - **Local PvP (hot‑seat)**: Two players on one device broadens use cases beyond solo play.
  - **Win-line animation**: Animated highlight of the winning trio for satisfying, clear outcomes.

- **Large (L)**
  - **Online multiplayer (SignalR)**: Play with friends via room codes/matchmaking for social engagement.
  - **Persistent leaderboard (SQLite + EF Core)**: Track wins and streaks across sessions to increase retention.

- *“Identify weak spots in the current design and propose refactors that improve testability or clarity.”*

- ### Weak Spots & Refactors (Testability/Clarity)
- **Tight coupling to `ISession`**: Abstract persistence behind `IGameStateStore` (get/save/reset) and inject it. Benefit: unit-test `GameService` without HTTP context.
- **Mixed responsibilities in `GameService`**: Separate `IRulesEngine` (win/draw/valid moves) and `ISolver` (Minimax). Benefit: smaller, focused units with targeted tests.
- **Mutable shared state**: Avoid in-place board mutations; return new `GameState`/`Board` instances (or copy-on-write). Benefit: fewer side effects, deterministic tests.
- **Hidden magic values**: Extract session keys, symbols ("X","O"), and evaluation scores into constants/options (`IOptions<AiSettings>`). Benefit: safer refactors and configurable difficulty.
- **Implicit validation**: Add guard clauses for illegal moves/out-of-range indices/game-over states. Benefit: clearer failure paths and simpler negative tests.
- **UI-domain leakage**: Introduce a `GameViewModel` and mapping from domain to UI. Benefit: page model tests no longer depend on domain internals.
- **Randomness without control**: Wrap randomness in `IRandomProvider` and inject. Benefit: deterministic solver/tests when randomness is used.
- **Monolithic methods**: Split long handlers/solver methods into named helpers (e.g., `TryApplyMove`, `EvaluateTerminal`, `GenerateMoves`). Benefit: readability and granular unit tests.
- **Ad-hoc serialization**: Centralize session serialization (e.g., `IGameStateSerializer`). Benefit: consistent versioning and isolated serialization tests.
- **Sparse observability**: Use `ILogger<GameService>` with structured events (MoveApplied, BotMoveComputed). Benefit: easier debugging; can assert logs in tests.

- *“Pick one Small feature and draft a task checklist with acceptance tests. Then implement it.”*
 
- ### Task Checklist: Theme switch (persisted + WCAG contrast)
- **Tasks**
  - [x] Define CSS color tokens as CSS variables in `wwwroot/css/site.css` (e.g., `--bg`, `--fg`, `--accent`).
  - [x] Create light and dark theme blocks using `[data-theme="light"]` and `[data-theme="dark"]` scopes.
  - [x] Add a toggle control in `Pages/Index.cshtml` (button or switch) with an accessible label and `aria-pressed` state.
  - [x] Initialize theme on load: read `localStorage.theme` (or cookie), else use `prefers-color-scheme`.
  - [x] Apply theme by setting `document.documentElement.dataset.theme` and persist to `localStorage.theme`.
  - [x] Prevent FOUC: inline a tiny script in the `<head>` to set the theme before CSS paints.
  - [x] Ensure WCAG AA contrast for text and interactive states (hover/focus/disabled) in both themes.
  - [x] Add visible focus styles with sufficient contrast; respect `prefers-reduced-motion` for animations.

- **Acceptance tests**
  - [ ] Toggle switches between light/dark and the choice persists across page reloads and new sessions.
  - [ ] First visit with no stored choice follows system `prefers-color-scheme`.
  - [ ] All primary text meets ≥ 4.5:1 contrast; large text/UI icons/controls meet ≥ 3:1 in both themes.
  - [ ] Focus outline is clearly visible and meets ≥ 3:1 contrast in both themes.
  - [ ] No flash of incorrect theme on load (theme applied before first paint).
  - [ ] Game state (board, scoreboard) is unaffected by theme changes.
  - [ ] Lint/style check passes and no accessibility regressions observed in manual audit.

- *“Write a short README ‘What I’d Do Next’ section summarizing potential upgrades.”*

### What I’d Do Next
- **Harden architecture & tests**: Extract `IGameStateStore`, `IRulesEngine`, `ISolver`; add unit tests for rules, solver, and session store to lock in behavior.
- **UX polish**: Add move history, single-step undo, last-move highlight, and win-line animation for clarity and delight.
- **Difficulty & game modes**: Difficulty selector (Easy/Medium/Hard) and Local PvP to support more players and skill levels.
- **Multiplayer MVP**: SignalR room codes with turn sync and basic chat to enable quick friend matches.
- **Persistence**: SQLite + EF Core leaderboard and match history to increase retention.
- **Accessibility & i18n**: Full keyboard support, ARIA live regions, reduced motion, and resource-based strings for localization.
- **Observability**: Structured logging with `ILogger`, minimal metrics (games played, bot time) to aid debugging and tuning.
- **CI/CD & quality gates**: GitHub Actions for build/test/format; add coverage and analyzers to keep quality high.
- **Performance**: Alpha–beta pruning and simple transposition cache to scale to larger boards.
- **Docs**: Brief architecture overview and testing strategy to onboard contributors faster.

