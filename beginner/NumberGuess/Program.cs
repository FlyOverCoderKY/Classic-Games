using NumberGuess;

// Minimal command-line host for the NumberGuess game. Supports optional flags:
//   --difficulty Easy|Normal|Hard  set the range of the secret number (default: Normal)
//   --seed <int>                   seed the RNG for deterministic runs (testing/debugging)
return Cli.Run(args, Console.In, Console.Out);
