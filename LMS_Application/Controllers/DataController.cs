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
        private DataRepository _repo;
        private JsonSerializerSettings _jsonSettings;

        public DataController()
        {
            this._repo = new DataRepository();
            this._jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        [HttpGet]
        public string GetAllClasses()
        {
            List<SchoolClassModels> SchoolClasses = _repo.GetAllSchoolClasses();
            List<object> tmp = new List<object>();

            foreach (var SchoolClass in SchoolClasses)
            {
                tmp.Add(new {
                    SchoolClassID = SchoolClass.SchoolClassID,
                    Name = SchoolClass.Name,
                    ValidTo = SchoolClass.ValidTo,
                    Students = SchoolClass.Students
                });
            }

            return JsonConvert.SerializeObject(tmp, Formatting.None, _jsonSettings);
        }

        [HttpPost]
        public ActionResult AddStudentsToClass(string classID, string[] studentSSN)
        {
            _repo.AddStudentsToClass(classID, studentSSN);
            return new HttpStatusCodeResult(HttpStatusCode.OK, String.Format("Student{0} added to class", (studentSSN.Length > 1) ? "s" : ""));
        }

        [HttpPost]
        public ActionResult RemoveStudentsFromClass(string classID, string[] studentSSN)
        {
            _repo.RemoveStudentsFromClass(classID, studentSSN);
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Student removed from class");
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewSchoolClass(SchoolClassModels model)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await _repo.CreateNewSchoolClassAsync(model);

                if (isCreated)
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "New school class created");
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Could not create new school class");
        }

        [HttpPost]
        public ActionResult UpdateSchoolClass(SchoolClassModels schoolClass)
        {
            bool isUpdated = _repo.UpdateSchoolClass(schoolClass);

            if (isUpdated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "School class successfully updated");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Could not update school class");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveSchoolClass(SchoolClassModels schoolClass)
        {
            bool isRemoved = await _repo.RemoveSchoolClassByIdAsync(schoolClass);

            if (isRemoved)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "School class successfully removed");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Could not remove school class");
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
    }
}