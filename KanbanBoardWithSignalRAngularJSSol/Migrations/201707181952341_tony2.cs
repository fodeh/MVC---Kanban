namespace KanbanBoardWithSignalRAngularJSSol.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tony2 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.EmployeeViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EmployeeViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeName = c.String(),
                        EmployeeImage = c.Binary(),
                        TotalHours = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
