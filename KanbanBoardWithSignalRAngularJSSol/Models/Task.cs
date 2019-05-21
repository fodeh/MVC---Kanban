using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace KanbanBoardWithSignalRAngularJSSol.Models
{
    public class Task
    {
        public int Id { get; set; }
        [Display(Name = "Task Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int ColumnId { get; set; }
        
        //[Required(ErrorMessage = "Enter the Due Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }
        [Display(Name = "Work Hours")]
        public decimal? selectedHours { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Assigned By")]
        public string PmAssignedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n0}")]
        [Range(0, 100)]
        [Display(Name = "Percentage Complete")]
        public decimal? PercentageComplete { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Employee Image")]
        public byte[] EmployeeImage { get; set; }
        public DateTime? CurrentDate { get; set; }
        public DateTime? CurrentDatePlus3 { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Completion Date")]
        public DateTime? CompletionDate { get; set; }
        [Required(ErrorMessage = "Enter the Start Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }    
        [Required(ErrorMessage = "Enter a Case Number.")]
        [DataType(DataType.Text)]
        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; }
    }
}