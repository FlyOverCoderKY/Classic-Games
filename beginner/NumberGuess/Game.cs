namespace NumberGuess;

/// <summary>
/// Encapsulates the state and interaction loop for a single NumberGuess game session.
/// Manages the secret number, guess validation, user feedback, and score tracking
/// across rounds. Use <see cref="Play"/> to run the synchronous console loop.
/// </summary>
public class Game
{
    private readonly Difficulty _difficulty;
    private readonly Random _random;
    private int _secret;
    private int _lower;
    private int _upper;
    private bool _running;
    private int _attempts;
    private int _bestScore = int.MaxValue;
    private int? _previousDistance;

    /// <summary>
    /// Creates a new game with the desired difficulty and optional random seed.
    /// </summary>
    /// <param name="difficulty">Determines the range of the secret number.</param>
    /// <param name="seed">Optional seed to make the game deterministic (useful for testing).</param>
    public Game(Difficulty difficulty, int? seed = null)
    {
        _difficulty = difficulty;
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
        ConfigureRange();
        ResetSecret();
    }

    /// <summary>
    /// Picks a new secret and resets per-round state.
    /// </summary>
    private void ResetSecret()
    {
        _secret = _random.Next(_lower, _upper + 1);
        _attempts = 0;
        _running = true;
        _previousDistance = null;
    }

    /// <summary>
    /// Sets the inclusive bounds of the secret number based on the selected difficulty.
    /// </summary>
    private void ConfigureRange()
    {
        switch (_difficulty)
        {
            case Difficulty.Easy:
                _lower = 1;
                _upper = 50;
                return;
            case Difficulty.Normal:
                _lower = 1;
                _upper = 100;
                return;
            case Difficulty.Hard:
                _lower = 1;
                _upper = 500;
                return;
            default:
                _lower = 1;
                _upper = 100;
                return;
        }
    }

    /// <summary>
    /// Runs the interactive console loop until the user quits or declines to play again.
    /// </summary>
    public void Play()
    {
        Console.WriteLine($"I'm thinking of a number between {_lower} and {_upper}. Type 'quit' to exit.");

        while (_running)
        {
            Console.Write($"Enter your guess [{_lower}-{_upper}]: ");
            var input = Console.ReadLine();
            if (input is null)
            {
                Console.WriteLine("Goodbye!");
                _running = false;
                break;
            }

            input = input.Trim();
            if (input.Length == 0)
            {
                Console.WriteLine("Please enter a number.");
                continue;
            }

            if (string.Equals(input, "quit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Goodbye!");
                _running = false;
                break;
            }

            if (!int.TryParse(input, out var guess))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                continue;
            }

            if (guess < _lower || guess > _upper)
            {
                Console.WriteLine($"Out of range. Please enter a number between {_lower} and {_upper}.");
                continue;
            }

            _attempts++;

            if (guess == _secret)
            {
                Console.WriteLine($"Correct! You got it in {_attempts} attempt(s).");

                var rangeSize = _upper - _lower + 1;
                int score = ScoreCalculator.Calculate(_attempts, rangeSize);

                if (score < _bestScore)
                {
                    _bestScore = score;
                    Console.WriteLine($"New best score: {_bestScore} (lower is better)");
                }
                else
                {
                    Console.WriteLine($"Score: {score}. Best: {_bestScore} (lower is better)");
                }

                Console.Write("Play again? (y/n): ");
                var again = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(again) && again.Trim().StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    ResetSecret();
                    Console.WriteLine($"New round! Guess a number between {_lower} and {_upper}.");
                    continue;
                }

                _running = false;
                Console.WriteLine("Thanks for playing!");
                break;
            }

            string hint = GetHint(guess);
            Console.WriteLine(hint);
        }
    }

    /// <summary>
    /// Provides a directional hint and trend relative to the previous guess.
    /// First hint includes direction only; subsequent hints also include a
    /// warmer/colder indicator based on distance to the secret.
    /// </summary>
    /// <param name="lastGuess">The most recent guess provided by the user.</param>
    /// <returns>Human-readable hint such as "Warmer! Too low. Try higher.".</returns>
    private string GetHint(int lastGuess)
    {
        int currentDistance = Math.Abs(lastGuess - _secret);

        if (!_previousDistance.HasValue)
        {
            _previousDistance = currentDistance;
            return lastGuess < _secret ? "Too low. Try higher." : "Too high. Try lower.";
        }

        int previousDistance = _previousDistance.Value;
        string trend;
        if (currentDistance < previousDistance)
        {
            trend = "Warmer!";
        }
        else if (currentDistance > previousDistance)
        {
            trend = "Colder!";
        }
        else
        {
            trend = string.Empty;
        }

        _previousDistance = currentDistance;

        string direction = lastGuess < _secret ? "Too low. Try higher." : "Too high. Try lower.";
        return string.IsNullOrEmpty(trend) ? direction : $"{trend} {direction}";
    }
}
