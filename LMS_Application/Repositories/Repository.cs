using LMS_Application.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LMS_Application.Repositories
{
    public class Repository
    {
        private ApplicationDbContext _context;

        public Repository()
        {
            this._context = new ApplicationDbContext();
        }

        /// <summary>
        /// Returns the filenames of all files currently in the database
        /// </summary>
        /// <returns></returns>
        public string[] GetAllFilenames()
        {
            List<FileObjectModels> files = _context.FilesObjects.ToList();
            string[] filenames = new string[files.Count()];
            int index = 0;
            foreach (var file in files)
            {
                filenames[index] = file.Filename;
                index++;
            }
            return filenames;
        }

        /// <summary>
        /// Returns a URL to the file with corresponding filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetUrlByFilename(string fileName = "ppap.png")
        {
            FileObjectModels file = _context.FilesObjects.Single(f => f.Filename == fileName);
            if (file != null)
                return string.Format("data:{0};base64,{1}", file.MIME_Type, Convert.ToBase64String(file.Data));
            return null;
        }

        /// <summary>
        /// Converts byte[] bytes to a readable string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string DeBlobber(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
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
        public async Task<List<FileObjectModels>> GenerateFileObjectFromFilesAsync(HttpFileCollectionBase files, string userId)
        {
            List<FileObjectModels> FileObjects = new List<FileObjectModels>();

            try
            {
                foreach (string key in files)
                {
                    var fileContent = files[key];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var stream = fileContent.InputStream;
                        var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Resources/Temp/"), fileContent.FileName);

                        using (var fileStream = System.IO.File.Create(path))
                        {
                            BinaryReader reader = new BinaryReader(fileStream);

                            // Bytes from the binary reader stored in BlobValue array //
                            byte[] BlobValue = reader.ReadBytes(fileContent.ContentLength);

                            reader.Close();
                            fileStream.Close();

                            FileObjects.Add(new FileObjectModels()
                            {
                                MIME_Type = fileContent.ContentType,
                                Data = BlobValue,
                                Filename = fileContent.FileName,
                                UserID = userId,

                            });

                            // Remove temp file //
                            System.IO.File.Delete(path);
                        }
                    }
                }
            }
            catch (Exception) {}

            return FileObjects;
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
            _context.FilesObjects.AddRange(files);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Creates and returns a new FileObjectModel based on input
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bytes"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private FileObjectModels CreateModel(string fileName, byte[] bytes, string contentType)
        {
            //Create relevant model
            FileObjectModels model = new FileObjectModels();
            model.Filename = fileName;
            model.Data = bytes;
            model.MIME_Type = contentType;
            return model;
        }

        /// <summary>
        /// Check and Delete file based on filepath
        /// </summary>
        /// <param name="fullPath"></param>
        private bool DeleteFile(string filepath)
        {
            //Check if file exists, if it does delete it
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if file already exists in database. If not, add to database
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        private bool DataExists(byte[] bytes, string fileName, string contentType)
        {
            //Check if model already exists
            bool dataExists = _context.FilesObjects.Any(m => m.Data.Equals(bytes));
            if (!dataExists)
            {
                _context.FilesObjects.Add(CreateModel(fileName, bytes, contentType));
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns an array with the names of the roles in the database
        /// </summary>
        /// <returns></returns>
        public List<IdentityRole> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public List<string> GetAllRoleNames()
        {
            return _context.Roles.Select(o => o.Name).ToList();
        }

        public bool CheckUserExistance(RegisterViewModel model)
        {
            if (_context.Users.Where(u => u.SSN == model.SSN || u.Email == model.Email || u.PhoneNumber == model.PhoneNumber).Any())
                return true;

            return false;
        }

        public Dictionary<string, string> ReturnDuplicateFields(RegisterViewModel model)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (_context.Users.Where(u => u.PhoneNumber == model.PhoneNumber).Any())
                errors.Add("phoneNumber", "Phonenumber already registred.");

            if (_context.Users.Where(u => u.Email == model.Email).Any())
                errors.Add("email", "Email already registred.");

            if (_context.Users.Where(u => u.SSN == model.SSN).Any())
                errors.Add("ssn", "SSN already registred.");

            return errors;
        }

        public List<SchoolClassModels> GetAllSchoolClasses()
        {
            return _context.SchoolClasses.ToList();
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <param name="RoleFilter">
        /// Filter on rolename, null returns all
        /// </param>
        /// <returns>
        /// Returns a list of ApplicationUsers
        /// </returns>
        public List<ApplicationUser> GetAllUsers(string RoleFilter = null)
        {
            List<ApplicationUser> users = _context.Users.ToList();

            if (RoleFilter != null)
            {
                List<ApplicationUser> tmp = new List<ApplicationUser>();

                foreach (ApplicationUser user in users)
                {
                    if (System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(user.Id, RoleFilter))
                    {
                        tmp.Add(user);
                    }
                }

                users = tmp;
            }

            return users;
        }

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        /// <param name="user">
        /// User to update
        /// </param>
        /// <param name="userRole">
        /// User role to update
        /// </param>
        /// <returns>
        /// Returns a bool indicating success or not
        /// </returns>
        public async Task<bool> UpdateUserAsync(ApplicationUser user, string userRole)
        {
            var tmp = await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(user.Id);
            user.SecurityStamp = tmp.SecurityStamp;
            user.PasswordHash = tmp.PasswordHash;
            _context.Entry(user).State = EntityState.Modified;

            await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().RemoveFromRolesAsync(user.Id, _context.Roles.Select(o => o.Name).ToArray());
            await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().AddToRoleAsync(user.Id, userRole);
            await _context.SaveChangesAsync();

            return _context.Users.Where(o => o.SSN == user.SSN).Any();
        }

        /// <summary>
        /// Removes a user from the database.
        /// </summary>
        /// <param name="user">
        /// User to remove
        /// </param>
        /// <returns>
        /// Returns a bool indicating success or not
        /// </returns>
        public async Task<bool> RemoveUserAsync(ApplicationUser user)
        {
            await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().RemoveFromRolesAsync(user.Id, _context.Roles.Select(o => o.Name).ToArray());
            ApplicationUser u = await _context.Users.SingleAsync(o => o.SSN == user.SSN);
            _context.Users.Remove(u);
            await _context.SaveChangesAsync();

            return !(_context.Users.Where(o => o.SSN == user.SSN).Any());
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
    }
}