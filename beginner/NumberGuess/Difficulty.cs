namespace NumberGuess;

/// <summary>
/// Represents the game difficulty, which determines the inclusive range of possible
/// secret numbers. Higher difficulty increases the range, making the game harder.
/// </summary>
public enum Difficulty
{
    /// <summary>
    /// Easiest setting. Range: 1-50.
    /// </summary>
    Easy,
    /// <summary>
    /// Default setting. Range: 1-100.
    /// </summary>
    Normal,
    /// <summary>
    /// Hardest setting. Range: 1-500.
    /// </summary>
    Hard
}


