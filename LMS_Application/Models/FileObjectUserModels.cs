using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Application.Models
{
    public class FileObjectUserModels
    {
        [Key]
        public string FileObjectUserID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string SSN { get; set; }

        [ForeignKey("FileObjectModels")]
        public string FileObjectID { get; set; }

        [ForeignKey("CourseModels")]
        public string CourseID { get; set; }

        public bool Shared { get; set; }

        public DateTime UploadedTime { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual FileObjectModels FileObjectModels { get; set; }

        public virtual CourseModels CourseModels { get; set; }

        public FileObjectUserModels()
        {
            this.FileObjectUserID = Guid.NewGuid().ToString();
        }
    }
}