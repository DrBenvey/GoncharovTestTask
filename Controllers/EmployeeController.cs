using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoncharovTestTask.Models;
using System.Data.Entity;

namespace GoncharovTestTask.Controllers
{
    public class EmployeeController : Controller
    {
        Organisation_Entities db = new Organisation_Entities();       

        //Показать сотрудников
        public ActionResult employee()
        {
            var query = db.Employee.OrderBy(s => s.SecondName);
            return View(query.ToList());
        }

        //Удалить сотрудника
        public ActionResult del(int Id)
        {
            Employee oneEmployee = db.Employee.Find(Id);
            db.Employee.Remove(oneEmployee);
            db.SaveChanges();
            return RedirectToAction("employee");
        }

        //Добавить сотрудника
        public ActionResult add(string Department, string Language)
        {
            //Формируем список наименований отделов
            var NameDepartment = new List<string>();
            var NameQryDepartment = from d in db.Department
                          orderby d.Id
                          select d.Departmentname;
            NameDepartment.AddRange(NameQryDepartment.Distinct());
            ViewBag.Department = new SelectList(NameDepartment);

            //Формируем список наименований языков
            var NameLanguage = new List<string>();
            var NameQryLanguage = from d in db.Language
                                  orderby d.Id
                                    select d.Languagename;
            NameLanguage.AddRange(NameQryLanguage.Distinct());
            ViewBag.Language = new SelectList(NameLanguage);

            return View();
            
        }

        [HttpPost]
        public ActionResult add(Employee oneEmployee, string Department, string Language)
        {
            if (ModelState.IsValid)
            {
                var query = db.Employee.OrderBy(s => s.Id).ToList();
                oneEmployee.Id = query[query.Count - 1].Id + 1;
                oneEmployee.Department = Department;
                oneEmployee.Language = Language;
                db.Employee.Add(oneEmployee);
                db.SaveChanges();
                return RedirectToAction("employee");
            }
            else
                return RedirectToAction("add");
        }
        // Редактирование пользователя
        public ActionResult edit(int Id)
        {
            Employee oneEmployee = db.Employee.Find(Id);

            //Формируем список наименований отделов
            var NameDepartment = new List<string>();
            NameDepartment.Add(oneEmployee.Department);
            var NameQryDepartment = from d in db.Department
                                    orderby d.Id
                                    where d.Departmentname!= oneEmployee.Department
                                    select d.Departmentname;

            NameDepartment.AddRange(NameQryDepartment.Distinct());
            ViewBag.Department = new SelectList(NameDepartment);

            //Формируем список наименований языков         
            var NameLanguage = new List<string>();
            NameLanguage.Add(oneEmployee.Language);
            var NameQryLanguage = from d in db.Language
                                  orderby d.Id
                                  where d.Languagename != oneEmployee.Language
                                  select d.Languagename;
            NameLanguage.AddRange(NameQryLanguage.Distinct());
            ViewBag.Language = new SelectList(NameLanguage);

           
            return View(oneEmployee);
        }
        [HttpPost]
        public ActionResult edit(Employee oneEmployee, string Department, string Language)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oneEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("employee");
            }
            return RedirectToAction("edit");
        }

        // Автозаполнение имени сотрудника        
        [HttpPost]
        public JsonResult GetFirstName(string Prefix)
        {
            var Names = (from c in db.Name
                             where c.FirstName.StartsWith(Prefix)
                             select new { c.FirstName });
            
            return Json(Names, JsonRequestBehavior.AllowGet);
        }
        


    }
}