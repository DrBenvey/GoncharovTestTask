using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoncharovTestTask.Models;
using System.Data.Entity;

namespace GoncharovTestTask.Controllers
{
    public class DepartmentController : Controller
    {
        Organisation_Entities db_department = new Organisation_Entities();

        //Показать отделы
        public ActionResult department()
        {
            var query = db_department.Department.OrderBy(s => s.Id);
            return View(query.ToList());
        }

        //Удалить отдел
        public ActionResult del(int Id)
        {
            Department oneDepartment = db_department.Department.Find(Id);
            db_department.Department.Remove(oneDepartment);
            db_department.SaveChanges();
            return RedirectToAction("department");
        }

        //Добавить отдел
        public ActionResult add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add(Department oneDepartment)
        {
            if (ModelState.IsValid)
            {
                var query = db_department.Department.OrderBy(s => s.Id).ToList();
                oneDepartment.Id = query[query.Count - 1].Id + 1;
                db_department.Department.Add(oneDepartment);
                db_department.SaveChanges();
                return RedirectToAction("department");
            }
            else
                return RedirectToAction("add");
        }

    }
}