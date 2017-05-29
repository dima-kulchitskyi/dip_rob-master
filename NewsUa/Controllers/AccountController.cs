using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NewsUa.Models;
using NewsUa.Models.Repository;
using NewsUa.Models.ViewModel;
using System.Collections.Generic;
using System.Configuration;
using NewsUa.Models.Services;
using System.IO;

namespace NewsUa.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController(
            UserManager<AppUser, int> userManager, 
            SignInManager<AppUser, int> signInManager,
            IUserRepository repo, 
            ITagRepository tagRepo, 
            INotifiactionsRepository notifiRepo)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this.repo = repo;
            this.tagRepo = tagRepo;
            this.notifiRepo = notifiRepo;
            notifiCountCache = new NotificationsCountService(notifiRepo);


        }
        readonly NotificationsCountService notifiCountCache;
        readonly IUserRepository repo;
        readonly ITagRepository tagRepo;
        readonly INotifiactionsRepository notifiRepo;
        readonly SignInManager<AppUser, int> SignInManager;
        readonly UserManager<AppUser, int> UserManager;

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "News");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = new AppUser { UserName = model.Email, Password = model.Password };
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/") return RedirectToAction("Index", "News");
                        return Redirect(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = "", RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            AppUser currentUser = repo.GetById(User.Identity.GetUserId<int>());
            UserViewModel userView = new UserViewModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                ImageName = currentUser.Image,
                UserTags = currentUser.Tags,
            };
            return View(userView);
        }

        [HttpGet]
        public ActionResult Notifications()
        {
            var lst = notifiRepo.GetList(User.Identity.GetUserId<int>());
            return View(lst);
        }

        [HttpGet]
        public ActionResult EditTags()
        {
            AppUser currentUser = repo.GetById(User.Identity.GetUserId<int>());
            EditTagsModel editTags = new EditTagsModel
            {
                UserTags = currentUser.Tags,
                AllTags = tagRepo.GetAllTags()
            };
            return View(editTags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTags(string[] tags)
        {
            AppUser currentUser = repo.GetById(User.Identity.GetUserId<int>());
            currentUser.Tags.Clear();
            if (tags == null)
            {
                repo.Save(currentUser);
            }
            else
            {
                IEnumerable<Tag> tagsList = TagsHelper.CreateTagList(tags, tagRepo);
                TagsHelper.SetTagForModel(currentUser, tagsList);
                repo.Save(currentUser);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> EditPassword()
        {
            AppUser currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (currentUser != null)
            {
                EditPasswordModel editPassword = new EditPasswordModel();
                return View(editPassword);
            }
            return RedirectToAction("Index", "News");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPassword(EditPasswordModel editModel)
        {
            AppUser currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }
            IdentityResult validPass = null;
            if (!string.IsNullOrEmpty(editModel.NewPassword))
            {
                if (await UserManager.CheckPasswordAsync(currentUser, editModel.OldPassword))
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(editModel.NewPassword);
                    if (!validPass.Succeeded)
                    {
                        AddErrors(validPass);
                    }
                    else
                    {
                        currentUser.Password = UserManager.PasswordHasher.HashPassword(editModel.NewPassword);
                        IdentityResult result = await UserManager.UpdateAsync(currentUser);
                        if (!result.Succeeded)
                        {
                            AddErrors(result);
                        }
                        else
                        {
                            return RedirectToAction("Index", "News");
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Введений пароль не збігається з поточним");
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> EditEmail()
        {
            AppUser currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (currentUser != null)
            {
                EditEmailModel editModel = new EditEmailModel { Email = currentUser.UserName };
                return View(editModel);
            }
            return RedirectToAction("Index", "News");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEmail(EditEmailModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }
            AppUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            user.UserName = editModel.Email;
            IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
            if (!validEmail.Succeeded)
            {
                AddErrors(validEmail);
            }
            else
            {
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    AddErrors(result);
                }
                return RedirectToAction("Index", "News");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "News");
            return View();
        }

    
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email };
                if (model.Image != null)
                {
                    user.Image = Path.GetFileName(model.Image.FileName);
                }
                else
                {
                    user.Image = "Default";
                }
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    if (user.Image != "Default")
                    {
                        FileHelper fileHelper = new FileHelper();
                        fileHelper.SaveFIle(Server.MapPath(ConfigurationManager.AppSettings["UserImagesFolder"]), model.Image, user.Id);
                    }
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("Index", "News");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

     
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

     


       
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "News");
        }

       
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}