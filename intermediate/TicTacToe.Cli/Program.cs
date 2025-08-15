using TicTacToe.Core;

var optionsParseResult = CliOptions.TryParse(args, out var options, out var errorMessage);
if (!optionsParseResult)
{
    Console.Error.WriteLine(errorMessage);
    Console.Error.WriteLine(CliOptions.Usage);
    Environment.Exit(1);
}

if (options!.ShowHelp)
{
    Console.WriteLine(CliOptions.Usage);
    Environment.Exit(0);
}

new GameLoop(options).Run();

// PROMPT: Ask Cursor to extract GameLoop into a class and add undo/redo as stretch goals.
class GameLoop
{
    private Board _board;
    private readonly IBotStrategy _bot = new HeuristicBot();
    private readonly Stack<Board> _history = new();
    private readonly Stack<Board> _redo = new();
    private readonly CliOptions _options;

    public GameLoop(CliOptions options)
    {
        _options = options;
        _board = new Board(options.StartingMark);
    }

    public void Run()
    {
        Console.WriteLine($"Tic-Tac-Toe — Human ({MarkChar(_options.HumanMark)}) vs Bot ({MarkChar(_options.BotMark)})");
        Console.WriteLine($"Starting: {(_board.CurrentPlayer == _options.HumanMark ? "Human" : "Bot")} | Human: {MarkChar(_options.HumanMark)} | Bot: {MarkChar(_options.BotMark)}");

        while (true)
        {
            Render(_board);

            // If it's the bot's turn, let the bot move first
            if (_board.CurrentPlayer == _options.BotMark)
            {
                var botMove = _bot.ChooseMove(_board);
                _history.Push(_board);
                _board = _board.Apply(botMove);

                var postBotStatus = _board.GetStatus();
                if (postBotStatus != GameStatus.InProgress)
                {
                    Render(_board);
                    Console.WriteLine(ResultMessage(postBotStatus));
                    break;
                }

                // Show updated board after bot move before prompting human
                Render(_board);

                // After bot move, continue loop to render and prompt human
            }

            Console.Write("Enter row,col (1-3,1-3), 'u' undo, 'r' redo, or 'q' to quit: ");
            var input = Console.ReadLine();
            if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase)) break;

            if (string.Equals(input, "u", StringComparison.OrdinalIgnoreCase))
            {
                if (_history.Count > 0)
                {
                    _redo.Push(_board);
                    _board = _history.Pop();
                }
                else
                {
                    Console.WriteLine("Nothing to undo.");
                }
                continue;
            }
            if (string.Equals(input, "r", StringComparison.OrdinalIgnoreCase))
            {
                if (_redo.Count > 0)
                {
                    _history.Push(_board);
                    _board = _redo.Pop();
                }
                else
                {
                    Console.WriteLine("Nothing to redo.");
                }
                continue;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid input. Please enter row,col like 2,3.");
                    continue;
                }

                var parts = input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2 || !int.TryParse(parts[0], out int row1) || !int.TryParse(parts[1], out int col1))
                {
                    Console.WriteLine("Invalid format. Use row,col with numbers 1-3.");
                    continue;
                }

                int r = row1 - 1;
                int c = col1 - 1;
                _history.Push(_board);
                _redo.Clear();
                _board = _board.Apply(new Move(r, c, _options.HumanMark));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (_history.Count > 0) _history.Pop();
                continue;
            }

            var status = _board.GetStatus();
            if (status != GameStatus.InProgress)
            {
                Render(_board);
                Console.WriteLine(ResultMessage(status));
                break;
            }

            var botMove2 = _bot.ChooseMove(_board);
            _history.Push(_board);
            _board = _board.Apply(botMove2);

            status = _board.GetStatus();
            if (status != GameStatus.InProgress)
            {
                Render(_board);
                Console.WriteLine(ResultMessage(status));
                break;
            }
        }
    }

    private static void Render(Board b)
    {
        Console.WriteLine();
        Console.WriteLine("    1   2   3");
        for (int r = 0; r < 3; r++)
        {
            Console.Write("{0}   ", r + 1);
            for (int c = 0; c < 3; c++)
            {
                char ch = b[r, c] switch { Cell.X => 'X', Cell.O => 'O', _ => ' ' };
                Console.Write(ch);
                if (c < 2) Console.Write(" | ");
            }
            Console.WriteLine();
            if (r < 2) Console.WriteLine("   ---+---+---");
        }
        Console.WriteLine();
    }

    private char MarkChar(Cell mark) => mark == Cell.X ? 'X' : 'O';

    private string ResultMessage(GameStatus status)
    {
        if (status == GameStatus.Draw) return "Draw.";
        var humanWon = (status == GameStatus.XWins && _options.HumanMark == Cell.X) || (status == GameStatus.OWins && _options.HumanMark == Cell.O);
        return humanWon ? "You win!" : "Bot wins!";
    }
}

public enum StartingPlayer
{
    Human,
    Bot
}

public record CliOptions(
    bool ShowHelp,
    StartingPlayer StartingPlayer,
    Cell HumanMark
)
{
    public static string Usage =>
        "Usage: tic-tac-toe [--start {human|bot}] [--human-mark {X|O}] [--swap-marks] [--help]\n" +
        "\n" +
        "Options:\n" +
        "  -s, --start <human|bot>     Choose who starts (default: human).\n" +
        "  -m, --human-mark <X|O>      Choose human's mark (default: X).\n" +
        "  -w, --swap-marks            Convenience toggle to swap X/O (ignored if --human-mark is specified).\n" +
        "  -h, --help                  Show this help.\n";

    public Cell BotMark => HumanMark == Cell.X ? Cell.O : Cell.X;
    public Cell StartingMark => StartingPlayer == StartingPlayer.Human ? HumanMark : BotMark;

    public static bool TryParse(string[] args, out CliOptions? options, out string? error)
    {
        options = null;
        error = null;

        bool showHelp = false;
        StartingPlayer starting = StartingPlayer.Human;
        Cell humanMark = Cell.X;
        bool humanMarkSpecified = false;
        bool swap = false;

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-h":
                case "--help":
                    showHelp = true;
                    break;
                case "-s":
                case "--start":
                    if (i + 1 >= args.Length)
                    {
                        error = "--start requires a value: human or bot.";
                        return false;
                    }
                    var who = args[++i].ToLowerInvariant();
                    if (who == "human") starting = StartingPlayer.Human;
                    else if (who == "bot") starting = StartingPlayer.Bot;
                    else
                    {
                        error = "Invalid value for --start. Use human or bot.";
                        return false;
                    }
                    break;
                case "-m":
                case "--human-mark":
                    if (i + 1 >= args.Length)
                    {
                        error = "--human-mark requires a value: X or O.";
                        return false;
                    }
                    var mark = args[++i];
                    if (string.Equals(mark, "X", StringComparison.OrdinalIgnoreCase)) humanMark = Cell.X;
                    else if (string.Equals(mark, "O", StringComparison.OrdinalIgnoreCase)) humanMark = Cell.O;
                    else
                    {
                        error = "Invalid value for --human-mark. Use X or O.";
                        return false;
                    }
                    humanMarkSpecified = true;
                    break;
                case "-w":
                case "--swap-marks":
                    swap = true;
                    break;
                default:
                    // Ignore unrecognized args for now to remain backward compatible
                    break;
            }
        }

        if (swap && !humanMarkSpecified)
        {
            humanMark = humanMark == Cell.X ? Cell.O : Cell.X;
        }

        options = new CliOptions(showHelp, starting, humanMark);
        return true;
    }
}
