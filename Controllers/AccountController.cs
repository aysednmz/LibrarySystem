using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LibrarySystem.ViewModels;

namespace LibrarySystem.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login(string? ReturnUrl)
    {
        // Login sayfasını gösterir
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
    {
        // Login POST: kullanıcı adı/şifre doğrular ve başarılıysa cookie oluşturur
        if (!ModelState.IsValid)
            return View(model);

        // Kullanıcı adından kullanıcıyı bulur
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user != null)
        {
            // şifre doğruysa oturum açar
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);

                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        // çıkış yapar
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        // Kayıt sayfasını gösterir
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        // kullanıcı oluşturur, e-posta doğrulamak için kullanılır
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Doğrulama linkini üretir 
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = token },
                    protocol: Request.Scheme
                );

                TempData["ConfirmationLink"] = confirmationLink;

                // yeni kullanıcılar "Member" rolünde başlar
                await _userManager.AddToRoleAsync(user, "Member");

                return RedirectToAction("RegisterSuccess", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult RegisterSuccess()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        // E-posta doğrulamasını yapar
        if (userId == null || token == null)
            return View("ConfirmEmailSuccess", "Invalid confirmation link.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return View("ConfirmEmailSuccess", "User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
            return View("ConfirmEmailSuccess", "Your email has been successfully confirmed.");

        return View("ConfirmEmailSuccess", "Email confirmation failed.");
    }

    public IActionResult AccessDenied()
    {
        // giriş reddedildi sayfasını gösterir
        return View();
    }
}