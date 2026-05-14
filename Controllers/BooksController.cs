using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

public class BooksController : Controller
{
    private readonly LibraryDbContext _context;

    public BooksController(LibraryDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? q, string? category)
    {
        // arama ve kategori filtresi search yapar
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            // kitap adı veya yazara göre basit metin araması
            query = query.Where(b => b.Title.Contains(q) || b.Author.Contains(q));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            // kategoriye göre filtreleme
            query = query.Where(b => b.Category == category);
        }

        ViewData["Query"] = q;
        ViewData["Category"] = category;
        ViewData["Categories"] = await _context.Books
            .Where(b => b.Category != null && b.Category != "")
            .Select(b => b.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        var books = await query.OrderBy(b => b.Title).ToListAsync();
        return View(books);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        // veritabanından kitabı getirir
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        // Sadece Admin rolü yeni kitap ekleyebilir
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        // borrow için mevcut kopya sayısı toplam kitap sayısından fazla olamaz
        if (book.AvailableCopies > book.TotalCopies)
        {
            ModelState.AddModelError(nameof(Book.AvailableCopies), "Available copies cannot exceed total copies.");
        }

        if (ModelState.IsValid)

        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Book created.";
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        // kitabı düzenlemek için veritabanından getirir
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book)
    {

        if (id != book.Id)
        {
            return NotFound();
        }


        if (book.AvailableCopies > book.TotalCopies)
        {
            ModelState.AddModelError(nameof(Book.AvailableCopies), "Available copies cannot exceed total copies.");
        }

        if (ModelState.IsValid)
        {
            try
            {

                _context.Update(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book updated.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        // Silme onay sayfası
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // kayıt varsa siler ve başarı mesajı yazar
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Book deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}
