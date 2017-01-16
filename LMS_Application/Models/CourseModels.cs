using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

namespace LMS_Application.Models
{
    public class CourseModels : ICourseModels
    {
        [Key]
        public string CourseID { get; set; }

        public string Subject { get; set; }

        public string CourseLevel { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Day { get; set; }

        public string Classroom { get; set; }

        public string Teacher { get; set; }

        public string Color
        {
            get { return "#" + this.Subject.GetHashCode().ToString("X").Substring(0, 6); }
        }

        public virtual ICollection<ApplicationUser> Teachers { get; set; }

        [ForeignKey("SchoolClass")]
        public string SchoolClassID { get; set; }

        public virtual SchoolClassModels SchoolClass { get; set; }

        public CourseModels()
        {
            this.CourseID = Guid.NewGuid().ToString();
        }
    }
}