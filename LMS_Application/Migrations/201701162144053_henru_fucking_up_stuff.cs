namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class henru_fucking_up_stuff : DbMigration
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
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Day = c.String(),
                        Classroom = c.String(),
                        Teacher = c.String(),
                        SchoolClassID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CourseID)
                .ForeignKey("dbo.SchoolClassModels", t => t.SchoolClassID)
                .Index(t => t.SchoolClassID);
            
            CreateTable(
                "dbo.SchoolClassModels",
                c => new
                    {
                        SchoolClassID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SchoolClassID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        ProfileImageID = c.String(),
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
                "dbo.FileObjectUserModels",
                c => new
                    {
                        FileObjectUserID = c.String(nullable: false, maxLength: 128),
                        SSN = c.String(maxLength: 128),
                        FileObjectID = c.String(maxLength: 128),
                        CourseID = c.String(maxLength: 128),
                        Shared = c.Boolean(nullable: false),
                        UploadedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FileObjectUserID)
                .ForeignKey("dbo.AspNetUsers", t => t.SSN)
                .ForeignKey("dbo.CourseModels", t => t.CourseID)
                .ForeignKey("dbo.FileObjectModels", t => t.FileObjectID)
                .Index(t => t.SSN)
                .Index(t => t.FileObjectID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.FileObjectModels",
                c => new
                    {
                        FileObjectID = c.String(nullable: false, maxLength: 128),
                        Filename = c.String(nullable: false, maxLength: 255),
                        ContentType = c.String(nullable: false, maxLength: 100),
                        Data = c.Binary(nullable: false),
                        CourseID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FileObjectID)
                .ForeignKey("dbo.CourseModels", t => t.CourseID)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FileObjectUserModels", "FileObjectID", "dbo.FileObjectModels");
            DropForeignKey("dbo.FileObjectModels", "CourseID", "dbo.CourseModels");
            DropForeignKey("dbo.FileObjectUserModels", "CourseID", "dbo.CourseModels");
            DropForeignKey("dbo.FileObjectUserModels", "SSN", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CourseModels_CourseID", "dbo.CourseModels");
            DropForeignKey("dbo.CourseModels", "SchoolClassID", "dbo.SchoolClassModels");
            DropForeignKey("dbo.AspNetUsers", "SchoolClassID", "dbo.SchoolClassModels");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FileObjectModels", new[] { "CourseID" });
            DropIndex("dbo.FileObjectUserModels", new[] { "CourseID" });
            DropIndex("dbo.FileObjectUserModels", new[] { "FileObjectID" });
            DropIndex("dbo.FileObjectUserModels", new[] { "SSN" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "CourseModels_CourseID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "SchoolClassID" });
            DropIndex("dbo.CourseModels", new[] { "SchoolClassID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FileObjectModels");
            DropTable("dbo.FileObjectUserModels");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SchoolClassModels");
            DropTable("dbo.CourseModels");
        }
    }
}
