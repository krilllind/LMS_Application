using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Application.Models
{
    public class SchoolClassModels
    {
        [Key]
        public string SchoolClassID { get; set; }

        public string Name { get; set; }

        public virtual ScheduleModels Schedule { get; set; }
        
        public virtual ICollection<ApplicationUser> Students { get; set; }

        public SchoolClassModels()
        {
            this.SchoolClassID = Guid.NewGuid().ToString();
        }
    }
}