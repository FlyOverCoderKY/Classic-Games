namespace WebTicTacToe.Services;

public static class MinimaxSolver
{
	public static (int row, int col) GetBestMove(char[] board, char currentPlayer)
	{
		if (board is null || board.Length != 9) return (-1, -1);
		char bot = currentPlayer;
		char human = Opponent(bot);

		int bestScore = int.MinValue;
		(int row, int col) bestMove = (-1, -1);
		for (int r = 0; r < 3; r++)
		{
			for (int c = 0; c < 3; c++)
			{
				int idx = r * 3 + c;
				if (board[idx] != ' ') continue;
				board[idx] = bot;
				int score = Minimax(board, isMaximizing: false, depth: 0, bot, human);
				board[idx] = ' ';
				if (score > bestScore)
				{
					bestScore = score;
					bestMove = (r, c);
				}
			}
		}
		return bestMove;
	}

	private static int Minimax(char[] board, bool isMaximizing, int depth, char bot, char human)
	{
		char winner = GetWinner(board);
		if (winner == bot) return 10 - depth;
		if (winner == human) return depth - 10;
		if (IsDraw(board)) return 0;

		if (isMaximizing)
		{
			int best = int.MinValue;
			for (int i = 0; i < 9; i++)
			{
				if (board[i] != ' ') continue;
				board[i] = bot;
				best = Math.Max(best, Minimax(board, false, depth + 1, bot, human));
				board[i] = ' ';
			}
			return best;
		}
		else
		{
			int best = int.MaxValue;
			for (int i = 0; i < 9; i++)
			{
				if (board[i] != ' ') continue;
				board[i] = human;
				best = Math.Min(best, Minimax(board, true, depth + 1, bot, human));
				board[i] = ' ';
			}
			return best;
		}
	}

	private static bool IsDraw(char[] board)
	{
		for (var i = 0; i < 9; i++) if (board[i] == ' ') return false;
		return GetWinner(board) == '\0';
	}

	private static char GetWinner(char[] board)
	{
		int[][] lines = new int[][]
		{
			new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 }, new int[] { 6, 7, 8 },
			new int[] { 0, 3, 6 }, new int[] { 1, 4, 7 }, new int[] { 2, 5, 8 },
			new int[] { 0, 4, 8 }, new int[] { 2, 4, 6 }
		};
		foreach (var line in lines)
		{
			char a = board[line[0]]; char b = board[line[1]]; char c = board[line[2]];
			if (a != ' ' && a == b && b == c) return a;
		}
		return '\0';
	}

	private static char Opponent(char mark) => mark == 'X' ? 'O' : 'X';
}


