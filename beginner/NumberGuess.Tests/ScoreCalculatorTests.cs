using NumberGuess;
using Xunit;

namespace NumberGuess.Tests;

public class ScoreCalculatorTests
{
    [Fact]
    public void FewerAttempts_ShouldYieldLowerScore()
    {
        // Arrange
        int range = 100;

        // Act
        int score3 = ScoreCalculator.Calculate(3, range);
        int score7 = ScoreCalculator.Calculate(7, range);

        // Assert
        Assert.True(score3 < score7);
    }

    [Theory]
    [InlineData(1, 50)]
    [InlineData(1, 100)]
    [InlineData(1, 500)]
    public void FirstTry_ShouldBeBest(int attempts, int range)
    {
        var best = ScoreCalculator.Calculate(attempts, range);
        var worse = ScoreCalculator.Calculate(attempts + 5, range);
        Assert.True(best < worse);
    }
}
