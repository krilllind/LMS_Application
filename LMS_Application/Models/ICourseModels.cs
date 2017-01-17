using System;
namespace LMS_Application.Models
{
    public interface ICourseModels
    {
        DateTime From { get; set; }
        DateTime To { get; set; }
        string Color { get; }
        string Day { get; set; }
        string Subject { get; set; }
        string CourseLevel { get; set; }
        string Classroom { get; set; }
        string Teacher { get; set; }
    }
}
