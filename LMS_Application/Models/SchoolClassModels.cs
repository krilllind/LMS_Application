﻿using System;
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

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }
        
        public virtual List<ApplicationUser> Students { get; set; }

        public virtual ICollection<CourseModels> Courses { get; set; }

        public SchoolClassModels()
        {
            this.SchoolClassID = Guid.NewGuid().ToString();
        }
    }
}