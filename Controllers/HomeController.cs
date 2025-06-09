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
using CoffeHub.Service;
using CoffeHub.Repo;
namespace CoffeHub.Controllers;
public class HomeController : Controller
{
    private readonly ICreateAccount _createAccount;
    private readonly IValidation _validateuser;

    public HomeController(ICreateAccount createAccount, IValidation validateuser)
    {
        this._createAccount = createAccount;
        this._validateuser = validateuser;
    }

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
        if(!_validateuser.IsValidMobileNumber(user.PhoneNumber))
        {
            HelperClass.IsPhoneNumberInvalid = true;
            return View("NewUser");
        }
        if(!_validateuser.IsSamePasswordAgain(user.Password, user.PasswordAgain))
        {
            HelperClass.IsPasswordMismatched = true;
            return View("NewUser");
        }
        if(!_validateuser.IsValidPincode(user.Pincode))
        {
            HelperClass.IsPinInvalid = true;
            return View("NewUser");
        }
        _createAccount.AddUser(user);
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
    public IActionResult ClosePasswordMismatchedPopup()
    {
        HelperClass.IsPasswordMismatched = false;
        return View("NewUser");
    }
    public IActionResult CloseInvalidPinPopUp()
    {
        HelperClass.IsPinInvalid = false;
        return View("NewUser");
    }    
    public IActionResult CloseInvalidPhonenumberPopup()
    {
        HelperClass.IsPhoneNumberInvalid = false;
        return View("NewUser");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
