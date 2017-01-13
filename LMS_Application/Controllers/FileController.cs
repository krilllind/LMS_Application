using LMS_Application.Models;
using LMS_Application.Repositories;
using Microsoft.AspNet.Identity;
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
        private Repository _repo;

        public FileController()
        {
            this._repo = new Repository();
        }

        [HttpGet]
        public string GetUrlByFilename(string fileName)
        {
            return _repo.GetUrlByFilename(fileName);
        }

        [HttpGet]
        public string[] GetAllFilenames()
        {
            return _repo.GetAllFilenames();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles()
        {
            List<FileObjectModels> FileObjects = await _repo.GenerateFileObjectFromFilesAsync(Request.Files, User.Identity.GetUserId());

            if(FileObjects.Any())
            {
                await _repo.UploadFilesAsync(FileObjects);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK, "File successfully uploaded");
        }
    }
}