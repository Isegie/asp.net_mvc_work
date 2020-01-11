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
    public class ProfesorController : Controller
    {

        private PopisDbContext _dbContext;

        public ProfesorController(PopisDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index(string query = null)
        {
      
            var prof = this._dbContext.Profesori.ToList();
       
            return View(prof);
        }

        [HttpPost]
        public ActionResult Index(string queryName, string querySurname)
        {
            var clientQuery = this._dbContext.Profesori.Include(p => p.ID).AsQueryable();

        
            if (!string.IsNullOrWhiteSpace(queryName))
                clientQuery = clientQuery.Where(p => p.IME.Contains(queryName));

            if (!string.IsNullOrWhiteSpace(querySurname))
                clientQuery = clientQuery.Where(p => p.PREZIME.Contains(querySurname));

            var model = clientQuery.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(Profesor filter)
        {
            var profQuery = this._dbContext.Profesori.Include(c => c.ID).AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter.IME))
                profQuery = profQuery.Where(p => p.IME.Contains(filter.IME.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.PREZIME))
                profQuery = profQuery.Where(p => p.PREZIME.Contains(filter.PREZIME.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.OIB))
                profQuery = profQuery.Where(p => p.OIB.Contains(filter.OIB.ToLower()));



            var model = profQuery.ToList();
            return View("Index", model);
        }


        public IActionResult Details(int? id = null)
        {

            var profesor = this._dbContext.Profesori.Find(id);

            this.FillDropdownValues();
            return View(profesor);
        }

        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }
        /*
        public IActionResult Delete()
        {

            return View();
        }

    */
        [HttpPost]
        public IActionResult Create(Profesor model)
        {
            if (ModelState.IsValid)
            {

                this._dbContext.Profesori.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();
            return View(model);
        }

        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            this.FillDropdownValues();
            return View(this._dbContext.Profesori.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            var student = this._dbContext.Profesori.FirstOrDefault(p => p.ID == id);
            var ok = await this.TryUpdateModelAsync(student);

            if (ok)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

             this.FillDropdownValues();
            return View(student);
        }


        private void FillDropdownValues()
        {
            var predmeti = new List<SelectListItem>();

    
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            predmeti.Add(listItem);

            foreach (var pred in _dbContext.Predmeti)
            {
               predmeti.Add(new SelectListItem()
                {
                    Value = "" + pred.ID,
                    Text = pred.NAZIV
                });
            }

            ViewBag.OdabirPredmeta = predmeti;
        }
        public ActionResult Delete(int id)
        {
            return View(this._dbContext.Profesori.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {

            Profesor result = this._dbContext.Profesori.Find(id);
            try
            {
                this._dbContext.Profesori.Remove(result);

                this._dbContext.SaveChanges();

                return RedirectToAction("Delete", new { ID = id, saveChangesError = true });
            }
            catch {
                return View();
            }         

        }
    }
}