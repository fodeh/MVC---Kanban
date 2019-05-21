namespace KanbanBoardWithSignalRAngularJSSol.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "CaseNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "CaseNumber");
        }
    }
}
