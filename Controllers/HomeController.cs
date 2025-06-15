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
using System.Security.AccessControl;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
namespace CoffeHub.Controllers;
public class HomeController : Controller
{
    private static List<FoodItems> foodItems = new List<FoodItems>();
    private static List<ViewCarts> ViewCartList = new List<ViewCarts>();
    private static List<UserDetails> UserDetailsList = new List<UserDetails>();
    private readonly ICreateAccount _createAccount;
    private readonly IValidation _validateuser;
    private readonly AppDbContext _appDbContext;
    public HomeController(ICreateAccount createAccount, IValidation validateuser, AppDbContext appDbContext)
    {
        this._createAccount = createAccount;
        this._validateuser = validateuser;
        this._appDbContext = appDbContext;
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult CloseUserNotExistsPopup()
    {
        HelperClass.IsUserNotExists = false;
        return View("Login");
    }
    public IActionResult Menu(UserDTO user)
    {
        if (!ModelState.IsValid)
        {
            return View("Login");
        }
        bool isuserexists = _validateuser.IsUserExists(user.Username, user.Password);
        if (!isuserexists)
        {
            HelperClass.IsUserNotExists = true;
            return View("Login");
        }
        TempData["Username"] = user.Username;
        HttpContext.Session.SetString("username", user.Username);
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
        if (!_validateuser.IsValidMobileNumber(user.PhoneNumber))
        {
            HelperClass.IsPhoneNumberInvalid = true;
            return View("NewUser");
        }
        if (!_validateuser.IsSamePasswordAgain(user.Password, user.PasswordAgain))
        {
            HelperClass.IsPasswordMismatched = true;
            return View("NewUser");
        }
        if (!_validateuser.IsValidPincode(user.Pincode))
        {
            HelperClass.IsPinInvalid = true;
            return View("NewUser");
        }
        if (_validateuser.IsUserExists(user.Username, user.PasswordAgain))
        {
            HelperClass.IsUserAlreadyExists = true;
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
    public IActionResult CloseUserAlreadyExistsPopup()
    {
        HelperClass.IsUserAlreadyExists = false;
        return View("NewUser");
    }

    public List<FoodItems> LoadFoodItemsWithCartState()
    {
        var foodItems = _appDbContext.FoodItems.ToList();
        var sessionCartJson = HttpContext.Session.GetString("Cart");
        if (!string.IsNullOrEmpty(sessionCartJson))
        {
            var cartList = JsonConvert.DeserializeObject<List<ViewCarts>>(sessionCartJson);
            if (cartList != null)
            {
                foreach (var item in foodItems)
                {
                    var matchedCart = cartList.FirstOrDefault(c => c.FoodName == item.FoodName);
                    if (matchedCart != null)
                    {
                        item.IsAddtoCartClicked = matchedCart.IsAddtoCartClicked;
                        item.CartCount = matchedCart.CartCount;
                    }
                }
            }
        }
        return foodItems;
    }
    public void UpdateSessionCart()
    {
        var viewCartList = _appDbContext.ViewCarts.ToList();
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(viewCartList));
    }

    public IActionResult BestSeller()
    {
        foodItems = LoadFoodItemsWithCartState();
        return View(foodItems);
    }
    public IActionResult AddToCartClicked(string foodname)
    {
        var food = foodItems.FirstOrDefault(x => x.FoodName == foodname);
        if (food != null)
        {
            food.IsAddtoCartClicked = true;
            food.CartCount = 1;
            HelperClass.CanIShowViewCart = true;
            HelperClass.OverAllQuantity = (HelperClass.OverAllQuantity ?? 0) + food.CartCount; //TODO: first problem identified
            HelperClass.TotalPrice = HelperClass.TotalPrice + food.FoodPrice * food.CartCount ?? 0;

            /*
            TODO: OverallPrice need to Calculate.             
            */

            ViewCarts viewCart = new ViewCarts
            {
                FoodName = food.FoodName,
                FoodPrice = food.FoodPrice,
                CartCount = food.CartCount,
                IsAddtoCartClicked = food.IsAddtoCartClicked,
                Quantity = food.Quantity
            };
            _appDbContext.ViewCarts.Add(viewCart);
            _appDbContext.SaveChanges();
        }
        UpdateSessionCart();
        return RedirectToAction("BestSeller");
    }
    public IActionResult Plus(string foodname)
    {
        var food = foodItems.Where(x => x.FoodName == foodname).FirstOrDefault();
        if (food != null)
        {
            if (food.CartCount < food.Quantity)
            {
                food.CartCount++;
                var item = _appDbContext.ViewCarts.Where(x => x.FoodName == foodname).FirstOrDefault();
                if (item != null)
                {
                    item.CartCount = food.CartCount;
                    _appDbContext.SaveChanges();
                }
                HelperClass.OverAllQuantity = (HelperClass.OverAllQuantity ?? 0) + 1;
                HelperClass.TotalPrice = HelperClass.TotalPrice + (1 * food.FoodPrice);
            }
        }
        UpdateSessionCart();
        return RedirectToAction("BestSeller");
    }
    public IActionResult Minus(string foodname)
    {
        var food = foodItems.Where(x => x.FoodName == foodname).FirstOrDefault();
        if (food != null)
        {
            food.CartCount--;
            HelperClass.OverAllQuantity = HelperClass.OverAllQuantity - 1;
            var item = _appDbContext.ViewCarts.Where(x => x.FoodName == foodname).FirstOrDefault();
            if (item != null)
            {
                if (food.CartCount <= 0)
                {
                    food.IsAddtoCartClicked = false;
                    food.CartCount = 0;
                }
                HelperClass.TotalPrice = HelperClass.TotalPrice - (1 * food.FoodPrice);
                item.CartCount = food.CartCount;
                item.IsAddtoCartClicked = food.IsAddtoCartClicked;
                _appDbContext.SaveChanges();
            }
            for (int i = 0; i < ViewCartList.Count; i++)
            {
                var zeroCountFood = _appDbContext.ViewCarts.Where(x => x.CartCount == 0).FirstOrDefault();
                if (zeroCountFood != null)
                {
                    _appDbContext.ViewCarts.Remove(zeroCountFood);
                }
            }
        }
        if (HelperClass.OverAllQuantity <= 0)
        {
            HelperClass.CanIShowViewCart = false;
        }
        UpdateSessionCart();
        return RedirectToAction("BestSeller");
    }
    public IActionResult ViewCart()
    {
        ViewCartList = _appDbContext.ViewCarts.ToList();
        return View(ViewCartList);
    }
    public IActionResult ViewCart_Plus(string foodname)
    {
        ViewCartList = _appDbContext.ViewCarts.ToList();
        var food = _appDbContext.ViewCarts.Where(x => x.FoodName == foodname).FirstOrDefault();
        if (food != null)
        {
            if (food.CartCount < food.Quantity)
            {
                food.CartCount++;
                HelperClass.OverAllQuantity = (HelperClass.OverAllQuantity ?? 0) + 1;
                HelperClass.TotalPrice = HelperClass.TotalPrice + (1 * food.FoodPrice);
            }
            _appDbContext.SaveChanges();
        }
        return View("ViewCart", ViewCartList);
    }
    public IActionResult ViewCart_Minus(string foodname)
    {
        /*
         something is wrong in here. 
         
         */
        ViewCartList = _appDbContext.ViewCarts.ToList();
        var food = _appDbContext.ViewCarts.Where(x => x.FoodName == foodname).FirstOrDefault();
        if (food != null)
        {
            food.CartCount--;
            if (food.CartCount <= 0)
            {
                food.CartCount = 0;
            }
            HelperClass.OverAllQuantity--;
            if (HelperClass.OverAllQuantity <= 0)
            {
                HelperClass.OverAllQuantity = 0;
            }

            HelperClass.TotalPrice = HelperClass.TotalPrice - (1 * food.FoodPrice);
            _appDbContext.SaveChanges();
        }
        return View("ViewCart", ViewCartList);
    }

    /*TODO: Currently Working*/
    public IActionResult AddAddress()
    {
        TempData["name"] = "Anithkesava";
        return View();
    }

    public IActionResult Checkout()
    {

        string username = HttpContext.Session.GetString("username") ?? "usernotfound";
        UserDetails userdetails = _appDbContext.UserDetails.Where(x => x.Username == username).FirstOrDefault() ?? new UserDetails();

        UserDetails demouserdetails = new UserDetails
        {
            Username = "Anith Kesava",
            AddressLine1 = "G1-groundfloor Demo Street",
            AddressLine2 = "Demo Nagar",
            NearBy = "Nearby Demo School",
            State = "Demo State",
            City = "Demo City",
            Pincode = 629001
        };
        UserAddress = _appDbContext.UserAddress.ToList();
        if (UserAddress.Count > 0)
        {
            HelperClass.IsAdditionalAddressExists = true;
        }
        else
        {
            HelperClass.IsAdditionalAddressExists = false;
        }
        return View(demouserdetails);
    }
    public IActionResult SaveAddress(UserAddress useraddress)
    {
        //TODO: need to add some more logic in here regarding address saving

        string username = HttpContext.Session.GetString("username") ?? "usernotfound";

        var user = _appDbContext.UserDetails.Where(x => x.Username == username).FirstOrDefault();
        if (user != null)
        {
            useraddress.UserID = user.UserID;
        }
        _appDbContext.UserAddress.Add(useraddress);
        _appDbContext.SaveChanges();
        HelperClass.IsAddressSaved = true;
        return View("AddAddress");
    }
    public static List<UserAddress> UserAddress = new List<UserAddress>();
    public IActionResult AddressAddedSuccessfully()
    {

        HelperClass.IsAddressSaved = false;
        UserDetails demouserdetails = new UserDetails
        {
            Username = "Anith Kesava",
            AddressLine1 = "G1-groundfloor Demo Street",
            AddressLine2 = "Demo Nagar",
            NearBy = "Nearby Demo School",
            State = "Demo State",
            City = "Demo City",
            Pincode = 629001
        };       
        return RedirectToAction("Checkout");
    }

    /* to here */

    public IActionResult Payment(string paymentmode)
    {
        if (paymentmode == "creditdebit")
        {
            HelperClass.IsPaymentCreditCard = true;
            HelperClass.IsPaymentUpiId = false;
            HelperClass.IsPaymentUpiApp = false;
            HelperClass.IsPaymentCod = false;

        }
        else if (paymentmode == "upiapp")
        {
            HelperClass.IsPaymentCreditCard = false;
            HelperClass.IsPaymentUpiId = false;
            HelperClass.IsPaymentUpiApp = true;
            HelperClass.IsPaymentCod = false;

        }
        else if (paymentmode == "upiid")
        {
            HelperClass.IsPaymentCreditCard = false;
            HelperClass.IsPaymentUpiId = true;
            HelperClass.IsPaymentUpiApp = false;
            HelperClass.IsPaymentCod = false;

        }
        else if (paymentmode == "cod")
        {
            HelperClass.IsPaymentCreditCard = false;
            HelperClass.IsPaymentUpiId = false;
            HelperClass.IsPaymentUpiApp = false;
            HelperClass.IsPaymentCod = true;

        }
        else
        {
            HelperClass.IsPaymentCreditCard = false;
            HelperClass.IsPaymentUpiId = false;
            HelperClass.IsPaymentUpiApp = false;
            HelperClass.IsPaymentCod = false;
        }
        UserDetails demouserdetails = new UserDetails
        {
            Username = "Anith Kesava",
            AddressLine1 = "G1-groundfloor Demo Street",
            AddressLine2 = "Demo Nagar",
            NearBy = "Nearby Demo School",
            State = "Demo State",
            City = "Demo City",
            Pincode = 629001
        };
        return View("Checkout", demouserdetails);
    }

    public IActionResult PlaceOrder()
    {

        HelperClass.CanIShowViewCart = false;
        HelperClass.OverAllQuantity = 0;
        HelperClass.TotalPrice = 0;

        var viewCarts = _appDbContext.ViewCarts.ToList();
        _appDbContext.ViewCarts.RemoveRange(viewCarts);
        _appDbContext.SaveChanges();
        HttpContext.Session.Clear();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
