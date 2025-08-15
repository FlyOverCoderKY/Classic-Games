using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace NumberGuess.Tests;

public class ProgramTests
{
    private static string TestAppPath => Path.Combine(
        AppContext.BaseDirectory,
        "..", "..", "..", "..",
        "NumberGuess",
        "bin", "Debug", "net8.0",
        Environment.OSVersion.Platform == PlatformID.Win32NT ? "NumberGuess.exe" : "NumberGuess");

    [Fact]
    public void HelpFlag_ShouldPrintUsage_AndExit()
    {
        var result = RunProcess("-h");
        Assert.Contains("Usage: NumberGuess", result.output);
        Assert.Equal(0, result.exitCode);
    }

    [Theory]
    [InlineData("--difficulty", "Easy")]
    [InlineData("--difficulty=Normal", null)]
    [InlineData("--difficulty", "Hard")]
    public void ValidDifficulty_ShouldStartGame_AndPrintBanner(string flag, string? value)
    {
        var args = value is null ? flag : $"{flag} {value}";
        var result = RunProcess(args, input: "quit\n");
        Assert.Contains("Number Guess", result.output);
        Assert.Contains("Difficulty:", result.output);
        Assert.Equal(0, result.exitCode);
    }

    [Fact]
    public void InvalidDifficulty_ShouldPrintUsage_AndExit()
    {
        var result = RunProcess("--difficulty Impossible");
        Assert.Contains("Invalid difficulty", result.output);
        Assert.Contains("Usage: NumberGuess", result.output);
        Assert.Equal(0, result.exitCode);
    }

    [Fact]
    public void SeedParsing_ShouldAcceptInteger()
    {
        var result = RunProcess("--seed 123", input: "quit\n");
        Assert.Contains("Seed: 123", result.output);
        Assert.Equal(0, result.exitCode);
    }

    [Fact]
    public void UnknownArg_ShouldPrintUsage_AndExit()
    {
        var result = RunProcess("--unknown");
        Assert.Contains("Unknown argument", result.output);
        Assert.Contains("Usage: NumberGuess", result.output);
        Assert.Equal(0, result.exitCode);
    }

    [Fact]
    public void InvalidSeed_ShouldPrintUsage_AndExit()
    {
        var result = RunProcess("--seed nope");
        Assert.Contains("Invalid seed", result.output);
        Assert.Contains("Usage: NumberGuess", result.output);
        Assert.Equal(0, result.exitCode);
    }

    [Fact]
    public void MissingSeedValue_ShouldPrintUsage_AndExit()
    {
        var result = RunProcess("--seed");
        Assert.Contains("Missing value for --seed", result.output);
        Assert.Contains("Usage: NumberGuess", result.output);
        Assert.Equal(0, result.exitCode);
    }

    private static (string output, int exitCode) RunProcess(string args, string? input = null)
    {
        if (!File.Exists(TestAppPath))
        {
            throw new FileNotFoundException($"App not built at {TestAppPath}. Build tests first.");
        }

        var psi = new ProcessStartInfo
        {
            FileName = TestAppPath,
            Arguments = args,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        using var p = Process.Start(psi)!;
        if (input != null)
        {
            p.StandardInput.Write(input);
            p.StandardInput.Flush();
            p.StandardInput.Close();
        }

        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit(5000);
        return (output, p.ExitCode);
    }
}


