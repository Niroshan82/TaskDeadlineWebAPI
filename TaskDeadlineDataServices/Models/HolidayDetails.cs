using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDeadlineDataServices.Models
{
    public class HolidayDetails
    {
        public int holidayID { get; set; }
        [Required]
        public string? holidayName { get; set; }
        [Required]
        public DateTime holidayDate { get; set; }
    }
}
