using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSL.Controllers
{
    public class RatingsController : Controller
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
        public RatingsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Ratings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(int? stars,int? comicId)
        {
            if (stars.HasValue)
            {
                Ratings review = new Ratings();
                review.Stars = stars.Value;
                review.ComicId = comicId.Value;

                var userId = User.Identity.GetUserId();
                review.UserId = userId;

                var reviewInDb = _context.Ratings.FirstOrDefault(c => c.UserId == userId && c.ComicId == comicId);
                if (reviewInDb == null)
                {
                    _context.Ratings.Add(review);
                }
                else
                {
                    reviewInDb.Stars = stars.Value;
                }
                _context.SaveChanges();
                return RedirectToAction("Edit", "Comics", new { id = comicId});
            }
            return RedirectToAction("Edit", "Comics", new { id = comicId});

        }

        /*public ActionResult TotalStars(int id,string userId)
        {
            var reviewInDb = _context.Ratings.FirstOrDefault(c => c.UserId == userId && c.ComicId == id);
            return View("Edit", "Comics", new { id = id });
        }*/

    }
}