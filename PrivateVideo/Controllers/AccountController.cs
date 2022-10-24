using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrivateVideo.Business.SubscribeService;
using PrivateVideo.Data.Dto;
using PrivateVideo.Data.Entity;
using Stripe;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateVideo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<PrivateUser> userManager;
        private readonly SignInManager<PrivateUser> signInManager;
        private readonly ISubscribeService subscribeService;

        public AccountController(UserManager<PrivateUser> userManager,
            SignInManager<PrivateUser> signInManager, ISubscribeService subscribeService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.subscribeService = subscribeService;
        }


        public IActionResult Register()
        {
            UserRegisterDto model = new UserRegisterDto();
            return View(model);
        }


        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new PrivateUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = false,
                        HasPaid = false
                    };

                    var result = await userManager.CreateAsync(user, "initphase");
                    if (result.Succeeded)
                    {
                        await this.userManager.AddToRoleAsync(user, "User");

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Subscribe(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = 999,
                Description = "Subscription Private Video",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {

                //register the user in the system

                return RedirectToAction("SuccededSubscription");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SuccededSubscription()
        {
            return View();
        }

       /* [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            *//*if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);*//*
        }*/


        /* public async Task<IActionResult> Logout()
         {
            *//* await signInManager.SignOutAsync();
             return RedirectToAction("Login", "Account");*//*
         }*/




    }
}
