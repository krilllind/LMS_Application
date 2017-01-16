namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FileObjectUserModels", name: "UserID", newName: "SSN");
            RenameIndex(table: "dbo.FileObjectUserModels", name: "IX_UserID", newName: "IX_SSN");
            AddColumn("dbo.FileObjectUserModels", "CourseID", c => c.String(maxLength: 128));
            CreateIndex("dbo.FileObjectUserModels", "CourseID");
            AddForeignKey("dbo.FileObjectUserModels", "CourseID", "dbo.CourseModels", "CourseID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileObjectUserModels", "CourseID", "dbo.CourseModels");
            DropIndex("dbo.FileObjectUserModels", new[] { "CourseID" });
            DropColumn("dbo.FileObjectUserModels", "CourseID");
            RenameIndex(table: "dbo.FileObjectUserModels", name: "IX_SSN", newName: "IX_UserID");
            RenameColumn(table: "dbo.FileObjectUserModels", name: "SSN", newName: "UserID");
        }
    }
}
