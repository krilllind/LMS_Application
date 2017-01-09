namespace LMS_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToSchoolClassModel_ScheduleModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchoolClassModels", "ValidTo", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SchoolClassModels", "Name", c => c.String(nullable: false));
            DropColumn("dbo.ScheduleModels", "ValidUntil");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScheduleModels", "ValidUntil", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SchoolClassModels", "Name", c => c.String());
            DropColumn("dbo.SchoolClassModels", "ValidTo");
        }
    }
}
