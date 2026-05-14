using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

public class Book
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    [Display(Name = "Kitap Başlığı")]
    public string Title { get; set; } = string.Empty;

    // Kitabın yazarı 
    [Required, StringLength(150)]
    [Display(Name = "Yazar")]
    public string Author { get; set; } = string.Empty;

    [Display(Name = "Yayın Yılı")]
    [Range(1000, 2100)]
    public int? PublishedYear { get; set; }

    [StringLength(50)]
    [Display(Name = "Kategori")]
    public string? Category { get; set; }

    [StringLength(2000)]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Toplam Kopya Sayısı")]
    [Range(1, 100)]
    public int TotalCopies { get; set; } = 1;

    [Required]
    [Display(Name = "Mevcut Kopya Sayısı")]
    [Range(0, 100)]
    public int AvailableCopies { get; set; } = 1;

    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}