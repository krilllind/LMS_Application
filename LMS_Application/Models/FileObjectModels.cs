using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LMS_Application.Models
{
    public class FileObjectModels
    {
        [Key]
        public string ID { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Filename { get; set; }

        [Required]
        [StringLength(100)]
        public string MIME_Type { get; set; }

        [Required]
        public byte[] Data { get; set; }
        
        [Required, ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        [ForeignKey("CourseModels")]
        public string CourseID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual CourseModels CourseModels { get; set; }

        public FileObjectModels()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }
}