namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseModels",
                c => new
                    {
                        CourseID = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        CourseLevel = c.String(),
                        SubjectStartsAt = c.DateTime(nullable: false),
                        SubjectEndsAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CourseID);
            
            CreateTable(
                "dbo.ScheduleModels",
                c => new
                    {
                        ScheduleID = c.String(nullable: false, maxLength: 128),
                        ValidUntil = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleID)
                .ForeignKey("dbo.SchoolClassModels", t => t.ScheduleID)
                .Index(t => t.ScheduleID);
            
            CreateTable(
                "dbo.SchoolClassModels",
                c => new
                    {
                        SchoolClassID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SchoolClassID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        ProfileImage = c.String(),
                        SSN = c.String(),
                        SchoolClassID = c.String(maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        CourseModels_CourseID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SchoolClassModels", t => t.SchoolClassID)
                .ForeignKey("dbo.CourseModels", t => t.CourseModels_CourseID)
                .Index(t => t.SchoolClassID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.CourseModels_CourseID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.FileObjectModels",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UserID = c.String(maxLength: 128),
                        CourseID = c.String(maxLength: 128),
                        Filename = c.String(nullable: false, maxLength: 255),
                        MIME_Type = c.String(nullable: false, maxLength: 100),
                        Data = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.CourseModels", t => t.CourseID)
                .Index(t => t.UserID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ScheduleModelsCourseModels",
                c => new
                    {
                        ScheduleModels_ScheduleID = c.String(nullable: false, maxLength: 128),
                        CourseModels_CourseID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ScheduleModels_ScheduleID, t.CourseModels_CourseID })
                .ForeignKey("dbo.ScheduleModels", t => t.ScheduleModels_ScheduleID, cascadeDelete: true)
                .ForeignKey("dbo.CourseModels", t => t.CourseModels_CourseID, cascadeDelete: true)
                .Index(t => t.ScheduleModels_ScheduleID)
                .Index(t => t.CourseModels_CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FileObjectModels", "CourseID", "dbo.CourseModels");
            DropForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CourseModels_CourseID", "dbo.CourseModels");
            DropForeignKey("dbo.ScheduleModels", "ScheduleID", "dbo.SchoolClassModels");
            DropForeignKey("dbo.AspNetUsers", "SchoolClassID", "dbo.SchoolClassModels");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ScheduleModelsCourseModels", "CourseModels_CourseID", "dbo.CourseModels");
            DropForeignKey("dbo.ScheduleModelsCourseModels", "ScheduleModels_ScheduleID", "dbo.ScheduleModels");
            DropIndex("dbo.ScheduleModelsCourseModels", new[] { "CourseModels_CourseID" });
            DropIndex("dbo.ScheduleModelsCourseModels", new[] { "ScheduleModels_ScheduleID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FileObjectModels", new[] { "CourseID" });
            DropIndex("dbo.FileObjectModels", new[] { "UserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "CourseModels_CourseID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "SchoolClassID" });
            DropIndex("dbo.ScheduleModels", new[] { "ScheduleID" });
            DropTable("dbo.ScheduleModelsCourseModels");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FileObjectModels");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SchoolClassModels");
            DropTable("dbo.ScheduleModels");
            DropTable("dbo.CourseModels");
        }
    }
}
