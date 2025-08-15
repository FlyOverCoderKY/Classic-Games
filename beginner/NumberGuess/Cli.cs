using System;
using System.IO;

namespace NumberGuess;

public static class Cli
{
    public static int Run(string[] args, TextReader input, TextWriter output)
    {
        Difficulty difficulty = Difficulty.Normal;
        int? seed = null;

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.Equals("--help", StringComparison.OrdinalIgnoreCase) || arg.Equals("-h", StringComparison.OrdinalIgnoreCase))
            {
                PrintUsage(output);
                return 0;
            }

            // Parse --difficulty from either --difficulty=Value or --difficulty Value
            if (arg.StartsWith("--difficulty", StringComparison.OrdinalIgnoreCase))
            {
                string? value = null;
                int eq = arg.IndexOf('=');
                if (eq >= 0)
                {
                    value = arg[(eq + 1)..];
                }
                else if (i + 1 < args.Length)
                {
                    value = args[++i];
                }
                else
                {
                    output.WriteLine("Missing value for --difficulty");
                    PrintUsage(output);
                    return 0;
                }

                if (!TryParseDifficulty(value, out difficulty))
                {
                    output.WriteLine("Invalid difficulty. Use Easy|Normal|Hard.");
                    PrintUsage(output);
                    return 0;
                }

                continue;
            }

            // Parse --seed from either --seed=Value or --seed Value
            if (arg.StartsWith("--seed", StringComparison.OrdinalIgnoreCase))
            {
                string? value = null;
                int eq = arg.IndexOf('=');
                if (eq >= 0)
                {
                    value = arg[(eq + 1)..];
                }
                else if (i + 1 < args.Length)
                {
                    value = args[++i];
                }
                else
                {
                    output.WriteLine("Missing value for --seed");
                    PrintUsage(output);
                    return 0;
                }

                if (int.TryParse(value, out int parsed))
                {
                    seed = parsed;
                }
                else
                {
                    output.WriteLine("Invalid seed. Must be an integer.");
                    PrintUsage(output);
                    return 0;
                }

                continue;
            }

            output.WriteLine($"Unknown argument: {arg}");
            PrintUsage(output);
            return 0;
        }

        output.WriteLine($"Number Guess - Difficulty: {difficulty}" + (seed.HasValue ? $", Seed: {seed.Value}" : string.Empty));

        // Redirect console IO around the game so tests can inject streams
        var originalIn = Console.In;
        var originalOut = Console.Out;
        try
        {
            Console.SetIn(input);
            Console.SetOut(output);

            var game = new Game(difficulty, seed);
            game.Play();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return 0;
    }

    private static void PrintUsage(TextWriter output)
    {
        output.WriteLine("Usage: NumberGuess [--difficulty Easy|Normal|Hard] [--seed <int>]");
    }

    /// <summary>
    /// Attempts to parse a human-friendly difficulty token into the <see cref="Difficulty"/> enum.
    /// Accepts case-insensitive values of "Easy", "Normal", and "Hard".
    /// </summary>
    private static bool TryParseDifficulty(string? value, out Difficulty difficulty)
    {
        if (value is null)
        {
            difficulty = Difficulty.Normal;
            return false;
        }

        if (value.Equals("Easy", StringComparison.OrdinalIgnoreCase))
        {
            difficulty = Difficulty.Easy;
            return true;
        }
        if (value.Equals("Normal", StringComparison.OrdinalIgnoreCase))
        {
            difficulty = Difficulty.Normal;
            return true;
        }
        if (value.Equals("Hard", StringComparison.OrdinalIgnoreCase))
        {
            difficulty = Difficulty.Hard;
            return true;
        }

        difficulty = Difficulty.Normal;
        return false;
    }
}


