using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PeopleSearch.Models;

namespace PeopleSearch.Controllers
{
	public class PeopleController : Controller
	{
		private PeopleDBContext db = new PeopleDBContext();
       public void setContext(DbSet<Person> obj)
       {
           db.People = obj;
       }
		[HttpPost]
		public string Index(FormCollection fc, string firstName)
		{
            return "<h3> From [HttpPost]Index: " + firstName + "</h3>";
		}

		// GET: People Info displays
        public ActionResult Index()
		{
			var people = from m in db.People
						 select m;
            
			return View(people);
		}

        [HttpPost]
	    public ActionResult Search(string firstName, string lastName)
	    {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return null;
            }

            var people = (from m in db.People
                         where m.LastName.Equals(lastName.ToLower()) || m.FirstName.Equals(firstName.ToLower())
                         select m).ToList();

            if (!people.Any())
            {
                return null;
            }

            return View(people);
	    }

		// GET: People/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person person = db.People.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			return View(person);
		}

		// GET: People/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: People/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "PersonID,FirstName,LastName,Address,Age,Interests,Picture,AlternateText")] Person person)
		{
			if (ModelState.IsValid)
			{
				db.People.Add(person);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(person);
		}

		// GET: People/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person person = db.People.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			return View(person);
		}

		// POST: People/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "PersonID,FirstName,LastName,Address,Age,Interests,Picture,AlternateText")] Person person)
		{
			if (ModelState.IsValid)
			{
				db.Entry(person).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(person);
		}

		// GET: People/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person person = db.People.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			return View(person);
		}

		// POST: People/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Person person = db.People.Find(id);
			db.People.Remove(person);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
