using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSL.Models;

namespace SSL.Controllers.api
{
    public class ComicsController : ApiController
    {
        private ApplicationDbContext _context;

        public ComicsController()
        {
            _context = new ApplicationDbContext();
        }


		//GET api/Comics

		public IEnumerable<Comics> GetComics()
		{
			return _context.Comics.ToList();
		}

		//GET api/Comics/1

		public Comics GetComics(int id)
		{
			var comic = _context.Comics.SingleOrDefault(c => c.Id == id);

			if (comic == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);
			return comic;
		}

		//POST /api/Comics
		[HttpPost]
		public Comics CreateComic(Comics comic)
		{
			if (!ModelState.IsValid)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			_context.Comics.Add(comic);
			_context.SaveChanges();

			return comic;
		}

		//PUT /api/Comics/1
		[HttpPut]
		public void UpdateComic(int id, Comics comic)
		{
			if (!ModelState.IsValid)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			var comicInDb = _context.Comics.SingleOrDefault(c => c.Id == id);

			if (comic == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			comicInDb.Name = comic.Name;
			comicInDb.Pages = comic.Pages;
			comicInDb.Publisher = comic.Publisher;
			comicInDb.Summary = comic.Summary;
			comicInDb.DateAdded = DateTime.Now;
			comicInDb.Genre = comic.Genre;
			comicInDb.GenreDropDownId = comic.GenreDropDownId;

			_context.SaveChanges();
		}

		//DELETE /api/Comics/1

		[HttpDelete]
		public void DeleteComic(int id)
		{

			var comicInDb = _context.Comics.SingleOrDefault(c => c.Id == id);

			if (comicInDb == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			_context.Comics.Remove(comicInDb);
			_context.SaveChanges();
		}
	}
}
