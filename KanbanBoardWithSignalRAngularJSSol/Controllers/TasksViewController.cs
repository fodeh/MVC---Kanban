﻿using System;
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
    public class TasksViewController : Controller
    {
        private KanbanBoardWithSignalRAngularJSSolContext db = new KanbanBoardWithSignalRAngularJSSolContext();

        // GET: TasksView
        public ActionResult Index()
        {
            return View(db.Tasks.ToList());
        }

        // GET: TasksView/Details/5
        public ActionResult Details(int? id)
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

        // GET: TasksView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TasksView/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ColumnId,DueDate,selectedHours,EmployeeName,PmAssignedBy,PercentageComplete,EmployeeId,EmployeeImage,CurrentDate,CurrentDatePlus3,CompletionDate,StartDate,CaseNumber")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        //-------------------------------------------------------------------------------------------------------------------

        public ActionResult Download()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Download([Bind(Include = "Id,Name,Description,ColumnId,DueDate,selectedHours,EmployeeName,PmAssignedBy,PercentageComplete,EmployeeId,EmployeeImage,CurrentDate,CurrentDatePlus3,CompletionDate,StartDate,CaseNumber")] Task task)
        {
           /// if (ModelState.IsValid)
            //{
            db.Tasks.Add(task);
            db.SaveChanges();
            //return RedirectToAction("Index");
           // }

            return View(task);
        }
        //--------------------------------------------------------------------------------------------------------------------

        // GET: TasksView/Edit/5
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

        // POST: TasksView/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ColumnId,DueDate,selectedHours,EmployeeName,PmAssignedBy,PercentageComplete,EmployeeId,EmployeeImage,CurrentDate,CurrentDatePlus3,CompletionDate,StartDate,CaseNumber")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET: TasksView/Delete/5
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

        // POST: TasksView/Delete/5
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