using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebTicTacToe.Services;
using WebTicTacToe.Models;

namespace WebTicTacToe.Pages;

public class IndexModel : PageModel
{
    private readonly GameService _svc;

    public IndexModel(GameService svc) => _svc = svc;

    // Exposed state
    public GameState State { get; private set; } = default!;

    public void OnGet()
    {
        State = _svc.GetState();
    }

    public IActionResult OnPostNewGame(bool playAsX = true)
    {
        _svc.NewGame(playAsX);
        return RedirectToPage();
    }

    public IActionResult OnPostMove(int row, int col)
    {
        _svc.ApplyHumanMove(row, col);
        return RedirectToPage();
    }

    public IActionResult OnPostRematch()
    {
        var state = _svc.GetState();
        _svc.NewGame(state.HumanIsX);
        return RedirectToPage();
    }
}
