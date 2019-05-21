using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KanbanBoardWithSignalRAngularJSSol.WebReference;
using KanbanBoardWithSignalRAngularJSSol.Models;

namespace KanbanBoardWithSignalRAngularJSSol.Controllers
{
    public class TasksController : ApiController
    {
        private SforceService binding;
        private KanbanBoardWithSignalRAngularJSSolContext db = new KanbanBoardWithSignalRAngularJSSolContext();

        // GET: api/Tasks
        public IQueryable<Models.Task> GetTasks()
        {
            return db.Tasks;
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(Models.Task))]
        public IHttpActionResult GetTask(int id)
        {
            Models.Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        [ResponseType(typeof(Models.Task))]
        public IHttpActionResult PostNewTask(Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var ctx = new KanbanBoardWithSignalRAngularJSSolContext())
            {
                ctx.Tasks.Add(new Models.Task()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    ColumnId = task.ColumnId,
                    EmployeeId = task.EmployeeId,
                    DueDate = task.DueDate,
                    selectedHours = task.selectedHours,
                    EmployeeName = task.EmployeeName,
                    PmAssignedBy = task.PmAssignedBy,
                    PercentageComplete = task.PercentageComplete,
                    CaseNumber = task.CaseNumber
                });
                ctx.SaveChanges();
            }
            return Ok();
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Models.Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Models.Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        [ResponseType(typeof(Models.Task))]
        public IHttpActionResult Download(int id)
        {
            Models.Task task = db.Tasks.Find(id);
            string[] CaseNumArray = new string[100];
            string[] SubjectArray = new string[100];
            string[] OwnerArray = new string[100];
            string[] CreatorArray = new string[100];
            double[] TotalHoursArray = new double[100];
            string[] DescArray = new string[100];
            DateTime[] CreatedDateArray = new DateTime[100];
            InitializeAsync();

            void InitializeAsync()
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string user = "firauso@gbscorp.com";
                string pass = "Lolyol12*T8bU6LVq7EFRE1WiYSGh1Mgn";

                binding = new SforceService();
                binding.Timeout = 60000;

                WebReference.LoginResult lr;

                lr = binding.login(user, pass);

                string authEndPoint = binding.Url;
                binding.Url = lr.serverUrl;

                binding.SessionHeaderValue = new SessionHeader();
                binding.SessionHeaderValue.sessionId = lr.sessionId;
                
                string soqlQuery = "SELECT Subject, CaseNumber, Owner.Name, CreatedDate, Total_Time_Logged__c, CreatedBy.Name, Description FROM Case where (Status = 'Case Opened' or Status = 'Case Assigned')" +
                    "and (Owner.Name = 'Bradley Soverns' or Owner.Name = 'Edward Spain' or Owner.Name = 'Kyle McQuistion' or Owner.Name = 'Dan Dibble' or Owner.Name = 'Porfirio Esparra' or Owner.Name = 'Firaus Odeh')";

                QueryResult qr = binding.query(soqlQuery);
                sObject[] records = qr.records;

                if (qr.size > 0)
                {
                    Console.WriteLine("");

                    for (int i = 0; i < records.Length; i++)
                    {
                        Case T = (Case)records[i];
                        if (CaseNumArray.Contains(T.CaseNumber))
                        {
                            //Should avoid duplicate cases
                        }
                        else
                        {
                            Case c = (Case)records[i];

                            SubjectArray[i] = c.Subject;
                            CaseNumArray[i] = c.CaseNumber;
                            OwnerArray[i] = c.Owner.Name1;
                            CreatorArray[i] = c.CreatedBy.Name;
                            CreatedDateArray[i] = c.CreatedDate ?? DateTime.Now;
                            TotalHoursArray[i] = c.Total_Time_Logged__c.Value;
                            DescArray[i] = c.Description;
                            
                            //Debug.WriteLine(c.CaseNumber + ": " + c.Subject + ": " + c.Owner.Name1 + ": " + c.CreatedBy.Name + ": " + c.CreatedDate + ": " + c.Total_Time_Logged__c);
                        }
                    }
                    logout();
                }
                for (int i = 0; id < records.Length; id++)
                {
                    decimal hours = (decimal)TotalHoursArray[i];
                    using (var ctx = new KanbanBoardWithSignalRAngularJSSolContext())
                    {
                        ctx.Tasks.Add(new Models.Task()
                        {
                            Id = task.Id,
                            Name = SubjectArray[i],
                            Description = DescArray[i],
                            ColumnId = task.ColumnId,
                            EmployeeId = task.EmployeeId,
                            DueDate = task.DueDate,
                            selectedHours = hours,
                            EmployeeName = OwnerArray[i],
                            PmAssignedBy = CreatorArray[i],
                            PercentageComplete = task.PercentageComplete,
                            CaseNumber = CaseNumArray[i]
                        });
                        ctx.SaveChanges();
                    }
                }
            }
            void logout()
            {
                binding.logout();
            }
            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }
    }
}