using LMS_Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using LMS_Application.Repositories;
using LMS_Application.Filters;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace LMS_Application.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private Repository _repo;
        private ApplicationDbContext _context;
        private JsonSerializerSettings _jsonSettings;

        public DataController()
        {
            this._repo = new Repository();
            this._context = new ApplicationDbContext();
            this._jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        [HttpGet]
        public string GetAllRoleNames()
        {
            var roles = _repo.GetAllRoleNames();
            return JsonConvert.SerializeObject(roles, Formatting.None, _jsonSettings);
        }

        [HttpGet]
        public string GetAllClasses()
        {
            List<SchoolClassModels> SchoolClasses = _repo.GetAllSchoolClasses();
            List<object> tmp = new List<object>();

            foreach (var SchoolClass in SchoolClasses)
            {
                tmp.Add(new {
                    ID = SchoolClass.SchoolClassID,
                    Name = SchoolClass.Name,
                    Students = SchoolClass.Students
                });
            }

            return JsonConvert.SerializeObject(tmp, Formatting.None, _jsonSettings);
        }

        [HttpGet]
        public async Task<string> GetAllUsers(string roleFilter = null)
        {
            List<ApplicationUser> Users = _repo.GetAllUsers(roleFilter);
            List<object> tmp = new List<object>();

            foreach (var user in Users)
            {
                var rolesForUser = await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRolesAsync(user.Id);

                tmp.Add(new {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    SSN = user.SSN,
                    ProfileImage = user.ProfileImage ?? "http://placehold.it/100x100",
                    UserRole = rolesForUser.ToList().SingleOrDefault(),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    SchoolClassID = user.SchoolClassID
                });
            }

            return JsonConvert.SerializeObject(tmp, Formatting.None, _jsonSettings);
        }

        [HttpPost]
        public ActionResult AddStudentsToClass(string classID, string studentSSN)
        {
            ApplicationUser student = _context.Users.Single(o => o.SSN == studentSSN);
            _context.SchoolClasses.Single(o => o.SchoolClassID == classID).Students.Add(student);
            _context.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Student added to class");
        }

        [HttpPost]
        public ActionResult RemmoveStudentsFromClass(string classID, string studentSSN)
        {
            ApplicationUser student = _context.Users.Single(o => o.SSN == studentSSN);
            _context.SchoolClasses.Single(o => o.SchoolClassID == classID).Students.Remove(student);
            _context.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Student removed from class");
        }

        [HttpPost]
        public ActionResult CreateNewSchoolClass(SchoolClassModels model)
        {
            if (ModelState.IsValid)
            {
                _context.SchoolClasses.Add(model);
                _context.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK, "New school class created");
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Could not create new school class");
        }

        [HttpGet]
        public async Task<string> GetUserInformation()
        {
            var User_id = User.Identity.GetUserId();
            var rolesForUser = await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRolesAsync(User_id);

            var CurrentUser = _context.Users.Select(o => new
            {
                Id = o.Id,
                Firstname = o.Firstname,
                Lastname = o.Lastname,
                ProfileImage = o.ProfileImage ?? "http://placehold.it/100x100",
                UserRole = rolesForUser.ToList().FirstOrDefault()
            }).Where(o => o.Id == User_id);

            return JsonConvert.SerializeObject(CurrentUser, Formatting.None, _jsonSettings);
        }

        [HttpGet]
        [ValidateAngularAntiForgery]
        public string GetSchedule()
        {
            List<object> test = new List<object>() {
                new {
                    From = "10:30",
                    To = "12:00",
                    Day = "Tuesday",
                    LessonType = "English",
                    Color = "lightblue",
                    Teacher = "TLUG",
                    Classroom = "C320"
                },
                new {
                    From = "10:30",
                    To = "15:20",
                    Day = "Friday",
                    LessonType = "Programming",
                    Color = "pink",
                    Teacher = "ELÖV",
                    Classroom = "D220"
                },
                new {
                    From = "08:30",
                    To = "09:45",
                    Day = "Monday",
                    LessonType = "Math",
                    Color = "lightgreen",
                    Teacher = "POLV",
                    Classroom = "A332"
                }
            };

            return JsonConvert.SerializeObject(test, Formatting.None, _jsonSettings);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(ApplicationUser user, string userRole)
        {
            bool isUpdated = await _repo.UpdateUserAsync(user, userRole);

            if (isUpdated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "User has been updated.");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "User could not be updated.");
        }
    }
}