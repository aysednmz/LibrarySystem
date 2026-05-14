using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Models;

public class Borrowing
{
    public int Id { get; set; }
    // Ödünç alınan kitabın id'si
    [Required]
    public int BookId { get; set; }

    // Ödünç alan kullanıcının id'si.
    [Required]
    public string UserId { get; set; } = string.Empty;

    // Ödünç alma tarihi
    [Required]
    [Display(Name = "Borrow Date")]
    public DateTime BorrowDate { get; set; }

    // Son iade tarihi
    [Required]
    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; }

    // İade edildiyse iade tarihi
    [Display(Name = "Return Date")]
    public DateTime? ReturnDate { get; set; }

    // Ödünç alma durumu Borrowed, Returned, Overdue mı olduğunu gösterir
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Borrowed";

    public Book? Book { get; set; }

    public IdentityUser? User { get; set; }
}
