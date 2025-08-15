using Xunit;

namespace NumberGuess.Tests;

public class ScoreCalculatorAdditionalTests
{
    [Fact]
    public void AttemptOrdering_IsAlwaysPreserved()
    {
        // Across a wide range of difficulties, a lower attempt count must yield a lower score
        for (int range = 1; range <= 2000; range *= 2)
        {
            int score1 = ScoreCalculator.Calculate(3, range);
            int score2 = ScoreCalculator.Calculate(4, range);
            Assert.True(score1 < score2, $"Expected 3 attempts < 4 attempts for range {range}");
        }
    }

    [Fact]
    public void DifficultyBonus_MakesHarderRangesScoreSlightlyBetter()
    {
        int attempts = 5;
        int easy = ScoreCalculator.Calculate(attempts, 50);
        int hard = ScoreCalculator.Calculate(attempts, 500);
        Assert.True(hard < easy);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, -5)]
    [InlineData(1, 1)]
    public void Guards_HandleInvalidInputs(int attempts, int range)
    {
        int score = ScoreCalculator.Calculate(attempts, range);
        Assert.True(score >= 1);
    }
}


