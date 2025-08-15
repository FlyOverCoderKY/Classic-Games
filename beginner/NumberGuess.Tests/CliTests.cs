using System;
using System.IO;
using System.Text;
using Xunit;

namespace NumberGuess.Tests;

public class CliTests
{
    [Fact]
    public void Cli_Help_PrintsUsage()
    {
        var output = new StringWriter(new StringBuilder());
        var exit = NumberGuess.Cli.Run(new[] {"--help"}, new StringReader(string.Empty), output);
        Assert.Equal(0, exit);
        Assert.Contains("Usage: NumberGuess", output.ToString());
    }

    [Fact]
    public void Cli_UnknownArg_PrintsUsage()
    {
        var output = new StringWriter(new StringBuilder());
        var exit = NumberGuess.Cli.Run(new[] {"--nope"}, new StringReader(string.Empty), output);
        Assert.Equal(0, exit);
        Assert.Contains("Unknown argument", output.ToString());
    }

    [Fact]
    public void Cli_StartsGame_AndRespectsInput()
    {
        var input = new StringReader("quit\n");
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var exit = NumberGuess.Cli.Run(Array.Empty<string>(), input, output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains("Number Guess - Difficulty:", text);
        Assert.Contains("I'm thinking of a number", text);
        Assert.Contains("Goodbye!", text);
    }

    [Theory]
    [InlineData("--difficulty", "Easy", "Easy")]
    [InlineData("--difficulty=Normal", null, "Normal")]
    [InlineData("--difficulty", "Hard", "Hard")]
    public void Cli_ValidDifficulty_ParsesAndDisplays(string flag, string? value, string expected)
    {
        var input = new StringReader("quit\n");
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var args = value is null ? new[] { flag } : new[] { flag, value };
        var exit = NumberGuess.Cli.Run(args, input, output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains($"Number Guess - Difficulty: {expected}", text);
    }

    [Fact]
    public void Cli_InvalidDifficulty_PrintsUsage()
    {
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var exit = NumberGuess.Cli.Run(new[] { "--difficulty", "Impossible" }, new StringReader(string.Empty), output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains("Invalid difficulty", text);
        Assert.Contains("Usage: NumberGuess", text);
    }

    [Fact]
    public void Cli_MissingDifficultyValue_PrintsUsage()
    {
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var exit = NumberGuess.Cli.Run(new[] { "--difficulty" }, new StringReader(string.Empty), output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains("Missing value for --difficulty", text);
        Assert.Contains("Usage: NumberGuess", text);
    }

    [Theory]
    [InlineData(new[] { "--seed", "123" }, 123)]
    [InlineData(new[] { "--seed=456" }, 456)]
    public void Cli_ValidSeed_ParsesAndDisplays(string[] args, int expected)
    {
        var input = new StringReader("quit\n");
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var exit = NumberGuess.Cli.Run(args, input, output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains($"Seed: {expected}", text);
    }

    [Fact]
    public void Cli_InvalidSeed_PrintsUsage()
    {
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var exit = NumberGuess.Cli.Run(new[] { "--seed", "nope" }, new StringReader(string.Empty), output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains("Invalid seed", text);
        Assert.Contains("Usage: NumberGuess", text);
    }

    [Fact]
    public void Cli_MissingSeedValue_PrintsUsage()
    {
        var sb = new StringBuilder();
        var output = new StringWriter(sb);
        var exit = NumberGuess.Cli.Run(new[] { "--seed" }, new StringReader(string.Empty), output);
        Assert.Equal(0, exit);
        var text = sb.ToString();
        Assert.Contains("Missing value for --seed", text);
        Assert.Contains("Usage: NumberGuess", text);
    }
}


