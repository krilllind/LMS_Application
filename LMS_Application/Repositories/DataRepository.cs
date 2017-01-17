using LMS_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
        /// Gets all courses from specific user
        /// </summary>
        /// <param name="userID">
        /// Users ID
        /// </param>
        /// <returns>
        /// Returns a list of CourseModels
        /// </returns>
        public List<CourseModels> GetAllMyCourses(string userID)
        {
            try
            {
                return _context.SchoolClasses
                    .ToList()
                        .SingleOrDefault(o => o.Students
                            .Contains(_context.Users.Find(userID)))
                            .Courses
                            .ToList();
            }
            catch (Exception) {
                return null;
            }
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
            _context.SchoolClasses.SingleOrDefault(cls => cls.SchoolClassID == classID)
                .Students
                .AddRange(_context.Users
                    .Where(user => studentSSN
                        .Contains(user.SSN)));
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
            _context.SchoolClasses.SingleOrDefault(cls => cls.SchoolClassID == classID)
                .Students
                .RemoveAll(user => studentSSN
                    .Contains(user.SSN));
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