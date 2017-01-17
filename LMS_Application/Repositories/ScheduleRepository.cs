using LMS_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Application.Repositories
{
    public class ScheduleRepository
    {
        public List<string> ValidationErrorList = new List<string>();
        private ApplicationDbContext _context;

        public ScheduleRepository()
        {
            this._context = new ApplicationDbContext();
        }

        /// <summary>
        /// Adds a new course to the database
        /// </summary>
        /// <param name="course">
        /// The course
        /// </param>
        /// <param name="userID">
        /// Id of the user who created it
        /// </param>
        /// <returns>
        /// Returns a bool value indicating success or not
        /// </returns>
        public async Task<bool> CreateCourseAsync(CourseModels course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return _context.Courses.Where(o => o.CourseID == course.CourseID).Any();
        }

        /// <summary>
        /// Update a course in the database
        /// </summary>
        /// <param name="course">
        /// The course
        /// </param>
        /// <param name="userID">
        /// Id of the user who modified it
        /// </param>
        /// <returns>
        /// Returns a bool value indicating success or not
        /// </returns>
        public bool UpdateCourse(CourseModels course)
        {
            _context.Entry(course).State = EntityState.Modified;
            _context.SaveChanges();

            return _context.Courses.Where(o => o.CourseID == course.CourseID).Any();
        }

        /// <summary>
        /// Removes a course
        /// </summary>
        /// <param name="course">
        /// The course
        /// </param>
        /// <param name="userID">
        /// Id of the user who removed it
        /// </param>
        /// <returns>
        /// Returns a bool value indicating success or not
        /// </returns>
        public async Task<bool> RemoveCourseAsync(CourseModels course)
        {
            CourseModels c = await _context.Courses.SingleAsync(o => o.CourseID == course.CourseID);
            _context.Courses.Remove(c);
            await _context.SaveChangesAsync();

            return !(_context.Courses.Where(o => o.CourseID == course.CourseID).Any());
        }


        /// <summary>
        /// Gets all courses
        /// </summary>
        /// <returns>
        /// Ruturns an IEnumerable of type ICourseModels
        /// </returns>
        public List<CourseModels> GetAll(string schoolClassID = null)
        {
            return _context.Courses.Where(o => o.SchoolClassID == schoolClassID).ToList();
        }

        public List<CourseModels> GetAllMyCourses(string userID)
        {
            List<CourseModels> tmp = new List<CourseModels>();
            List<CourseModels> courses = _context.Courses.ToList();
            ApplicationUser user = _context.Users.Where(u => u.Id == userID).Single();
            List<SchoolClassModels> schoolClasses = _context.SchoolClasses.ToList();

            foreach (CourseModels course in courses)
            {
                foreach (SchoolClassModels schoolClass in schoolClasses)
                {
                    if (!schoolClass.Students.Contains(user))
                        continue;
                    if (course.SchoolClassID == schoolClass.SchoolClassID)
                        tmp.Add(course);
                }
            }
            return tmp;
        }
    }
}