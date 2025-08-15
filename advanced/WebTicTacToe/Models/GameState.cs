namespace WebTicTacToe.Models;

// Serializable game state stored in session
public enum GameStatus
{
	InProgress,
	XWon,
	OWon,
	Draw
}

public class Scoreboard
{
	public int XWins { get; set; }
	public int OWins { get; set; }
	public int Draws { get; set; }
}

public class GameState
{
	// 3x3 board flattened to 9 cells: 'X', 'O', or ' ' (space)
	public char[] Board { get; set; } = new char[9];

	// True if human plays as X, otherwise human is O
	public bool HumanIsX { get; set; } = true;

	// Current player to move: 'X' or 'O'
	public char CurrentPlayer { get; set; } = 'X';

	// Current game status
	public GameStatus Status { get; set; } = GameStatus.InProgress;

	// Session scoreboard
	public Scoreboard Score { get; set; } = new Scoreboard();

	public GameState()
	{
		// Initialize board with spaces for clarity in UI
		for (var i = 0; i < Board.Length; i++) Board[i] = ' ';
	}

	public static int Index(int row, int col) => row * 3 + col;

	public char GetCell(int row, int col) => Board[Index(row, col)];

	public void SetCell(int row, int col, char value) => Board[Index(row, col)] = value;
}
