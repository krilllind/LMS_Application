using LMS_Application.Models;
using LMS_Application.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS_Application.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private FileRepository _repo;

        public FileController()
        {
            this._repo = new FileRepository();
        }

        [HttpGet]
        public string GenerateFileUrl(string fileName)
        {
            return _repo.GenerateFileUrl(fileName);
        }

        [HttpGet]
        public List<string> GetAllFilenames()
        {
            return _repo.GetAllFilenames();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles()
        {
            List<FileObjectModels> FileObjects = _repo.GenerateFileObjectFromFiles(Request.Files, User.Identity.GetUserId());

            var fd = Request.Form;

            if(FileObjects.Any())
            {
                await _repo.UploadFilesAsync(FileObjects, _repo.GetCurrentUser(User.Identity.GetUserId()), fd["CourseID"], Convert.ToBoolean(fd["Shared"]));
                return new HttpStatusCodeResult(HttpStatusCode.OK, "File successfully uploaded");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "File failed to upload");
            
        }

        [HttpPost]
        public ActionResult RemoveFile()
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed to remove file");
        }

        [HttpGet]
        public ActionResult DownloadFiles()
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed to download file");
        }

        [HttpPost]
        public ActionResult UpdateFile()
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed to update file");
        }
    }
}