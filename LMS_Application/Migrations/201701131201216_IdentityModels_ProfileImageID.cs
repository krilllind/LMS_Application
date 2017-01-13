namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityModels_ProfileImageID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfileImageID", c => c.String());
            DropColumn("dbo.AspNetUsers", "ProfileImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ProfileImage", c => c.String());
            DropColumn("dbo.AspNetUsers", "ProfileImageID");
        }
    }
}
