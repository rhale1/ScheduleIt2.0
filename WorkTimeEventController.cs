using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScheduleIt2._0.Models;

namespace ScheduleIt2._0.Controllers
{
    public class WorkTimeEventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkTimeEventModel
        public ActionResult Index()
        {
            return View(db.EventModels.ToList());
        }

        // GET: WorkTimeEventModel/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimeEventModel workTimeEventModel = db.WorkTimeEventModels.Find(id);
            if (workTimeEventModel == null)
            {
                return HttpNotFound();
            }
            return View(workTimeEventModel);
        }

        // GET: WorkTimeEventModel/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: WorkTimeEventModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClockIn(WorkTimeEventModel workTimeEventModel, LoginViewModel lvm)
        {
            //gets user from db by email or username
            var user = db.Users.SingleOrDefault(x => x.UserName == lvm.Email || x.Email == lvm.Email);
            //maps user from db to current user
            var ClockInTimeEvent = db.EventModels.FirstOrDefault(x => x.User.Id == user.Id);
            // Checks if user is clocked in by checking if any events exist without a endtime
            var TimeEvent = db.EventModels.FirstOrDefault(x => x.User.Id == user.Id && !x.EndTime.HasValue);

            //if user is already clocked in but has no endtime value
            if (ClockInTimeEvent != null && TimeEvent != null)
            {
                //displays message to user *currently using to keep track of methods
                TempData["Message"] = "Already clocked in";
                return RedirectToAction("Login", "Account");
            }
            //if a user is not already clocked in, create a new worktimeevent and save to db
            //clock in 
            else
            {
                DateTime start = DateTime.Now;
                var message = "clock in: ";
                WorkTimeEventModel clockIn = new WorkTimeEventModel(user, message, start);
                db.EventModels.Add(clockIn);
                db.SaveChanges();
                //displays message to user *currently using to keep track of methods
                TempData["Message"] = "Clock in: " + DateTime.Now.ToString("h:mm tt") + " Have a great shift " + user.Fname;
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClockOut(WorkTimeEventModel workTimeEventModel, LoginViewModel lvm)
        {
            //gets user from db by email or username
            var user = db.Users.SingleOrDefault(x => x.UserName == lvm.Email || x.Email == lvm.Email);
            var TimeEvent = db.EventModels.FirstOrDefault(x => x.User.Id == user.Id && !x.EndTime.HasValue);
            if (TimeEvent != null)
            {  //Update the current open event with an end datetime.
                //Updates message column in db Event
                TimeEvent.Message += "clock out: ";
                DateTime endTime = DateTime.Now;
                TimeEvent.EndTime = endTime;
                db.SaveChanges();
                //displays message to user *currently using to keep track of methods
                TempData["Message"] = "Clock out: " + DateTime.Now.ToString("h:mm tt") + " Have a great day!";
                return RedirectToAction("Login", "Account");
            }
            //displays message to user *currently using to keep track of methods
            else
                TempData["Message"] = "Unable to clock out, please clock in to clock out";
            return RedirectToAction("Login", "Account");
        }

        // GET: WorkTimeEventModel/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimeEventModel workTimeEventModel = db.WorkTimeEventModels.Find(id);
            if (workTimeEventModel == null)
            {
                return HttpNotFound();
            }
            return View(workTimeEventModel);
        }

        // POST: WorkTimeEventModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,StartTime,EndTime,Message,Title,AdminId")] WorkTimeEventModel workTimeEventModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workTimeEventModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workTimeEventModel);
        }

        // GET: WorkTimeEventModel/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimeEventModel workTimeEventModel = db.WorkTimeEventModels.Find(id);
            if (workTimeEventModel == null)
            {
                return HttpNotFound();
            }
            return View(workTimeEventModel);
        }

        // POST: WorkTimeEventModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            WorkTimeEventModel workTimeEventModel = db.WorkTimeEventModels.Find(id);
            db.EventModels.Remove(workTimeEventModel);
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
