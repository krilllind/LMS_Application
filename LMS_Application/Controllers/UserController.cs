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

namespace LMS_Application.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserRepository _repo;
        private JsonSerializerSettings _jsonSettings;

        public UserController()
        {
            this._repo = new UserRepository();
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
        public async Task<string> GetAll(string roleFilter)
        {
            var users = await _repo.GetAllUsersAsync(roleFilter);
            return JsonConvert.SerializeObject(users, Formatting.None, _jsonSettings);
        }

        [HttpGet]
        public async Task<string> GetSignedIn()
        {
            string userID = User.Identity.GetUserId();
            object currentUser = await _repo.GetUserAsync(userID);
            return JsonConvert.SerializeObject(currentUser, Formatting.None, _jsonSettings);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ApplicationUser user, string userRole)
        {
            bool isUpdated = await _repo.UpdateUserAsync(user, userRole);

            if (isUpdated)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "User has been updated.");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "User could not be updated.");
        }

        [HttpPost]
        public async Task<ActionResult> Remove(ApplicationUser user)
        {
            bool isRemoved = await _repo.RemoveUserAsync(user);

            if (isRemoved)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "User has been removed");

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "User could not be removed");
        }
    }
}