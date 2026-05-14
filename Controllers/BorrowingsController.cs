using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.ViewModels;

namespace LibrarySystem.Controllers;

[Authorize]
public class BorrowingsController : Controller
{
    private readonly LibraryDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BorrowingsController(LibraryDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // tüm borrowing kayıtlarını listeler
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(string status, int page = 1, int pageSize = 10)
    {
        IEnumerable<Borrowing> borrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .OrderByDescending(b => b.BorrowDate)
            .ToListAsync();

        ViewData["Statuses"] = new List<string> { "Borrowed", "Returned", "Overdue" };
        ViewData["Status"] = status;

        if (!string.IsNullOrEmpty(status))
        {
            borrowings = borrowings.Where(b => b.Status == status);
        }

        // İade tarihi geçmiş ancak hala "Borrowed" görünenleri "Overdue" olarak işaretler
        foreach (var borrowing in borrowings.Where(b => b.Status == "Borrowed"))
        {
            if (borrowing.DueDate < DateTime.Now)
            {
                borrowing.Status = "Overdue";
            }
        }
        await _context.SaveChangesAsync();

        var totalBorrowings = borrowings.Count();
        borrowings = borrowings
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalBorrowings / (double)pageSize);

        return View(borrowings);
    }

    //giriş yapan kullanıcının kendi ödünç alma kayıtlarını listeler
    public async Task<IActionResult> MyBorrowings()
    {
        var userId = _userManager.GetUserId(User);

        var borrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BorrowDate)
            .ToListAsync();

        // İade tarihi geçmiş ancak hala "Borrowed" görünenleri "Overdue" olarak işaretler
        foreach (var borrowing in borrowings.Where(b => b.Status == "Borrowed"))
        {
            if (borrowing.DueDate < DateTime.Now)
            {
                borrowing.Status = "Overdue";
            }
        }
        await _context.SaveChangesAsync();

        return View(borrowings);
    }

    //seçilen kitap uygun mu kontrol eder
    public async Task<IActionResult> Borrow(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        if (book.AvailableCopies <= 0)
        {
            // mevcutta kitap yoksa ödünç almaya izin vermez
            TempData["ErrorMessage"] = "Sorry, this book is not available for borrowing.";
            return RedirectToAction("Details", "Books", new { id = book.Id });
        }

        var viewModel = new BorrowBookViewModel
        {
            BookId = book.Id,
            BookTitle = book.Title,
            Author = book.Author,
            AvailableCopies = book.AvailableCopies,
            BorrowDurationDays = 14
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(BorrowBookViewModel model)
    {
        if (ModelState.IsValid)
        {
            var book = await _context.Books.FindAsync(model.BookId);
            if (book == null || book.AvailableCopies <= 0)
            {
                // Kitap bulunamadı veya stok yok
                TempData["ErrorMessage"] = "Book is not available for borrowing.";
                return RedirectToAction("Index", "Books");
            }

            var userId = _userManager.GetUserId(User);

            // Kullanıcı aynı kitabı halihazırda "Borrowed" durumunda tutuyorsa tekrar ödünç alamaz
            var existingBorrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == model.BookId && b.Status == "Borrowed");

            if (existingBorrowing != null)
            {
                TempData["ErrorMessage"] = "You have already borrowed this book.";
                return RedirectToAction("MyBorrowings");
            }

            var borrowing = new Borrowing
            {
                BookId = model.BookId,
                UserId = userId!,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(model.BorrowDurationDays),
                Status = "Borrowed"
            };

            // bir kitap ödünç alındığında mevcut kopya azaltılır
            book.AvailableCopies--;

            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book borrowed successfully!";
            return RedirectToAction(nameof(MyBorrowings));
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrowing == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var isAdmin = User.IsInRole("Admin");

        // kitabı ödünç alan kullanıcı veya Admin dışında kimse iade edemez
        if (borrowing.UserId != userId && !isAdmin)
        {
            return Forbid();
        }

        if (borrowing.Status != "Borrowed" && borrowing.Status != "Overdue")
        {
            TempData["ErrorMessage"] = "This book has already been returned.";
            return RedirectToAction(nameof(MyBorrowings));
        }

        // Borrowing kaydını iade edildi olarak günceller
        borrowing.Status = "Returned";
        borrowing.ReturnDate = DateTime.Now;

        // iade edildiğinde mevcut kopya artırılır
        if (borrowing.Book != null)
        {
            borrowing.Book.AvailableCopies++;
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Book returned successfully!";

        if (isAdmin)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(MyBorrowings));
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (borrowing == null)
        {
            return NotFound();
        }

        return View(borrowing);
    }
}