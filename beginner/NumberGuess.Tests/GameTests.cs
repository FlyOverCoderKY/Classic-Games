using System;
using System.IO;
using System.Text;
using Xunit;

namespace NumberGuess.Tests;

[Collection("ConsoleTests")]
public class GameTests
{
    [Fact]
    public void EOFInput_ShouldExitGracefully()
    {
        string output = RunGameWithInput(Difficulty.Normal, seed: 42, input: string.Empty);
        Assert.Contains("I'm thinking of a number", output);
        Assert.Contains("Goodbye!", output);
    }

    [Fact]
    public void FirstTryCorrect_WithSeed_NormalDifficulty()
    {
        const int seed = 1234;
        // Compute the same secret the game will pick
        int secret = new Random(seed).Next(1, 100 + 1);
        string input = secret + "\n" + "n\n"; // guess correctly, then decline replay

        string output = RunGameWithInput(Difficulty.Normal, seed, input);

        Assert.Contains("Correct! You got it in 1 attempt(s).", output);
        Assert.True(output.Contains("Score:") || output.Contains("New best score:"));
    }

    [Fact]
    public void InvalidAndOutOfRange_Input_ShouldPrompt()
    {
        const int seed = 7;
        string input = string.Join('\n', new[] { "abc", "0", "quit" }) + "\n";
        string output = RunGameWithInput(Difficulty.Easy, seed, input);
        Assert.Contains("Invalid input", output);
        Assert.Contains("Out of range", output);
        Assert.Contains("Goodbye!", output);
    }

    [Fact]
    public void Hints_ShouldReportTrendAndDirection()
    {
        const int seed = 2024;
        // Determine secret for Easy range 1..50
        int secret = new Random(seed).Next(1, 50 + 1);
        int guess1 = Math.Max(1, secret - 10);
        int guess2 = Math.Max(1, secret - 2); // closer -> Warmer

        string input = $"{guess1}\n{guess2}\nquit\n";
        string output = RunGameWithInput(Difficulty.Easy, seed, input);

        Assert.Contains("Too", output); // direction present
        Assert.Contains("Warmer!", output);
    }

    [Fact]
    public void PlayAgain_Yes_ShouldStartNewRound()
    {
        const int seed = 99;
        int secret = new Random(seed).Next(1, 100 + 1);
        // Win first round, choose to play again with 'y', then quit immediately
        string input = $"{secret}\nY\nquit\n";
        string output = RunGameWithInput(Difficulty.Normal, seed, input);
        Assert.Contains("New round!", output);
        // After starting new round, we don't finish the second round; just ensure loop continues
    }

    private static string RunGameWithInput(Difficulty difficulty, int seed, string input)
    {
        // Redirect console I/O for deterministic testing
        var originalIn = Console.In;
        var originalOut = Console.Out;
        try
        {
            using var reader = new StringReader(input);
            var sb = new StringBuilder();
            using var writer = new StringWriter(sb);
            Console.SetIn(reader);
            Console.SetOut(writer);

            var game = new Game(difficulty, seed);
            game.Play();

            writer.Flush();
            return sb.ToString();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }
}


