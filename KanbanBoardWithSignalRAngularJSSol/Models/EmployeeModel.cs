using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KanbanBoardWithSignalRAngularJSSol.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Employee Image")]
        public byte[] EmployeeImage { get; set; }
    }
}