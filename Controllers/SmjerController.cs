using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Projektni_Zadatak.Controllers
{
    public class SmjerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("Edit")]
        public IActionResult Edit_Get(int id)
        {

            return View();
        }
        [HttpPost]
        public IActionResult Edit_Post(int id)
        {

            return RedirectToAction("Index");
        }


        public IActionResult Details()
        {

            return View();
        }

        [ActionName("Create")]
        public IActionResult Create()
        {

            return View();
        }
        [ActionName("Delete")]
        public IActionResult Delete()
        {

            return View();
        }

    }
}