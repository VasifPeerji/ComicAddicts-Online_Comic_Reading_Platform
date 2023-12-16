using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
/*using PdfiumViewer;
using ImageMagick;*/
using SharpCompress.Archives;
using SSL.Models;
using SSL.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;


namespace SSL.Controllers
{
    public class ComicsController : Controller
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
        public ComicsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Comics
        public ActionResult Index(HttpPostedFileBase myfile)
        {
            if (myfile == null)
            {
                return RedirectToAction("GetComics", "Comics");
            }
            // extract only the filename
            var fileName = Path.GetFileName(myfile.FileName);
            string outputFolder = @"D:\\.Net\\SSL\\SSL\\Media\\Comics\\";
            string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Media/Comics"), fileName);
            //Path.Combine is user for physical path
            /*            var path = Path.Combine(outputFolder, fileName);*/
            myfile.SaveAs(path);
            Decompressor d = new Decompressor();
            Comics comic = d.Convert(myfile);
            if (comic == null)
            {
                var comics = _context.Comics.GroupBy(c => c.Publisher).ToList();
                var stars = CalculateAverageStarsByComic();
                var viewmodel = new GetComicsViewModel
                {
                    Comics = comics,
                    stars = stars
                };
                ViewBag.WrongFile = "Not A Proper File";
                return View("GetComics",viewmodel);
            }
            Console.Write(myfile);
            _context.Comics.Add(comic);
            _context.SaveChanges();

            
            return RedirectToAction("GetComics", "Comics");
        }
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult ComicAdd()
        {

            return View();
        }

        public ActionResult CustomComicAdd()
        {
            var viewmodel = new ComicFormViewModel
            {
                Comic = new Comics(),
                GenresDropDown = _context.Genres.ToList()
            };
            return View("ComicForm", viewmodel);
        }
       
        public ActionResult GetComics()
        {
            var comics = _context.Comics.GroupBy(c=>c.Publisher).ToList();
            var stars = CalculateAverageStarsByComic();
            var viewmodel = new GetComicsViewModel
            {
                Comics = comics,
                stars = stars
            };
            return View(viewmodel);
        }

        public Dictionary<int?, float> CalculateAverageStarsByComic()
        {
            var averageStarsByComic = _context.Ratings
                .Where(r => r.Stars.HasValue && r.Stars.Value != 0)  
                .GroupBy(r => r.ComicId)
                .ToDictionary(
                    group => group.Key,
                    group => (float)group.Average(r => r.Stars.Value)
                );

            return averageStarsByComic;
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Comics comic, HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Media/Comics"), fileName);
            //Path.Combine is user for physical path
            /*            var path = Path.Combine(outputFolder, fileName);*/
            file.SaveAs(path);

            var fileNameWithoutExension = Path.GetFileNameWithoutExtension(file.FileName);
            string cbzFilePath = Path.Combine(HttpContext.Server.MapPath("~/Media/Comics"), fileName);
            string extractionPath = Path.Combine(HttpContext.Server.MapPath("~/Media/turnjs4/samples/docs"), fileNameWithoutExension + "/");
            string readOnlinePath = fileNameWithoutExension;
            string downloadPath = "~/Media/Comics/" + fileName;
            comic.ReadOnline = readOnlinePath;
            comic.Download = downloadPath;
            if (!Directory.Exists(extractionPath))
            {
                Directory.CreateDirectory(extractionPath);
            }
            using (var archive = ArchiveFactory.Open(cbzFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        string entryFilePath = Path.Combine(extractionPath, entry.Key);
                        entry.WriteToFile(entryFilePath);
                    }
                }
            }

            // After extraction is complete, rename non-.xml files
            string[] extractedFiles = Directory.GetFiles(extractionPath, "*.jpg");
            int counter = 1;

