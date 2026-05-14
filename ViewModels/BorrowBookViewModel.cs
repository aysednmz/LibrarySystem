using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.ViewModels;

public class BorrowBookViewModel
{
    // Formdan gelen kitap id'si
    [Required]
    public int BookId { get; set; }

    // View'da gösterim amaçlı alanlar
    public string BookTitle { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    // mevcut kitap sayısı
    public int AvailableCopies { get; set; }

    // Kullanıcının seçebileceği ödünç alma süresi
    [Required]
    [Range(1, 30)]
    [Display(Name = "Borrow Duration (Days)")]
    public int BorrowDurationDays { get; set; } = 14;
}
