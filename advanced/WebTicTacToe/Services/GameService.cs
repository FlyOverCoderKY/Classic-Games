using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using WebTicTacToe.Models;

namespace WebTicTacToe.Services;

public class GameService
{
	private const string SessionKey = "WebTicTacToe.GameState";
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly JsonSerializerOptions _jsonOptions = new()
	{
		WriteIndented = false,
		Converters = { new JsonStringEnumConverter() }
	};

	public GameService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public GameState GetState()
	{
		var session = _httpContextAccessor.HttpContext!.Session;
		var json = session.GetString(SessionKey);
		if (string.IsNullOrEmpty(json))
		{
			var state = new GameState();
			SaveState(state);
			return state;
		}
		return JsonSerializer.Deserialize<GameState>(json, _jsonOptions) ?? new GameState();
	}

	private void SaveState(GameState state)
	{
		var session = _httpContextAccessor.HttpContext!.Session;
		session.SetString(SessionKey, JsonSerializer.Serialize(state, _jsonOptions));
	}

	public void NewGame(bool playAsX)
	{
		var state = GetState();
		for (var i = 0; i < state.Board.Length; i++) state.Board[i] = ' ';
		state.HumanIsX = playAsX;
		state.CurrentPlayer = 'X';
		state.Status = GameStatus.InProgress;
		SaveState(state);

		// If human plays as O, bot goes first
		if (!state.HumanIsX)
		{
			ApplyBotMove();
		}
	}

	public void ResetScoreboard()
	{
		var state = GetState();
		state.Score = new Scoreboard();
		SaveState(state);
	}

	public void ApplyHumanMove(int row, int col)
	{
		var state = GetState();
		if (state.Status != GameStatus.InProgress) return;
		var humanMark = state.HumanIsX ? 'X' : 'O';
		if (state.CurrentPlayer != humanMark) return;
		var idx = GameState.Index(row, col);
		if (idx < 0 || idx >= 9) return;
		if (state.Board[idx] != ' ') return;

		state.Board[idx] = humanMark;
		UpdateStatus(state);
		if (state.Status == GameStatus.InProgress)
		{
			state.CurrentPlayer = Opponent(state.CurrentPlayer);
			SaveState(state);
			ApplyBotMove();
		}
		else
		{
			SaveState(state);
		}
	}

	public void ApplyBotMove()
	{
		var state = GetState();
		if (state.Status != GameStatus.InProgress) return;
		var botMark = state.HumanIsX ? 'O' : 'X';
		if (state.CurrentPlayer != botMark) return;

		var best = FindBestMove(state.Board, botMark, Opponent(botMark));
		if (best.row >= 0 && best.col >= 0)
		{
			state.SetCell(best.row, best.col, botMark);
		}
		UpdateStatus(state);
		if (state.Status == GameStatus.InProgress)
		{
			state.CurrentPlayer = Opponent(state.CurrentPlayer);
		}
		SaveState(state);
	}

	private static char Opponent(char mark) => mark == 'X' ? 'O' : 'X';

	private void UpdateStatus(GameState state)
	{
		var winner = GetWinner(state.Board);
		if (winner == 'X')
		{
			state.Status = GameStatus.XWon;
			state.Score.XWins++;
			return;
		}
		if (winner == 'O')
		{
			state.Status = GameStatus.OWon;
			state.Score.OWins++;
			return;
		}
		if (IsDraw(state.Board))
		{
			state.Status = GameStatus.Draw;
			state.Score.Draws++;
			return;
		}
		state.Status = GameStatus.InProgress;
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
			var a = board[line[0]]; var b = board[line[1]]; var c = board[line[2]];
			if (a != ' ' && a == b && b == c) return a;
		}
		return '\0';
	}

	private (int row, int col) FindBestMove(char[] board, char bot, char human)
	{
		int bestScore = int.MinValue;
		(int row, int col) bestMove = (-1, -1);
		for (int r = 0; r < 3; r++)
		{
			for (int c = 0; c < 3; c++)
			{
				int idx = r * 3 + c;
				if (board[idx] != ' ') continue;
				board[idx] = bot;
				int score = Minimax(board, false, 0, bot, human);
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

	private int Minimax(char[] board, bool isMaximizing, int depth, char bot, char human)
	{
		var winner = GetWinner(board);
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
}
