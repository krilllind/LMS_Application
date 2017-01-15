using LMS_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LMS_Application.Repositories
{
    public class FileRepository
    {
        private ApplicationDbContext _context;

        public FileRepository()
        {
            this._context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets all filenames from the database
        /// </summary>
        /// <returns>
        /// Returns a list of all filenames
        /// </returns>
        public List<string> GetAllFilenames()
        {
            return _context.FilesObjects.Select(o => o.Filename).ToList();
        }

        /// <summary>
        /// Generates a URL to the file with corresponding filename
        /// </summary>
        /// <param name="fileName">
        /// Name of the file
        /// </param>
        /// <returns>
        /// Returns a string url
        /// </returns>
        public string GenerateFileUrl(string fileName)
        {
            FileObjectModels file = _context.FilesObjects.SingleOrDefault(o => o.Filename == fileName);
            return (file != null) ? string.Format("data:{0};base64,{1}", file.MIME_Type, Convert.ToBase64String(file.Data)) : null;
        }

        /// <summary>
        /// Generates a list of FileObjectModels containing blob data
        /// </summary>
        /// <param name="files">
        /// HttpFileCollectionBase of FormData files
        /// </param>
        /// <param name="userId">
        /// The id of the associated application user
        /// </param>
        /// <returns>
        /// Returns a list of FileObjectModels with blob data
        /// </returns>
        public List<FileObjectModels> GenerateFileObjectFromFiles(HttpFileCollectionBase files, string userId)
        {
            List<FileObjectModels> FileObjects = new List<FileObjectModels>();

            foreach (string key in files)
            {
                var file = files[key];
                FileObjects.Add(new FileObjectModels()
                {
                    MIME_Type = file.ContentType,
                    Data = FileToBlob(file),
                    Filename = file.FileName,
                    UserID = userId,
                });
            }

            return FileObjects;
        }

        /// <summary>
        /// Converts file into blob format
        /// </summary>
        /// <param name="file">
        /// File to convert
        /// </param>
        /// <returns>
        /// Returns an array of bytes
        /// </returns>
        public byte[] FileToBlob(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            return target.ToArray();
        }

        /// <summary>
        /// Upload files to database
        /// </summary>
        /// <param name="files">
        /// List of files to upload
        /// </param>
        /// <returns>
        /// void
        /// </returns>
        public async Task UploadFilesAsync(List<FileObjectModels> files)
        {
            foreach(FileObjectModels file in files)
            {
                if (_context.FilesObjects.Where(o => o.Data == file.Data).Any())
                    continue;
                _context.FilesObjects.Add(file);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a file from the database
        /// </summary>
        /// <param name="fileID">
        /// ID of the file
        /// </param>
        /// <returns>
        /// void
        /// </returns>
        public async Task DeleteFileAsync(string fileID)
        {
            _context.FilesObjects.Remove(await _context.FilesObjects.FindAsync(fileID));
        }
    }
}