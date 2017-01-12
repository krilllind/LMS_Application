using LMS_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS_Application.Repositories;

namespace LMS_Application.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private Repository _repo;
        private ApplicationDbContext _context;

        public FileController()
        {
            this._repo = new Repository();
            this._context = new ApplicationDbContext();
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
        public ActionResult UploadFiles()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];

            }

            _repo.UploadFiles(hfc, Server.MapPath("~/Resources/Tmp/"));
            return View();
        }

        public ActionResult DownloadFile(string fileName)
        {
            //Retrieve file with corrisponding filename
            var file = _context.FilesObjects.Where(f => f.Filename == fileName).First();

            //Return file
            return File(file.Data, file.MIME_Type, file.Filename);
        }
    }
}