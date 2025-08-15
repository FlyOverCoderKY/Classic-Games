# START HERE — Beginner: Number Guess (CLI, .NET 8)

## Prerequisites
- Install **.NET 8 SDK**: https://dotnet.microsoft.com/download
- Verify install:
  ```bash
  dotnet --version
  dotnet --info
  ```

## Project Setup
1. Unzip the repo and open a terminal at the folder:
   ```bash
   cd classic-games/beginner
   ```
2. (Optional sanity) Try to build and run tests. **It will fail** until you complete the prompts below:
   ```bash
   dotnet build ./NumberGuess/NumberGuess.csproj
   dotnet test  ./NumberGuess.Tests/NumberGuess.Tests.csproj
   ```

## How to Use Cursor for this Exercise
- Open the `beginner` folder in **Cursor**.
- Work through the **Prompt Roadmap** below **in order**. For each step:
  1. Open the file at the indicated line.
  2. Copy the prompt text (after “PROMPT:”) and paste it into Cursor.
  3. Let Cursor generate code; review and accept with small edits if needed.
  4. Rebuild after a few steps to check progress.


## Before You Prompt: Explore the Project
Before firing prompts into Cursor, take 5–10 minutes to **see what you’ve got**.

1. Build and try to run:
   ```bash
   dotnet build ./NumberGuess/NumberGuess.csproj
   dotnet test  ./NumberGuess.Tests/NumberGuess.Tests.csproj   # will fail at first
   dotnet run   --project ./NumberGuess/NumberGuess.csproj     # will likely throw NotImplementedException
   ```
   That failure is intentional—it shows you exactly what’s missing.

2. Use **Cursor** to get oriented:
   - Right‑click files and use “Explain” / “Ask” to **summarize `Program.cs`, `Game.cs`, and `ScoreCalculator.cs`**.
   - Ask for a **walkthrough of the program flow**: where input is read, how the secret number is stored, and where scoring will be called.
   - Have Cursor **list the TODOs / PROMPTs** it finds and paraphrase them in plain English.


## Prompt Roadmap (do these in this order)
- 1. `NumberGuess/Game.cs` line **19** — PROMPT: Implement ConfigureRange() to set (_lower,_upper) for Easy(1-50), Normal(1-100), Hard(1-500).
- 2. `NumberGuess/Program.cs` line **7** — PROMPT: Ask Cursor to implement Game.Play() that runs the main loop with input validation,
- 3. `NumberGuess/Program.cs` line **11** — PROMPT: Ask Cursor to add command-line parsing: support --difficulty Easy|Normal|Hard and --seed <int>.
- 4. `NumberGuess.Tests/ScoreCalculatorTests.cs` line **11** — PROMPT: Ask Cursor to implement Calculate() and make these tests pass.
- 5. `NumberGuess/Game.cs` line **35** — PROMPT: Implement ConfigureRange() that validates _difficulty and sets _lower/_upper accordingly.
- 6. `NumberGuess/Game.cs` line **43** — PROMPT: Implement a loop that reads lines from Console, supports 'quit', validates integers,
- 7. `NumberGuess/Game.cs` line **53** — PROMPT: Implement "warm/colder" logic: based on distance between lastGuess and _secret compared
- 8. `NumberGuess/ScoreCalculator.cs` line **5** — PROMPT: Ask Cursor to design a scoring function that rewards fewer attempts and higher difficulty.

## Build, Run, Test
```bash
# Build app and tests
dotnet build ./NumberGuess/NumberGuess.csproj
dotnet build ./NumberGuess.Tests/NumberGuess.Tests.csproj

# Run tests
dotnet test  ./NumberGuess.Tests/NumberGuess.Tests.csproj

# Run the game (after prompts are implemented)
dotnet run   --project ./NumberGuess/NumberGuess.csproj --difficulty Normal
```
> Tip: Add `--seed 42` to make runs deterministic for debugging.

## Acceptance Criteria
- Game starts and accepts input with **Easy/Normal/Hard** ranges
- Warm/colder hints work after the first guess
- Score is computed; first-try is best; tests pass
- Graceful handling of invalid input and `quit`

Does it meet the AC?
Game starts and accepts input with Easy/Normal/Hard ranges: Yes.
--difficulty is parsed; ranges set to 1–50/1–100/1–500. Covered by CliTests.Cli_ValidDifficulty_ParsesAndDisplays and ProgramTests.ValidDifficulty_ShouldStartGame_AndPrintBanner.
Warm/colder hints work after the first guess: Yes.
Game.GetHint uses previous distance to emit “Warmer!”/“Colder!”. Verified by GameTests.Hints_ShouldReportTrendAndDirection.
Score is computed; first-try is best; tests pass: Yes.
Score printed on win; ScoreCalculator.Calculate ensures attempt count dominates and first-try best. Verified by ScoreCalculatorTests and ScoreCalculatorAdditionalTests. All tests passed.
Graceful handling of invalid input and quit: Yes.
Handles non-integer, out-of-range, EOF, and quit with friendly messages. Verified by GameTests.EOFInput_ShouldExitGracefully and GameTests.InvalidAndOutOfRange_Input_ShouldPrompt. CLI handles help/unknown args gracefully.
Test status: 35 tests, 0 failures.
Conclusion: The NumberGuess application meets all acceptance criteria.
Built beginner/NumberGuess and ran beginner/NumberGuess.Tests. All 35 tests passed, confirming difficulty handling, hints, scoring, and graceful input handling.

## Closing: Ideate Enhancements with Cursor
When your acceptance criteria are green, take 5–10 minutes to brainstorm **stretch features**.

Use one or more of these prompts in Cursor:
- *“Suggest 10 practical feature improvements for this project, grouped by effort (S/M/L). Explain the user benefit of each.”*
- *“Identify weak spots in the current design and propose refactors that improve testability or clarity.”*
- *“Pick one Small feature and draft a task checklist with acceptance tests. Then implement it.”*
- *“Write a short README ‘What I’d Do Next’ section summarizing potential upgrades.”*
