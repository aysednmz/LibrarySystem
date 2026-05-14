using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.ViewModels;

public class RegisterViewModel
{
    // Kayıt olacak kullanıcının kullanıcı adı
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    // E-posta adresi
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Şifre
    [Required]
    [DataType(DataType.Password)]
    [MinLength(1)]
    public string Password { get; set; } = string.Empty;

    // şifre doğrulaması Password ile aynı olmalı
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
