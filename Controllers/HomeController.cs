using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CoffeHub.Models;
using CoffeHub.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
namespace CoffeHub.Controllers;
public class HomeController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    [AllowAnonymous]
    public async Task<IActionResult> Menu(UserInfo userinfo)
    {
        Console.WriteLine($"User Authenticated State: {User.Identity?.IsAuthenticated}");
        Console.WriteLine($"User Name: {User.Identity?.Name}");
        if (!ModelState.IsValid)
        {
            return View("Login");
        }
        var claim = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userinfo.Username),
            new Claim(ClaimTypes.Role, userinfo.Role)
        };
        var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = false });
        HttpContext.Session.SetString("CurrentUser", userinfo.Username);
        return View();
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Order()
    {
        return View();
    }
    [Authorize]
    public IActionResult Payment()
    {
        return View();
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
    [Authorize]
    public IActionResult GoBackToAction()
    {
        return RedirectToAction("Payment");
    }
    [AllowAnonymous]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

   
}
