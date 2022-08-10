using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; // need this for password hasher
using LoginAndRegistration.Models;

namespace LoginAndRegistration.Controllers;

public class UserController : Controller
{
    private int? userid
    {
        get{return HttpContext.Session.GetInt32("UserId");}
    }

    private bool isLoggedIn
    {
        get
        {
            return userid != null;
        }
    }
    
    // the following context things are needed to inject the context service into the controller
    private MyContext _context;

    public UserController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Registration()
    {
        return View("Registration");
    }

    [HttpPost("create")]
    public IActionResult CreateUser(User newUser)
    {
        if(ModelState.IsValid)
        {
            if(_context.Users.Any(user => user.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "This email is already in use.");
            }
        }
        if(!ModelState.IsValid)
        {
            return Registration();
        }

        PasswordHasher<User> hashedPass = new PasswordHasher<User>();
        newUser.Password = hashedPass.HashPassword(newUser, newUser.Password);
        _context.Users.Add(newUser); // have to specify which table you are adding to.
        _context.SaveChanges();

        // The following line is placed after "SaveChanges()" because it allows us to then access the Id from the database. 
        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        return RedirectToAction("Success");
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpPost("loginuser")]
    public IActionResult LoginUser(Login loginUser)
    {
        if(ModelState.IsValid == false)
        {
            return Login();
        }

        User? existingUser = _context.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if(existingUser == null)
        {
            ModelState.AddModelError("LoginEmail", "The email/password entered is invalid.");
            return Login();
        }
        PasswordHasher<Login> hasher = new PasswordHasher<Login>();
        PasswordVerificationResult checkPassword = hasher.VerifyHashedPassword(loginUser, existingUser.Password, loginUser.LoginPassword);
        if(checkPassword == 0)
        {
            ModelState.AddModelError("LoginPassword", "The email/password entered is invalid.");
            return Login();
        }

        // if it reaches this point, it means that there have been no errors, so store in session and redirect to page that requires login.
        HttpContext.Session.SetInt32("UserId", existingUser.UserId);
        return RedirectToAction("Success");
    }

    [HttpGet("success")]
    public IActionResult Success()
    {
        // checking to see if the userid is in session, so the user has to be logged in to view this page. This information comes from private at the top.
        if(!isLoggedIn)
        {
            return RedirectToAction("Login");
        }
        return View("Success");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}