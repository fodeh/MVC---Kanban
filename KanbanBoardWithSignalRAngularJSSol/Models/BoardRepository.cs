using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KanbanBoardWithSignalRAngularJSSol.Controllers;

namespace KanbanBoardWithSignalRAngularJSSol.Models
{
    public class BoardRepository
    {
        private KanbanBoardWithSignalRAngularJSSolContext db = new KanbanBoardWithSignalRAngularJSSolContext();
        public List<Column> GetColumns()
        {
            var tasks = new List<Task>(db.Tasks.OrderBy(r=> r.StartDate).Where(r => r.ColumnId.Equals(1)).ToList());
            var tasks2 = new List<Task>(db.Tasks.OrderBy(r => r.StartDate).Where(r => r.ColumnId.Equals(2)).ToList());
            var tasks3 = new List<Task>(db.Tasks.OrderBy(r => r.StartDate).Where(r => r.ColumnId.Equals(3)).ToList());
            var tasks4 = new List<Task>(db.Tasks.OrderBy(r => r.StartDate).Where(r => r.ColumnId.Equals(4)).ToList());
            var tasks5 = new List<Task>(db.Tasks.OrderBy(r => r.StartDate).Where(r => r.ColumnId.Equals(5)).ToList());
            if (HttpContext.Current.Cache["columns"] == null)
            {
                var columns = new List<Column>();
                columns.Add(new Column { Description = "needs assigned column", Id = 1, Name = "Needs Assigned", Tasks = tasks });
                columns.Add(new Column { Description = "Work Stalled column", Id = 2, Name = "On Hold", Tasks = tasks2 });
                columns.Add(new Column { Description = "in progress column", Id = 3, Name = "In Progress", Tasks = tasks3 });
                columns.Add(new Column { Description = "test column", Id = 4, Name = "Testing", Tasks = tasks4 });
                columns.Add(new Column { Description = "done column", Id = 5, Name = "Completed in past 30 days", Tasks = tasks5 });
                HttpContext.Current.Cache["columns"] = columns;
            }
            else
            {
                var columns = new List<Column>();
                columns.Add(new Column { Description = "to do column", Id = 1, Name = "Needs Assigned", Tasks = tasks });
                columns.Add(new Column { Description = "Work Stalled column", Id = 2, Name = "On Hold", Tasks = tasks2 });
                columns.Add(new Column { Description = "in progress column", Id = 3, Name = "In Progress", Tasks = tasks3 });
                columns.Add(new Column { Description = "test column", Id = 4, Name = "Testing", Tasks = tasks4 });
                columns.Add(new Column { Description = "done column", Id = 5, Name = "Completed in past 30 days", Tasks = tasks5 });
                HttpContext.Current.Cache["columns"] = columns;
            }
            
            return (List<Column>)HttpContext.Current.Cache["columns"];
        }

        public Column GetColumn(int colId)
        {
            return (from c in this.GetColumns()
                    where c.Id == colId
                    select c).FirstOrDefault();
        }

        public Task GetTask(int taskId)
        {
            var columns = this.GetColumns();            
            foreach (var c in columns)
            {                
                foreach (var task in c.Tasks)
                {
                    if (task.Id == taskId)
                        return task;
                }
            }

            return null;
        }

        //public List<Task> GetTasks()
        //{
        //    return db.Tasks.ToList();
        //}
        

        public void MoveTask(int taskId, int targetColId)
        {
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);
            
            // Add task to the target column
            var task = this.GetTask(taskId);
            var sourceColId = task.ColumnId;
            task.ColumnId = targetColId;
            targetColumn.Tasks.Add(task);
            
            // Remove task from source column
            var sourceCol = this.GetColumn(sourceColId);
            sourceCol.Tasks.RemoveAll(t => t.Id == taskId);

            // Update column collection
            columns.RemoveAll(c => c.Id == sourceColId || c.Id == targetColId);
            columns.Add(targetColumn);
            columns.Add(sourceCol);

            //Update task
           
            Task taskupdate = db.Tasks.Where(r => r.Id.Equals(taskId)).FirstOrDefault();
            if (taskupdate.ColumnId == 5)
            {
                taskupdate.ColumnId = task.ColumnId;
                taskupdate.CompletionDate = DateTime.Now;
                taskupdate.PercentageComplete = 100;
                db.SaveChanges();
            }
            else { 
            taskupdate.ColumnId = task.ColumnId;
            taskupdate.CompletionDate = null;
            db.SaveChanges();
            }
            

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        private void UpdateColumns(List<Column> columns)
        {
            HttpContext.Current.Cache["columns"] = columns;
        }


    }
}