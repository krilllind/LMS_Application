using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Application.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfileImageID { get; set; }
        public string SSN { get; set; }

        [ForeignKey("SchoolClassModels")]
        public string SchoolClassID { get; set; }

        public virtual SchoolClassModels SchoolClassModels { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<FileObjectModels> FilesObjects { get; set; }
        public DbSet<FileObjectUserModels> FileObjectUsers { get; set; }
        public DbSet<CourseModels> Courses { get; set; }
        public DbSet<SchoolClassModels> SchoolClasses { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) {}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}