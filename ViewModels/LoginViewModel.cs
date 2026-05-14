using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.ViewModels;

public class LoginViewModel
{
    // Kullanıcı adı
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    // şifre
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