            foreach (string filePath in extractedFiles)
            {
                string newFilePath = Path.Combine(extractionPath, $"{counter}.jpg");

                // Now copy the source file to the destination
                System.IO.File.Copy(filePath, newFilePath, true);

                counter++;
            }
            comic.DateAdded = DateTime.Now;
            _context.Comics.Add(comic);
            _context.SaveChanges();
            return RedirectToAction("GetComics", "Comics");
        }
        [Route("Comics/ComicDetails/{id?}")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("GetComics", "Comics");
            }
            string decodedId = Server.UrlDecode(id); // Decode the received ID
            int comicId;
            if (int.TryParse(decodedId, out comicId))
            {
                var comic = _context.Comics.SingleOrDefault(c => c.Id == comicId);

                if (comic == null)
                {
                    return RedirectToAction("GetComics", "Comics");
                }
                var userId = User.Identity.GetUserId();
                var viewModel = new ComicFormViewModel
                {
                    Comic = comic,
                    GenresDropDown = _context.Genres.ToList(),
                    Rating=_context.Ratings.FirstOrDefault(c=>c.ComicId==comicId && c.UserId==userId)
                };

                return View("ComicDetails", viewModel);
            }
            return RedirectToAction("GetComics", "Comics");
        }

        public ActionResult Update(Comics comic)
        {
            var comicInDb = _context.Comics.SingleOrDefault(c => c.Id == comic.Id);
            comicInDb.Name = comic.Name;
            comicInDb.Pages = comic.Pages;
            comicInDb.Publisher = comic.Publisher;
            comicInDb.Summary = comic.Summary;
            comicInDb.DateAdded = DateTime.Now;
            if (comic.GenreDropDownId == null)
            {
                comic.Genre = comic.Genre;
                string[] delimiters = { ",", "/" };
                string[] genres = comic.Genre.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                string selectedGenre = genres.FirstOrDefault();
                if (selectedGenre == "Action")
                {
                    comic.GenreDropDownId = 1;
                }
                else if (selectedGenre == "Superhero")
                {
                    comic.GenreDropDownId = 2;
                }
                else if (selectedGenre == "Horror")
                {
                    comic.GenreDropDownId = 3;
                }
                else if (selectedGenre == "Adventure")
                {
                    comic.GenreDropDownId = 4;
                }
                else if (selectedGenre == "Fantasy")
                {
                    comic.GenreDropDownId = 5;
                }
                else if (selectedGenre == "Children's" || selectedGenre == "Kids")
                {
                    comic.GenreDropDownId = 6;
                }
                else
                {
                    comic.GenreDropDownId = 7;
                }
            }
            else
            {
                comicInDb.GenreDropDownId = comic.GenreDropDownId;
            }
            _context.SaveChanges();
            if (Request.Form["detailsKey"] != null && Request.Form["detailsKey"] == "detailsKey")
            {
                int id = comic.Id;
                return RedirectToAction("Edit", "Comics", new { id = id });
            }
            else
            {
                return RedirectToAction("GetComics", "Comics");
            }

        }
    
       /* public ActionResult ComicDetails()
        {
            return View();
        }*/

        public ActionResult Avatar()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            var comicInDb = _context.Comics.SingleOrDefault(c => c.Id == id);
            var rating = _context.Ratings.FirstOrDefault(c => c.ComicId == id);
            var folderPath = Server.MapPath("~/Media/turnjs4/samples/docs/" + comicInDb.ReadOnline);
            var filePath = Server.MapPath("~/Media/Comics/" + comicInDb.Download);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            if (rating != null)
            {
                _context.Ratings.Remove(rating);
            }
            _context.Comics.Remove(comicInDb);
            _context.SaveChanges();
             
            return RedirectToAction("GetComics", "Comics");
        }
        [Authorize]
        public ActionResult PricingList(string message=null)
        {
           /* if (message == null)
            {
                return View("PricingList");
            }*/
            ViewBag.Message = message;
            return View();
        }



        /* public ActionResult Read()
         {
             return View();
         }*/


    }
}