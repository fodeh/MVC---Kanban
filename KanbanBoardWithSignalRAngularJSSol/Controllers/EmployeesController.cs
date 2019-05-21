using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KanbanBoardWithSignalRAngularJSSol.Models;

namespace KanbanBoardWithSignalRAngularJSSol.Controllers
{
    public class EmployeesController : Controller
    {
        private KanbanBoardWithSignalRAngularJSSolContext db = new KanbanBoardWithSignalRAngularJSSolContext();
        // GET: Employees
        public ActionResult Index()
        {
            IEnumerable<EmployeeModel> model = null;
            model = (from e in db.Employees
                     select new EmployeeModel
                     {
                         Id = e.Id,
                         EmployeeName = e.EmployeeName,
                         EmployeeImage = e.EmployeeImage
                     });
            return View(model);
        }

        public ActionResult Employee()
        {
            IEnumerable<EmployeeViewModel> model = null;
            model = (from e in db.Employees
                     join t in db.Tasks on e.EmployeeName equals t.EmployeeName
                     where (t.ColumnId < 4 && t.StartDate <= DateTime.Now)
                     group t by new { t.EmployeeName, t.EmployeeImage } into row
                     select new EmployeeViewModel
                     {
                         EmployeeName = row.Key.EmployeeName,
                         TotalHours = row.Sum(x => x.selectedHours),
                         EmployeeImage = row.Key.EmployeeImage
                     });
            //ViewBag.Total = model.Sum(x => x.TotalHours);
            return View(model);
        }

        public ActionResult Completed_Hours()
        {
            IEnumerable<EmployeeViewCompletedModel> model = null;
            model = (from e in db.Employees
                     join t in db.Tasks on e.EmployeeName equals t.EmployeeName
                     where (t.ColumnId == 4)
                     group t by new { t.EmployeeName, t.EmployeeImage } into row
                     select new EmployeeViewCompletedModel
                     {
                         EmployeeName = row.Key.EmployeeName,
                         TotalHoursCompleted = row.Sum(x => x.selectedHours),
                         EmployeeImage = row.Key.EmployeeImage
                     });
            //ViewBag.Total = model.Sum(x => x.TotalHours);
            return View(model);
        }


        public ActionResult AddImage()
        {
            Employees b1 = new Employees();
            return View(b1);
        }
        [HttpPost]
        public ActionResult AddImage(Employees model, HttpPostedFileBase image1)
        {
            if (image1 != null)
            {
                model.EmployeeImage = new byte[image1.ContentLength];
                image1.InputStream.Read(model.EmployeeImage, 0, image1.ContentLength);
            }
            db.Employees.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }



        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeName,EmployeeImage")] Employees employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employees employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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