using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KanbanBoardWithSignalRAngularJSSol.Models
{
    public class DetailViewModel
    {
        public Employees em { get; set; }
        public Task ta { get; set; }
        public List<Employees> Employees { get; set; }
    }
}