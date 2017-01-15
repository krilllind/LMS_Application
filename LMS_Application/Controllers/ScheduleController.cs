using LMS_Application.Models;
using LMS_Application.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net;
using LMS_Application.Filters;

namespace LMS_Application.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private ScheduleRepository _repo;
        private JsonSerializerSettings _jsonSettings;

        public ScheduleController()
        {
            this._repo = new ScheduleRepository();
            this._jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create(CourseModels course, string userID)
        {
            bool isCreated = await _repo.CreateCourseAsync(course, userID);

            if (isCreated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Course has been created");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course could not be created");
        }

        [HttpPost]
        public ActionResult Update(CourseModels course, string userID)
        {
            bool isUpdated = _repo.UpdateCourse(course, userID);

            if (isUpdated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Course has been updated.");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course could not be updated.");
        }

        [HttpPost]
        public async Task<ActionResult> Remove(CourseModels course, string userID)
        {
            bool isRemoved = await _repo.RemoveCourseAsync(course, userID);

            if (isRemoved)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Course has been removed");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course could not be removed");
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