using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SSL.Models;

namespace SSL.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AdminController : Controller
    {
        ApplicationDbContext _context;

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

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public ActionResult AllComics()
        {
            var users = _context.Comics.ToList();
            return View(users);
        }

        public ActionResult DeleteComic(int id)
        {

            var getRating = _context.Ratings.FirstOrDefault(c => c.ComicId == id);
            var getComic = _context.Comics.FirstOrDefault(c => c.Id == id);

            if (getRating != null)
            {
                _context.Ratings.Remove(getRating);
            }
           
    
            
            _context.Comics.Remove(getComic);

            
            _context.SaveChanges();
            return RedirectToAction("AllComics","Admin");
        }
        
    }
}