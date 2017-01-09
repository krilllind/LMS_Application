using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Application.Models
{
    public class ScheduleModels
    {
        [Key, ForeignKey("SchoolClass")]
        public string ScheduleID { get; set; }

        public virtual SchoolClassModels SchoolClass { get; set; }

        public virtual ICollection<CourseModels> Courses { get; set; }

        public ScheduleModels()
        {
            this.ScheduleID = Guid.NewGuid().ToString();
        }
    }
}