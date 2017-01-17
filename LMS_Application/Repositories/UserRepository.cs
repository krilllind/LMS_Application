using LMS_Application.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LMS_Application.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext _context;

        public UserRepository()
        {
            this._context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets all roles from the database
        /// </summary>
        /// <returns>
        /// Returns a list of IdentityRole
        /// </returns>
        public List<IdentityRole> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        /// <summary>
        /// Gets all role names from the database
        /// </summary>
        /// <returns>
        /// Returns a list of role names
        /// </returns>
        public List<string> GetAllRoleNames()
        {
            return _context.Roles.Select(o => o.Name).ToList();
        }

        /// <summary>
        /// Checks if new user ssn, email or phonenumber is in the database
        /// </summary>
        /// <param name="model">
        /// RegisterViewModel to check against
        /// </param>
        /// <returns>
        /// Returns a Dictionary containing error messages, empty if none
        /// </returns>
        public Dictionary<string, string> CheckUserExistance(RegisterViewModel model)
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

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <param name="roleFilter">
        /// Filter on rolename, null returns all
        /// </param>
        /// <returns>
        /// Returns a IQueryable of ApplicationUsers
        /// </returns>
        private IEnumerable<ApplicationUser> GetUsers(string roleFilter = null)
        {
            return (roleFilter == null) ? _context.Users.ToList() : _context.Users
                .ToList()
                .Where(u => (System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().IsInRole(u.Id, roleFilter)) == true);
        }

        /// <summary>
        /// Gets a list of ApplicationUsers from the database
        /// </summary>
        /// <param name="roleFilter">
        /// Filter on rolename, null returns all
        /// </param>
        /// <returns>
        /// Returns a list of ApplicationUsers
        /// </returns>
        public IEnumerable<object> GetAllUsers(string roleFilter = null)
        {
            return GetUsers(roleFilter).Select(user => new
            {
                ID = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                SSN = user.SSN,
                ProfileImage = user.ProfileImageID ?? "http://placehold.it/100x100",
                UserRole = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(user.Id).SingleOrDefault(),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                SchoolClassID = user.SchoolClassID
            });
        }

        /// <summary>
        /// Gets a ApplicationUser from the database
        /// </summary>
        /// <param name="userID">
        /// ApplicationUser id
        /// </param>
        /// <returns>
        /// Returns a ApplicationUser
        /// </returns>
        public async Task<object> GetUserAsync(string userID)
        {
            ApplicationUser RequestedUser = _context.Users.Find(userID);
            var userRoles = await System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRolesAsync(RequestedUser.Id);

            return new {
                ID = RequestedUser.Id,
                Firstname = RequestedUser.Firstname,
                Lastname = RequestedUser.Lastname,
                SSN = RequestedUser.SSN,
                ProfileImage = RequestedUser.ProfileImageID ?? "http://placehold.it/100x100",
                RequestedUserRole = userRoles.ToList().SingleOrDefault(),
                Email = RequestedUser.Email,
                PhoneNumber = RequestedUser.PhoneNumber,
                UserName = RequestedUser.UserName,
                SchoolClassID = RequestedUser.SchoolClassID
            };
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
    }
}