namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.FileObjectModels", new[] { "UserID" });
            DropPrimaryKey("dbo.FileObjectModels");
            CreateTable(
                "dbo.FileObjectUserModels",
                c => new
                    {
                        FileObjectUserID = c.String(nullable: false, maxLength: 128),
                        UserID = c.String(maxLength: 128),
                        FileObjectID = c.String(maxLength: 128),
                        Shared = c.Boolean(nullable: false),
                        UploadedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FileObjectUserID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.FileObjectModels", t => t.FileObjectID)
                .Index(t => t.UserID)
                .Index(t => t.FileObjectID);
            
            AddColumn("dbo.FileObjectModels", "FileObjectID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.FileObjectModels", "ContentType", c => c.String(nullable: false, maxLength: 100));
            AddPrimaryKey("dbo.FileObjectModels", "FileObjectID");
            DropColumn("dbo.FileObjectModels", "ID");
            DropColumn("dbo.FileObjectModels", "MIME_Type");
            DropColumn("dbo.FileObjectModels", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileObjectModels", "UserID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.FileObjectModels", "MIME_Type", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.FileObjectModels", "ID", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.FileObjectUserModels", "FileObjectID", "dbo.FileObjectModels");
            DropForeignKey("dbo.FileObjectUserModels", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.FileObjectUserModels", new[] { "FileObjectID" });
            DropIndex("dbo.FileObjectUserModels", new[] { "UserID" });
            DropPrimaryKey("dbo.FileObjectModels");
            DropColumn("dbo.FileObjectModels", "ContentType");
            DropColumn("dbo.FileObjectModels", "FileObjectID");
            DropTable("dbo.FileObjectUserModels");
            AddPrimaryKey("dbo.FileObjectModels", "ID");
            CreateIndex("dbo.FileObjectModels", "UserID");
            AddForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
