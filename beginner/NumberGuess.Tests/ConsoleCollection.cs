using Xunit;

// Ensure tests that manipulate global Console state do not run in parallel
[CollectionDefinition("ConsoleTests", DisableParallelization = true)]
public class ConsoleCollection : ICollectionFixture<object>
{
}


