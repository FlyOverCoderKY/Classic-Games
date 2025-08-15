namespace TicTacToe.Core;

public class HeuristicBot : IBotStrategy
{
    // PROMPT: Ask Cursor to implement a simple heuristic:
    // 1) If winning move exists, take it.
    // 2) If opponent can win next, block it.
    // 3) Prefer center, then corners, then edges.
    public Move ChooseMove(Board board)
    {
        var my = board.CurrentPlayer;
        var opp = my == Cell.X ? Cell.O : Cell.X;

        // 1) If winning move exists, take it
        foreach (var (r, c) in board.GetEmptyCells())
        {
            var after = board.Apply(new Move(r, c, my));
            var status = after.GetStatus();
            if ((my == Cell.X && status == GameStatus.XWins) || (my == Cell.O && status == GameStatus.OWins))
                return new Move(r, c, my);
        }

        // 2) If opponent can win next, block it
        // 2) If opponent can win next, block it
        foreach (var (r, c) in board.GetEmptyCells())
        {
            // Simulate opponent taking (r,c) on their turn by first making a no-op of our move to flip turn,
            // but easier: create a hypothetical board where opponent plays now by constructing via Apply with opp as player
            // This is legal only if it's their legal turn; ensure we simulate correctly by making a temporary board with same cells and CurrentPlayer set to opp via a move by my then undo — but undo not available.
            // Simpler approach: try our move somewhere else is unnecessary; instead, temporarily treat move as opponent by crafting from current board a board with same cells and CurrentPlayer=opp by applying a dummy and reverting is complex.
            // Since Apply enforces player equals CurrentPlayer, we simulate opponent's next by imagining board after our hypothetical pass: after we move, could they win? That is expensive.
            // Pragmatic approach: check if placing opp at (r,c) from current layout creates a line with two existing opp and empty here.

            // Check if opponent would win by playing at (r,c)
            // Temporarily pretend they play there by copying logic similar to Apply but not changing turn/state
            bool oppWinsHere = WouldWin(board, r, c, opp);
            if (oppWinsHere)
                return new Move(r, c, my);
        }

        // Fallback priorities: center, corners, edges
        var center = (1, 1);
        if (board[center.Item1, center.Item2] == Cell.Empty)
            return new Move(center.Item1, center.Item2, my);

        var corners = new (int r, int c)[] { (0,0), (0,2), (2,0), (2,2) };
        foreach (var (r, c) in corners)
        {
            if (board[r, c] == Cell.Empty)
                return new Move(r, c, my);
        }

        var edges = new (int r, int c)[] { (0,1), (1,0), (1,2), (2,1) };
        foreach (var (r, c) in edges)
        {
            if (board[r, c] == Cell.Empty)
                return new Move(r, c, my);
        }

        // Should not happen if board has empty cells; if not, default (0,0)
        return new Move(0, 0, my);
    }

    private static bool WouldWin(Board board, int r, int c, Cell player)
    {
        if (board[r, c] != Cell.Empty) return false;

        // Row
        if ((board[r, 0] == player || (c == 0)) &&
            (board[r, 1] == player || (c == 1)) &&
            (board[r, 2] == player || (c == 2)))
        {
            int count = 0;
            for (int cc = 0; cc < 3; cc++)
                if ((cc == c ? player : board[r, cc]) == player) count++;
            if (count == 3) return true;
        }

        // Column
        if ((board[0, c] == player || (r == 0)) &&
            (board[1, c] == player || (r == 1)) &&
            (board[2, c] == player || (r == 2)))
        {
            int count = 0;
            for (int rr = 0; rr < 3; rr++)
                if ((rr == r ? player : board[rr, c]) == player) count++;
            if (count == 3) return true;
        }

        // Diagonals
        if (r == c)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
                if ((i == r ? player : board[i, i]) == player) count++;
            if (count == 3) return true;
        }
        if (r + c == 3 - 1)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
                if (((i == r && (3 - 1 - i) == c) ? player : board[i, 3 - 1 - i]) == player) count++;
            if (count == 3) return true;
        }
        return false;
    }
}
