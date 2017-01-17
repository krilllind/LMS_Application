using LMS_Application.Filters;
using LMS_Application.Models;
using LMS_Application.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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
        public async Task<ActionResult> Create(CourseModels course)
        {
            bool isCreated = await _repo.CreateCourseAsync(course);

            if (isCreated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Course has been created");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course could not be created");
        }

        [HttpPost]
        public ActionResult Update(CourseModels course)
        {
            bool isUpdated = _repo.UpdateCourse(course);

            if (isUpdated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Course has been updated.");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course could not be updated.");
        }

        [HttpPost]
        public async Task<ActionResult> Remove(CourseModels course)
        {
            bool isRemoved = await _repo.RemoveCourseAsync(course);

            if (isRemoved)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Course has been removed");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Course could not be removed");
        }

        [HttpGet]
        [ValidateAngularAntiForgery]
        public string GetSchedule(string schoolClassID = null)
        {
            return JsonConvert.SerializeObject(_repo.GetAll(schoolClassID), Formatting.None, _jsonSettings);
        }

        [HttpGet]
        [ValidateAngularAntiForgery]
        public string GetMySchedule()
        {
            return JsonConvert.SerializeObject(_repo.GetAllMyCourses(User.Identity.GetUserId()), Formatting.None, _jsonSettings);
        }
    }
}