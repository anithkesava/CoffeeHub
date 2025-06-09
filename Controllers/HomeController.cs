using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CoffeHub.Models;
using CoffeHub.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CoffeHub.Helper;
namespace CoffeHub.Controllers;
public class HomeController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Menu(UserDTO user)
    {
        if (!ModelState.IsValid)
        {
            return View("Login");
        }
        TempData["Username"] = user.Username;
        return View();
    }

    public IActionResult NewUser()
    {
        return View();
    }

    public IActionResult CreateAccount(UserDetails user)
    {
        if (!ModelState.IsValid)
        {
            HelperClass.IsRequiredFieldEmpty = true;
            return View("NewUser");
        }
        HelperClass.IsAccountCreated = true;
        return View("NewUser");
    }
    public IActionResult CloseRequiredFieldPopup()
    {
        HelperClass.IsRequiredFieldEmpty = false;        
        return View("NewUser");
    }
    public IActionResult CloseAccountCreatedPopup()
    {
        HelperClass.IsAccountCreated = false;
        return View("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
