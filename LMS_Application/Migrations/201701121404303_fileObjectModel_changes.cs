namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fileObjectModel_changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.FileObjectModels", new[] { "UserID" });
            AlterColumn("dbo.FileObjectModels", "UserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.FileObjectModels", "UserID");
            AddForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.FileObjectModels", new[] { "UserID" });
            AlterColumn("dbo.FileObjectModels", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.FileObjectModels", "UserID");
            AddForeignKey("dbo.FileObjectModels", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
