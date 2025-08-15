using System;
using Xunit;

public class CliOptionsTests
{
    [Fact]
    public void Defaults_WhenNoArgs()
    {
        var ok = CliOptions.TryParse(Array.Empty<string>(), out var opts, out var err);

        Assert.True(ok);
        Assert.Null(err);
        Assert.NotNull(opts);
        Assert.False(opts!.ShowHelp);
        Assert.Equal(StartingPlayer.Human, opts.StartingPlayer);
        Assert.Equal(TicTacToe.Core.Cell.X, opts.HumanMark);
        Assert.Equal(TicTacToe.Core.Cell.O, opts.BotMark);
        Assert.Equal(opts.HumanMark, opts.StartingMark);
    }

    [Fact]
    public void Start_Bot()
    {
        var ok = CliOptions.TryParse(new[] { "--start", "bot" }, out var opts, out var err);

        Assert.True(ok);
        Assert.Null(err);
        Assert.Equal(StartingPlayer.Bot, opts!.StartingPlayer);
        Assert.Equal(opts.BotMark, opts.StartingMark);
    }

    [Fact]
    public void HumanMark_O()
    {
        var ok = CliOptions.TryParse(new[] { "--human-mark", "O" }, out var opts, out var err);
        Assert.True(ok);
        Assert.Null(err);
        Assert.Equal(TicTacToe.Core.Cell.O, opts!.HumanMark);
        Assert.Equal(TicTacToe.Core.Cell.X, opts.BotMark);
        Assert.Equal(opts.HumanMark, opts.StartingMark);
    }

    [Fact]
    public void SwapMarks_TogglesDefault()
    {
        var ok = CliOptions.TryParse(new[] { "--swap-marks" }, out var opts, out var err);
        Assert.True(ok);
        Assert.Null(err);
        Assert.Equal(TicTacToe.Core.Cell.O, opts!.HumanMark);
        Assert.Equal(TicTacToe.Core.Cell.X, opts.BotMark);
    }

    [Fact]
    public void HumanMark_Precedes_Swap()
    {
        var ok = CliOptions.TryParse(new[] { "--swap-marks", "--human-mark", "X" }, out var opts, out var err);
        Assert.True(ok);
        Assert.Null(err);
        Assert.Equal(TicTacToe.Core.Cell.X, opts!.HumanMark);
        Assert.Equal(TicTacToe.Core.Cell.O, opts.BotMark);
    }

    [Fact]
    public void Invalid_Start_Value_Fails()
    {
        var ok = CliOptions.TryParse(new[] { "--start", "alice" }, out var opts, out var err);
        Assert.False(ok);
        Assert.Null(opts);
        Assert.NotNull(err);
    }

    [Fact]
    public void Invalid_HumanMark_Value_Fails()
    {
        var ok = CliOptions.TryParse(new[] { "--human-mark", "Z" }, out var opts, out var err);
        Assert.False(ok);
        Assert.Null(opts);
        Assert.NotNull(err);
    }
}


