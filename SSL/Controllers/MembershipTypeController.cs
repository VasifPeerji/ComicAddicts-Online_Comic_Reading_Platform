using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace SSL.Controllers
{
    public class MembershipTypeController : Controller
    {
    /*    private UserManager<ApplicationUser> UserManager;*/
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        ApplicationDbContext _context;
        public MembershipTypeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: MembershipType
        public ActionResult Index()
        {
            return View();
        }

        // Action to assign the Silver role
        public ActionResult AssignSilverRole()
        {
            var userId = User.Identity.GetUserId();
            if (User.IsInRole("Free"))
            {
                UserManager.RemoveFromRole(userId, "Free");
            }
            else if (User.IsInRole("Silver"))
            {
                UserManager.RemoveFromRole(userId, "Silver");
            }
            
            UserManager.AddToRole(userId, "Silver");

            var currentDownloads = _context.FreeTrials.FirstOrDefault(c => c.UserId == userId);
            currentDownloads.Downloads = 10;
            currentDownloads.Name = "Silver";
            currentDownloads.Trials = 99999;

            _context.SaveChanges();

            // Get the user details and create a new ClaimsIdentity for the user
            var user = UserManager.FindById(userId);
            var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Sign the user out
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Manually sign the user in with the new identity
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToAction("GetComics","Comics"); // Redirect to the desired page
        }

        // Action to assign the Gold role
        public ActionResult AssignGoldRole()
        {
            var userId = User.Identity.GetUserId();
            if (User.IsInRole("Free"))
            {
                UserManager.RemoveFromRole(userId,"Free");
            }
            else if (User.IsInRole("Silver"))
            {
                UserManager.RemoveFromRole(userId,"Silver");
            }
            else if (User.IsInRole("Gold"))
            {
                UserManager.RemoveFromRole(userId,"Gold");
            }
            UserManager.AddToRole(userId, "Gold");


            var currentDownloads = _context.FreeTrials.FirstOrDefault(c => c.UserId == userId);
            currentDownloads.Downloads = 99999;
            currentDownloads.Name = "Gold";
            currentDownloads.Trials = 99999;

            _context.SaveChanges();

            // Get the user details and create a new ClaimsIdentity for the user
            var user = UserManager.FindById(userId);
            var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Sign the user out
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Manually sign the user in with the new identity
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);


            return RedirectToAction("GetComics","Comics"); // Redirect to the desired page
        }

        // Action to assign the Platinum role
        public ActionResult AssignPlatinumRole(string userId)
        {
            UserManager.AddToRole(User.Identity.GetUserId(), "Platinum");
            return RedirectToAction("Index"); // Redirect to the desired page
        }

    }
}