using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using KanbanBoardWithSignalRAngularJSSol.Models;
using KanbanBoardWithSignalRAngularJSSol.Controllers;

namespace KanbanBoardWithSignalRAngularJSSol.Controllers
{
    public class BoardController : Controller
    {

        private KanbanBoardWithSignalRAngularJSSolContext db = new KanbanBoardWithSignalRAngularJSSolContext();

        [HttpGet]
        //[HttpPost, ValidateInput(false)]
        public ActionResult Index()
        {
            //Response.AddHeader("Refresh", "30");
            ViewBag.Employees = db.Employees.ToList();
            ViewData["CurrentTime"] = DateTime.Today.ToString("MM/dd/yyyy");
           return View();
        }



        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.Employees = db.Employees.ToList();
            return View();
        }
        
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Tasks/Create
        
        public ActionResult Download()
        {
            ViewBag.Employees = db.Employees.ToList();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Download(Task task)
        {
           if (ModelState.IsValid)
           {
            db.Tasks.Add(task);
            db.SaveChanges();
            return RedirectToAction("Download");
           }
            return View();
        }

        public ActionResult Keep()
        {
            ViewBag.Employees = db.Employees.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Keep(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Download");
            }
            return View();
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
            
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,DueDate,selectedHours,EmployeeName,PmAssignedBy,PercentageComplete,ColumnId,EmployeeId,CaseNumber")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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