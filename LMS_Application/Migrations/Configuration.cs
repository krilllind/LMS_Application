namespace LMS_Application.Migrations
{
    using LMS_Application.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS_Application.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS_Application.Models.ApplicationDbContext context)
        {
            // Create handlers //
            var UserStore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(UserStore);

            // Create a schoolclass //
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "NA3B",  ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "TE2B", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "BF1A", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "BF1B", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "DA9B", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "TE2A", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "TE3A", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "ROL2B", ValidTo = new DateTime(2017, 12, 01) });
            context.SchoolClasses.Add(new SchoolClassModels() { Name = "ROL2A", ValidTo = new DateTime(2017, 12, 01) });

            // Create a teacher user //
            var TmpUser = new ApplicationUser()
            {
                UserName = "test@test.com",
                Email = "test@test.com",
                PhoneNumber = "0701234567",
                Firstname = "Test",
                Lastname = "User",
                ProfileImageID = null,
                SSN = "185201052663"
            };

            // Create some students
            var student1 = new ApplicationUser() { UserName = "test2@test.com", Email = "test2@test.com", PhoneNumber = "1231231231", Firstname = "Ben", Lastname = "Andersson", SSN = "123412341234" };
            var student2 = new ApplicationUser() { UserName = "test3@test.com", Email = "test3@test.com", PhoneNumber = "2342342342", Firstname = "Henrik", Lastname = "Pettersson", SSN = "234523452345" };
            var student3 = new ApplicationUser() { UserName = "test4@test.com", Email = "test4@test.com", PhoneNumber = "3453453453", Firstname = "Ylva", Lastname = "Larsson", SSN = "435634563456" };
            var student4 = new ApplicationUser() { UserName = "test5@test.com", Email = "test5@test.com", PhoneNumber = "4564564564", Firstname = "Stina", Lastname = "Gustavsson", SSN = "456745675467" };
            var student5 = new ApplicationUser() { UserName = "test6@test.com", Email = "test6@test.com", PhoneNumber = "5675675675", Firstname = "Nisse", Lastname = "Nilsson", SSN = "567856785678" };
            var student6 = new ApplicationUser() { UserName = "test7@test.com", Email = "test7@test.com", PhoneNumber = "6786786786", Firstname = "Eva", Lastname = "Lundström", SSN = "678967896789" };
            var student7 = new ApplicationUser() { UserName = "test8@test.com", Email = "test8@test.com", PhoneNumber = "7897897897", Firstname = "Ida", Lastname = "Öhman", SSN = "789078907890" };
            var student8 = new ApplicationUser() { UserName = "test9@test.com", Email = "test9@test.com", PhoneNumber = "8908908908", Firstname = "Jenny", Lastname = "Gunnarsson", SSN = "098709870987" };
            var student9 = new ApplicationUser() { UserName = "test10@test.com", Email = "test10@test.com", PhoneNumber = "6545646546", Firstname = "Gunnar", Lastname = "Fredriksson", SSN = "987689769876" };

            // Set password for user 'TmpUser' to 'Test@123' //
            UserManager.Create(TmpUser, "Test@123");
            UserManager.Create(student1, "Test@123");
            UserManager.Create(student2, "Test@123");
            UserManager.Create(student3, "Test@123");
            UserManager.Create(student4, "Test@123");
            UserManager.Create(student5, "Test@123");
            UserManager.Create(student6, "Test@123");
            UserManager.Create(student7, "Test@123");
            UserManager.Create(student8, "Test@123");
            UserManager.Create(student9, "Test@123");

            // Create roll Teacher //
            if (!context.Roles.Any(o => o.Name == "Teacher"))
            {
                var RoleStore = new RoleStore<IdentityRole>(context);
                var RoleManager = new RoleManager<IdentityRole>(RoleStore);

                var Role = new IdentityRole() { Name = "Teacher" };

                RoleManager.Create(Role);
            }

            // Create roll Student //
            if (!context.Roles.Any(o => o.Name == "Student"))
            {
                var RoleStore = new RoleStore<IdentityRole>(context);
                var RoleManager = new RoleManager<IdentityRole>(RoleStore);

                var Role = new IdentityRole() { Name = "Student" };

                RoleManager.Create(Role);
            }


            // Set user roles //
            UserManager.AddToRole(TmpUser.Id, "Teacher");
            UserManager.AddToRole(student1.Id, "Student");
            UserManager.AddToRole(student2.Id, "Student");
            UserManager.AddToRole(student3.Id, "Student");
            UserManager.AddToRole(student4.Id, "Student");
            UserManager.AddToRole(student5.Id, "Student");
            UserManager.AddToRole(student6.Id, "Student");
            UserManager.AddToRole(student7.Id, "Student");
            UserManager.AddToRole(student8.Id, "Student");
            UserManager.AddToRole(student9.Id, "Student");

            // Save all changes //
            context.SaveChanges();
        }
    }
}
