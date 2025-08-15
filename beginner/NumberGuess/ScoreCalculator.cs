namespace NumberGuess;

/// <summary>
/// Provides a simple scoring heuristic for NumberGuess. Lower scores are better.
/// Attempts are weighted heavily to ensure that fewer attempts always produce a
/// better score than more attempts, regardless of difficulty. A small difficulty
/// bonus slightly reduces the score for larger ranges so harder games can still
/// be compared while preserving attempt ordering.
/// </summary>
public static class ScoreCalculator
{
    /// <summary>
    /// Calculates a score given the number of attempts and the size of the range.
    /// </summary>
    /// <param name="attempts">Number of guesses taken to find the secret. Must be positive.</param>
    /// <param name="rangeSize">Inclusive range size (upper - lower + 1). Must be positive.</param>
    /// <returns>An integer score; lower values indicate better performance.</returns>
    public static int Calculate(int attempts, int rangeSize)
    {
        // Guard against invalid inputs without mutating parameters
        int safeAttempts = attempts < 1 ? 1 : attempts;
        int safeRange = rangeSize < 1 ? 1 : rangeSize;

        // Preserve attempt ordering as the dominant factor by weighting attempts heavily.
        // A 1-attempt difference should always outweigh any difficulty bonus across practical ranges.
        // Using log2(range) provides a modest bonus for larger ranges while keeping numbers readable.
        int difficultyBonus = (int)Math.Floor(Math.Log2(safeRange));

        int score = safeAttempts * 100 - difficultyBonus;
        return score < 1 ? 1 : score;
    }
}
