using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoncharovTestTask.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;

namespace GoncharovTestTask.Controllers
{
    public class LanguageController : Controller
    {
        Organisation_Entities db_language = new Organisation_Entities();
        // Показать языки
        public ActionResult language()
        {
            var query = db_language.Language.OrderBy(s => s.Id);
            return View(query.ToList());
        }

        // Удалить язык
        public ActionResult del(int Id) 
        {
            Language oneLanguage = db_language.Language.Find(Id);
            db_language.Language.Remove(oneLanguage);           
            db_language.SaveChanges();
            return RedirectToAction("language");
        }

        // Добавить язык
        public ActionResult add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add(Language oneLanguage)
        {
            if (ModelState.IsValid)
            {
                var query = db_language.Language.OrderBy(s => s.Id).ToList();
                oneLanguage.Id = query[query.Count - 1].Id + 1;
                db_language.Language.Add(oneLanguage);
                db_language.SaveChanges();
                return RedirectToAction("language");
            }
            else
                return RedirectToAction("add");
        }


    }
}