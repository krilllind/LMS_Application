using LMS_Application.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LMS_Application.Repositories
{
    public class DataRepository
    {
        private ApplicationDbContext _context;

        public DataRepository()
        {
            this._context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets all school classes from the database
        /// </summary>
        /// <returns>
        /// Returns a list of SchoolClassModels
        /// </returns>
        public List<SchoolClassModels> GetAllSchoolClasses()
        {
            return _context.SchoolClasses.ToList();
        }

        /// <summary>
        /// Gets all courses from the database
        /// </summary>
        /// <returns>
        /// Returns a list of CourseModels
        /// </returns>
        public List<CourseModels> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        /// <summary>
        /// Removes a school class from the database
        /// </summary>
        /// <param name="schoolClass">
        /// School class to remove
        /// </param>
        /// <returns>
        /// Returns a bool indicating success or not
        /// </returns>
        public async Task<bool> RemoveSchoolClassByIdAsync(SchoolClassModels schoolClass)
        {
            SchoolClassModels c = await _context.SchoolClasses.SingleAsync(o => o.SchoolClassID == schoolClass.SchoolClassID);
            _context.SchoolClasses.Remove(c);
            await _context.SaveChangesAsync();

            return !(_context.SchoolClasses.Where(o => o.SchoolClassID == schoolClass.SchoolClassID).Any());   
        }

        /// <summary>
        /// Updates a school class in the database
        /// </summary>
        /// <param name="schoolClass">
        /// School class to update
        /// </param>
        /// <returns>
        /// Returns a bool indicating success or notdd
        /// </returns>
        public bool UpdateSchoolClass(SchoolClassModels schoolClass)
        {
            _context.Entry(schoolClass).State = EntityState.Modified;
            _context.SaveChanges();

            return _context.SchoolClasses.Where(o => o.SchoolClassID == schoolClass.SchoolClassID).Any();
        }

        /// <summary>
        /// Adds students to a school class
        /// </summary>
        /// <param name="classID">
        /// ID of school class
        /// </param>
        /// <param name="studentSSN">
        /// Social security number of students
        /// </param>
        /// <returns>
        /// void
        /// </returns>
        public void AddStudentsToClass(string classID, string[] studentSSN)
        {
            SchoolClassModels schoolClass = GetSchoolClassById(classID);
            foreach (ApplicationUser user in _context.Users.Where(o => studentSSN.Contains(o.SSN)) as List<ApplicationUser>)
		        schoolClass.Students.Add(user);

            _context.SaveChanges();
        }

        /// <summary>
        /// Removes a students from a school class
        /// </summary>
        /// <param name="classID">
        /// ID of school class
        /// </param>
        /// <param name="studentSSN">
        /// Social security number of students
        /// </param>
        /// <returns>
        /// void
        /// </returns>
        public void RemoveStudentsFromClass(string classID, string[] studentSSN)
        {
            SchoolClassModels schoolClass = GetSchoolClassById(classID);
            foreach (ApplicationUser user in _context.Users.Where(o => studentSSN.Contains(o.SSN)) as List<ApplicationUser>)
                schoolClass.Students.Remove(user);

            _context.SaveChanges();
        }

        /// <summary>
        /// Gets school class from id
        /// </summary>
        /// <param name="classID">
        /// School Class ID
        /// </param>
        /// <returns>
        /// Returns a SchoolClassModels
        /// </returns>
        private SchoolClassModels GetSchoolClassById(string classID)
        {
            return _context.SchoolClasses.Single(o => o.SchoolClassID == classID);
        }

        /// <summary>
        /// Adds a new school class to the database
        /// </summary>
        /// <param name="model">
        /// The school class
        /// </param>
        /// <returns>
        /// Returns a bool value indicating success or not
        /// </returns>
        public async Task<bool> CreateNewSchoolClassAsync(SchoolClassModels model)
        {
            _context.SchoolClasses.Add(model);
            await _context.SaveChangesAsync();

            return _context.SchoolClasses.Where(o => o.Name == model.Name).Any();
        }
    }
}