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
    public class PredmetController : Controller
    {

        private PopisDbContext _dbContext;

        public PredmetController(PopisDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index(string query = null)
        {

            var lista = this._dbContext.Predmeti.ToList();

            return View(lista);
        }

        [HttpPost]
        public ActionResult Index(string queryName,int id)
        {
            var predmetQuery = this._dbContext.Predmeti.Include(c => c.ID).AsQueryable();

        
            if (!string.IsNullOrWhiteSpace(queryName))
                predmetQuery = predmetQuery.Where(p => p.NAZIV.Contains(queryName));

     

            var model = predmetQuery.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(Predmet filter)
        {
            var clientQuery = this._dbContext.Studenti.Include(c => c.ID).AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter.NAZIV))
                clientQuery = clientQuery.Where(p => p.IME.Contains(filter.NAZIV.ToLower()));


            var model = clientQuery.ToList();
            return View("Index", model);
        }


        public IActionResult Details(int? id = null)
        {

            var predmet = this._dbContext.Predmeti.Find(id);


            return View(predmet);
        }

        public IActionResult Create()
        {
         //   this.FillDropdownValues();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Predmet model)
        {
            if (ModelState.IsValid)
            {

                this._dbContext.Predmeti.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

         //   this.FillDropdownValues();
            return View(model);
        }

        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            //this.FillDropdownValues();
            return View(this._dbContext.Predmeti.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(int id, Predmet predmet)
        {
            try
            {

                this._dbContext.Entry(predmet).State = EntityState.Modified;
                this._dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
     

        public ActionResult Delete(int id)
        {
            return View(this._dbContext.Studenti.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {

            Student result = this._dbContext.Studenti.Find(id);

            this._dbContext.Studenti.Remove(result);
            //Postavljanje stanja na deleted - oznaavanje da je kviz obrisan 
            //  this._dbContext.Entry(result).State = System.Data.Entity.EntityState.Deleted;
            //System.Data.Entity.EntityState.Deleted;
            //Spremanje promjena (commit)
            this._dbContext.SaveChanges();



            //      return RedirectToAction("Delete", new { ID = id, saveChangesError = true });

            return RedirectToAction("Index");
        }
    }
}