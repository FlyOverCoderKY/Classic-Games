using TicTacToe.Core;
using Xunit;

namespace TicTacToe.Tests;

public class BoardTests
{
    [Fact]
    public void EmptyBoard_IsInProgress()
    {
        var b = new Board();
        Assert.Equal(GameStatus.InProgress, b.GetStatus());
    }

    [Fact]
    public void XCompletesRow_Wins()
    {
        // Arrange a sequence where X wins on top row.
        var b = new Board();
        b = b.Apply(new Move(0,0,Cell.X));
        b = b.Apply(new Move(1,1,Cell.O));
        b = b.Apply(new Move(0,1,Cell.X));
        b = b.Apply(new Move(2,2,Cell.O));
        b = b.Apply(new Move(0,2,Cell.X));

        Assert.Equal(GameStatus.XWins, b.GetStatus());
    }
}
