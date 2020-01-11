using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Popis.Models;
using Popis1.DAL;

namespace Projektni_Zadatak.Controllers
{
    
    public class FakultetController : Controller
    {

        private PopisDbContext _dbContext;

        public FakultetController(PopisDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index(string query = null)
        {

            var lista = this._dbContext.Fakulteti.ToList();

            return View(lista);
        }

      
        [HttpPost]
        public ActionResult Index(string queryName, string querySurname)
        {
            var fakultetiQuery = this._dbContext.Fakulteti.Include(c =>c.ID).AsQueryable();


            if (!string.IsNullOrWhiteSpace(queryName))
                fakultetiQuery = fakultetiQuery.Where(p => p.NAZIV.Contains(queryName));



            var model = fakultetiQuery.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(Fakultet filter)
        {
            var fakultetiQuery = this._dbContext.Fakulteti.Include(c => c.ID).AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter.NAZIV))
                fakultetiQuery = fakultetiQuery.Where(p => p.NAZIV.Contains(filter.NAZIV.ToLower()));

            var model = fakultetiQuery.ToList();
            return View("Index", model);
        }


        public IActionResult Details(int? id = null)
        {

            var fakultet = this._dbContext.Fakulteti.Find(id);


            return View(fakultet);
        }

  
        public string VratiNaziv(string nazivFaks,int? id = null)
        {

            var fakultet = this._dbContext.Fakulteti.Find(id);
            string naziv = fakultet.NAZIV;
            return naziv;
        }
        public IActionResult Create()
        {
           // this.FillDropdownValues();
            return View();
        }
      
 

        [HttpPost]
        public IActionResult Create(Fakultet model)
        {
            if (ModelState.IsValid)
            {

                this._dbContext.Fakulteti.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

          //  this.FillDropdownValues();
            return View(model);
        }

        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
        //    this.FillDropdownValues();
            return View(this._dbContext.Fakulteti.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(int id, Fakultet faks)
        {
            try
            {

                this._dbContext.Entry(faks).State = EntityState.Modified;
                this._dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        private void FillDropdownValues()
        {
            var fakulteti = new List<SelectListItem>();


            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            fakulteti.Add(listItem);

            foreach (var pred in _dbContext.Fakulteti)
            {
                fakulteti.Add(new SelectListItem()
                {
                    Value = "" + pred.ID,
                    Text = pred.NAZIV
                });
            }

            ViewBag.OdbrFakulteti = fakulteti;
        }

        public ActionResult Delete(int id)
        {
            return View(this._dbContext.Fakulteti.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {

            Fakultet result = this._dbContext.Fakulteti.Find(id);

            this._dbContext.Fakulteti.Remove(result);
     
            this._dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}