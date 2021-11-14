namespace Job_Offers_Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditNameOfTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "JobTime", c => c.String());
            DropColumn("dbo.Jobs", "JobImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "JobImage", c => c.String());
            DropColumn("dbo.Jobs", "JobTime");
        }
    }
}
