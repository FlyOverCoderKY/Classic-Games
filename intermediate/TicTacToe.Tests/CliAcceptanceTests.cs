using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

public class CliAcceptanceTests
{
    // AppContext.BaseDirectory => .../intermediate/TicTacToe.Tests/bin/Debug/net8.0/
    // Go up 5 levels to reach repo root: Classic-Games
    private static string RepoRoot => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    private static string CliProjectPath => Path.Combine(RepoRoot, "intermediate", "TicTacToe.Cli", "TicTacToe.Cli.csproj");

    [Fact]
    public void Help_Shows_Flags()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" -- --help", input: null, timeoutMs: 15000);
        Assert.Equal(0, result.exitCode);
        Assert.Contains("--start", result.stdout);
        Assert.Contains("--human-mark", result.stdout);
        Assert.Contains("--swap-marks", result.stdout);
    }

    [Fact]
    public void Default_Summary_Line()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" --", input: "q\n", timeoutMs: 15000);
        Assert.Equal(0, result.exitCode);
        Assert.Contains("Starting: Human | Human: X | Bot: O", result.stdout);
    }

    [Fact]
    public void Summary_Line_Reflects_Flags()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" -- --start bot --human-mark O", input: "q\n", timeoutMs: 15000);
        Assert.Equal(0, result.exitCode);
        Assert.Contains("Starting: Bot | Human: O | Bot: X", result.stdout);
    }

    [Fact]
    public void Swap_Marks_Summary_Line()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" -- --swap-marks", input: "q\n", timeoutMs: 15000);
        Assert.Equal(0, result.exitCode);
        Assert.Contains("Human: O | Bot: X", result.stdout);
    }

    [Fact]
    public void HumanMark_Precedence_Over_Swap_Summary_Line()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" -- --swap-marks --human-mark X", input: "q\n", timeoutMs: 15000);
        Assert.Equal(0, result.exitCode);
        Assert.Contains("Human: X | Bot: O", result.stdout);
    }

    [Fact]
    public void Invalid_Start_Exits_NonZero()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" -- --start alice --help", input: null, timeoutMs: 15000);
        Assert.NotEqual(0, result.exitCode);
        Assert.Contains("--start", result.stderr + result.stdout);
    }

    [Fact]
    public void Invalid_HumanMark_Exits_NonZero()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" -- --human-mark Z --help", input: null, timeoutMs: 15000);
        Assert.NotEqual(0, result.exitCode);
        Assert.Contains("--human-mark", result.stderr + result.stdout);
    }

    [Fact]
    public void No_Duplicate_Summary_Line()
    {
        var result = RunProcess("dotnet", $"run --project \"{CliProjectPath}\" --", input: "q\n", timeoutMs: 15000);
        Assert.Equal(0, result.exitCode);
        int count = CountOccurrences(result.stdout, "Starting:");
        Assert.Equal(1, count);
    }

    private static (int exitCode, string stdout, string stderr) RunProcess(string fileName, string arguments, string? input, int timeoutMs)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = input != null,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = RepoRoot
        };

        using var process = new Process { StartInfo = psi };
        var stdout = new StringBuilder();
        var stderr = new StringBuilder();
        process.OutputDataReceived += (_, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (input != null)
        {
            process.StandardInput.Write(input);
            process.StandardInput.Flush();
            process.StandardInput.Close();
        }

        if (!process.WaitForExit(timeoutMs))
        {
            try { process.Kill(true); } catch { }
            return (-1, stdout.ToString(), stderr.ToString());
        }

        return (process.ExitCode, stdout.ToString(), stderr.ToString());
    }

    private static int CountOccurrences(string text, string needle)
    {
        int count = 0, index = 0;
        while ((index = text.IndexOf(needle, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += needle.Length;
        }
        return count;
    }
}


