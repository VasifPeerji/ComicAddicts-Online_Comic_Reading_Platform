using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSL.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace SSL.Controllers
{
    public class FreeTrialsController : Controller
    {
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
        public FreeTrialsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: FreeTrials
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReadTrials(string comicUrl)
        {
            var userId = User.Identity.GetUserId();
            var tries = _context.FreeTrials.FirstOrDefault(c => c.UserId == userId);
            if (tries.Trials <= 0)
            {
                return RedirectToAction("PricingList", "Comics", new { message = "Your free trials are completed! Upgrade the plan for endless reading !" });

            }
            tries.Trials -= 1;

            _context.SaveChanges();
            return Redirect("~/Media/turnjs4/samples/docs/index.html?val="+comicUrl);
        }
        public ActionResult DownloadTrials()
        {
            var userId = User.Identity.GetUserId();
            var tries = _context.FreeTrials.FirstOrDefault(c => c.UserId == userId);
            if (tries.Downloads <= 0)
            {
                return RedirectToAction("PricingList", "Comics", new { message = "Your download counts are exhausted! Upgrade the plan for countless downloads !" });

            }
            tries.Downloads -= 1;

            _context.SaveChanges();

            // Get the URL of the referring page
            var urlReferrer = Request.UrlReferrer;
            if (urlReferrer != null)
            {
                return Redirect(urlReferrer.ToString());
            }
            else
            {
                // Redirect to a default page if there is no referrer
                return RedirectToAction("GetComics", "Comics");
            }
        }

      /*  public ActionResult Read(string comic)
        {
            ViewData["comicUrl"] = comic;
            return View();
        }*/

    }
}