using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Models;
using LibrarySystem.Data;

namespace LibrarySystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LibraryDbContext _context;

    public HomeController(ILogger<HomeController> logger, LibraryDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // home pagede toplam kitap sayısı, ödünç alınan kitap sayısı ve o anda ödünç alınan kitap sayısını gösterir
        ViewBag.TotalBooks = _context.Books.Count();
        ViewBag.TotalBorrowings = _context.Borrowings.Count();
        ViewBag.ActiveBorrowings = _context.Borrowings.Count(b => b.Status == "Borrowed");

        // home pagedeen son eklenen kitapları gösterir
        var recentBooks = _context.Books
            .OrderByDescending(b => b.Id)
            .Take(15)
            .ToList();

        return View(recentBooks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}