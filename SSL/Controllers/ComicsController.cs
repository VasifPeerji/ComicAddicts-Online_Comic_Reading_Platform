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
            Console.Write(myfile);
            _context.Comics.Add(comic);
            _context.SaveChanges();

            /*string pdfPath = $"D:\\.Net\\SSL\\SSL\\App_Data\\comics\\{fileName}";
            Console.Write(pdfPath);
            string outputFolder = "D:\\.Net\\SSL\\SSL\\App_Data\\comics\\";

            using (MagickImageCollection pdfDocument = new MagickImageCollection())
            {
                pdfDocument.Read(pdfPath);

                for (int pageIndex = 1; pageIndex <= pdfDocument.Count; pageIndex++)
                {
                    using (MagickImage image = new MagickImage(pdfDocument[pageIndex]))
                    {
                        // Set the output image format (JPEG)
                        image.Format = MagickFormat.Jpeg;

                        // Set any additional settings for image quality, size, etc.
                        image.Quality = 90; // Adjust as needed

                        // Specify the output file path for the current page
                        string outputPath = $"{outputFolder}/page_{pageIndex}.jpg";
                        string outputPath = $"{outputFolder}//{pageIndex}.jpg";

                        // Save the image
                        image.Write(outputPath);
                    }
                }
                // Verify that the user selected a file
                if (myfile != null && myfile.ContentLength > 0)
                {
                    // extract only the filename
                    var fileName = Path.GetFileName(myfile.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    //Path.Combine Server.MapPath is used for virtual path 
                    var path = Path.Combine(Server.MapPath("~/Media/Comics"), fileName);
                    myfile.SaveAs(path);
                }
                Rename.Renamemyfile("D:\\.Net\\docs\\images2\\public_domain_comic_FKB");
                return View();*/
            /*}*/
            /*  return View(comic);*/
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
                comicInDb.Genre = comic.Genre;
            }
            else
            {
                comicInDb.GenreDropDownId = comic.GenreDropDownId;
            }
            _context.SaveChanges();
            if (Request.Form["detailsKey"] != null && Request.Form["detailsKey"] == "detailsKey")
            {
                // If "detailsKey" is set and is equal to "detailsKey", redirect to ComicDetails
                int id = comic.Id;
                return RedirectToAction("Edit", "Comics", new { id = id });
            }
            else
            {
                // If "detailsKey" is not set or set to something else, redirect to GetComics
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