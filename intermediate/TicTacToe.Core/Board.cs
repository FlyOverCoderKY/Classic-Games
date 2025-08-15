namespace TicTacToe.Core;

public enum Cell { Empty, X, O }
public enum GameStatus { InProgress, XWins, OWins, Draw }

public record Move(int Row, int Col, Cell Player);

public class Board
{
    private readonly Cell[,] _cells = new Cell[3,3];
    public Cell this[int r, int c] => _cells[r,c];
    public Cell CurrentPlayer { get; private set; } = Cell.X;

    public Board()
    {
    }

    public Board(Cell startingPlayer)
    {
        if (startingPlayer == Cell.Empty) throw new ArgumentException("Starting player must be X or O", nameof(startingPlayer));
        CurrentPlayer = startingPlayer;
    }

    // PROMPT: Ask Cursor to implement immutable Apply(Move) that returns a NEW Board with the move applied,
    // throws on invalid move, and flips CurrentPlayer.
    public Board Apply(Move move)
    {
        if (move.Row < 0 || move.Row > 2 || move.Col < 0 || move.Col > 2)
            throw new ArgumentOutOfRangeException("Move is out of bounds");

        if (move.Player != CurrentPlayer)
            throw new InvalidOperationException("It is not this player's turn");

        if (_cells[move.Row, move.Col] != Cell.Empty)
            throw new InvalidOperationException("Cell is already occupied");

        var next = new Board();
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                next._cells[r, c] = _cells[r, c];
            }
        }

        next._cells[move.Row, move.Col] = move.Player;
        next.CurrentPlayer = move.Player == Cell.X ? Cell.O : Cell.X;
        return next;
    }

    // PROMPT: Ask Cursor to implement GetStatus() that checks rows, cols, diags, and draw.
    public GameStatus GetStatus()
    {
        // Check rows and columns
        for (int i = 0; i < 3; i++)
        {
            // Rows
            if (_cells[i,0] != Cell.Empty && _cells[i,0] == _cells[i,1] && _cells[i,1] == _cells[i,2])
                return _cells[i,0] == Cell.X ? GameStatus.XWins : GameStatus.OWins;

            // Columns
            if (_cells[0,i] != Cell.Empty && _cells[0,i] == _cells[1,i] && _cells[1,i] == _cells[2,i])
                return _cells[0,i] == Cell.X ? GameStatus.XWins : GameStatus.OWins;
        }

        // Diagonals
        if (_cells[1,1] != Cell.Empty)
        {
            if ((_cells[0,0] == _cells[1,1] && _cells[1,1] == _cells[2,2]) ||
                (_cells[0,2] == _cells[1,1] && _cells[1,1] == _cells[2,0]))
            {
                return _cells[1,1] == Cell.X ? GameStatus.XWins : GameStatus.OWins;
            }
        }

        // Draw or InProgress
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (_cells[r,c] == Cell.Empty)
                    return GameStatus.InProgress;
            }
        }
        return GameStatus.Draw;
    }

    // PROMPT: Ask Cursor to implement GetEmptyCells() helper returning IEnumerable<(int r,int c)>.
    public IEnumerable<(int r, int c)> GetEmptyCells()
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (_cells[r, c] == Cell.Empty)
                {
                    yield return (r, c);
                }
            }
        }
    }
}
