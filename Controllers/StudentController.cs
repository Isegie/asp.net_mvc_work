using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Popis.Models;
using Popis1.DAL;
using Projektni_Zadatak.Models;

namespace Projektni_Zadatak.Controllers
{

    public class StudentController : Controller
    {


        private PopisDbContext _dbContext;

        public StudentController(PopisDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

      
        public IActionResult Index(string query = null)
        {

            var lista = this._dbContext.Studenti.ToList();
    
            return View(lista);
        }

        [HttpPost]
        public ActionResult Index(string queryName, string querySurname)
        {
            var clientQuery = this._dbContext.Studenti.Include(c => c.ID).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryName))
                clientQuery = clientQuery.Where(p => p.IME.Contains(queryName));

            if (!string.IsNullOrWhiteSpace(querySurname))
                clientQuery = clientQuery.Where(p => p.PREZIME.Contains(querySurname));

            var model = clientQuery.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Pretraga(StudentFilterModel filter)
        {
            var studentQuery = this._dbContext.Studenti.Include(c => c.ID).AsQueryable();

      
            if (!string.IsNullOrWhiteSpace(filter.Ime))
                studentQuery = studentQuery.Where(p => p.IME.Contains(filter.Ime.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Prezime))
                studentQuery = studentQuery.Where(p => p.PREZIME.Contains(filter.Prezime.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Oib))
                studentQuery = studentQuery.Where(p => p.OIB.Contains(filter.Oib.ToLower()));

            var model = studentQuery.ToList();

            return View("Index",model);
        }
        public IActionResult StudentiInfo()
        {

            var lista = this._dbContext.Studenti.ToList();

            return View(lista);
        }

        public IActionResult Details(int? id = null)
        {

            var student = this._dbContext.Studenti.Find(id);
               
      
            return View(student);
        }

        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }
   
 
        [HttpPost]
        public IActionResult Create(Student model)
        {
            if (ModelState.IsValid)
            {
              
                this._dbContext.Studenti.Add(model);
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
            return View(this._dbContext.Studenti.FirstOrDefault(p => p.ID == id));
        }

        [HttpPost, ActionName("Edit")]
        public  IActionResult EditPost(int id,Student student)
        {
            try {
               
                this._dbContext.Entry(student).State = EntityState.Modified;
                this._dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch{
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

            ViewBag.OdbrFakulteti =fakulteti;
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

            this._dbContext.SaveChanges();

           
          
            return RedirectToAction("Index");
        }
        }
    }